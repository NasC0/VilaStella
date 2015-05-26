using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using VilaStella.Data.Common.Repositories;
using VilaStella.Models;
using VilaStella.WebAdminClient.Areas.Admin.ViewModels;
using VilaStella.WebAdminClient.Infrastructure.Contracts;
using VilaStella.WebAdminClient.Models;

namespace VilaStella.WebAdminClient.Controllers
{
    public class HomeController : Controller
    {
        private IDeletableRepository<VilaStella.Models.Image> images;
        private IGenericRepositoy<GeneralSettings> settings;
        private IDeletableRepository<Reservation> reservations;
        private IReservationManager reservationManager;
        private IOverlapDatesManager datesManager;
        private IEmailManager emailManager;

        public HomeController(IDeletableRepository<VilaStella.Models.Image> images, IGenericRepositoy<GeneralSettings> settings, IDeletableRepository<Reservation> reservations, IReservationManager reservationManager, IOverlapDatesManager datesManager, IEmailManager emailManager)
        {
            this.images = images;
            this.settings = settings;
            this.reservations = reservations;
            this.reservationManager = reservationManager;
            this.datesManager = datesManager;
            this.emailManager = emailManager;
        }

        public ActionResult Index()
        {
            var currentSetings = this.settings.All()
                .OrderByDescending(x => x.ID)
                .FirstOrDefault();

            bool areReservationsOpen = currentSetings.AreReservationsOpen;
            return View(areReservationsOpen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ReservationsInputModel reservation)
        {
            bool isValidReservation = this.reservationManager.ValidateReservation(ModelState, reservation);

            if (ModelState.IsValid && isValidReservation)
            {
                bool isSeenInNewTab = true;
                var dbResrvation = this.reservationManager.CreateReservation(reservation, isSeenInNewTab);

                this.reservations.Add(dbResrvation);
                this.emailManager.SendConfirmationEmail(dbResrvation);
                this.reservations.SaveChanges();

                return Json(true);
            }

            List<string> errors = new List<string>();

            foreach (var error in ModelState.Values)
            {
                if (error.Errors.Count > 0)
                {
                    for (int i = 0; i < error.Errors.Count; i++)
                    {
                        errors.Add(error.Errors[i].ErrorMessage);
                    }
                }
            }

            return Json(errors);
        }

        public ActionResult RenderGallery()
        {
            var images = this.images.All()
                .Where(x => x.IsShown)
                .OrderByDescending(x => x.ID)
                .Project()
                .To<ImageOutputModel>()
                .ToList();

            return PartialView("_Gallery", images);
        }

        public ActionResult RenderReservation()
        {
            return PartialView("_Reservation");
        }

        public ActionResult CancelReservation(string id)
        {
            Guid guidID;

            if (!Guid.TryParse(id, out guidID))
            {
                return HttpNotFound("Invalid reservation");
            }

            var statusChangeReservations = new StatusChangeInputModel()
            {
                ID = guidID,
                Status = Status.Cancelled
            };

            try
            {
                var reservation = this.reservationManager.ChangeStatus(statusChangeReservations);
                this.reservations.Update(reservation);
                this.reservations.SaveChanges();

                return View();
            }
            catch (ArgumentNullException)
            {
                return HttpNotFound("Reservation not found");
            }
        }

        [HttpGet]
        public JsonResult GetOverlapDates()
        {
            var overlappingDates = this.datesManager.GetOverlappedDates();
            var datesList = this.datesManager.AvailableDates(overlappingDates);
            var dates = new { Dates = datesList };

            return Json(dates, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPricing(DateTime from, DateTime to)
        {
            var pricing = this.reservationManager.GetPricing(from, to);
            return Json(pricing);
        }
    }
}