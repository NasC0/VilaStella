using System;
using System.Linq;
using System.Configuration;
using System.Web.Mvc;
using VilaStella.Data;
using VilaStella.Data.Common.Repositories;
using VilaStella.Models;
using AutoMapper.QueryableExtensions;
using VilaStella.WebAdminClient.Areas.Admin.ViewModels;
using System.Collections.Generic;
using VilaStella.WebAdminClient.Infrastructure.Contracts;
using VilaStella.WebAdminClient.Infrastructure;

namespace VilaStella.WebAdminClient.Controllers
{
    public class HomeController : Controller
    {
        private IDeletableRepository<Image> images;
        private IGenericRepositoy<GeneralSettings> settings;
        private IDeletableRepository<Reservation> reservations;
        private IReservationManager reservationManager;
        private IOverlapDatesManager datesManager;

        public HomeController(IDeletableRepository<Image> images, IGenericRepositoy<GeneralSettings> settings, IDeletableRepository<Reservation> reservations, IReservationManager reservationManager, IOverlapDatesManager datesManager)
        {
            this.images = images;
            this.settings = settings;
            this.reservations = reservations;
            this.reservationManager = reservationManager;
            this.datesManager = datesManager;
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

        [HttpGet]
        public JsonResult GetOverlapDates()
        {
            var overlappingDates = this.datesManager.GetOverlappedDates();
            var datesList = this.datesManager.AvailableDates(overlappingDates);
            var dates = new { Dates = datesList };

            return Json(dates, JsonRequestBehavior.AllowGet);
        }
    }
}