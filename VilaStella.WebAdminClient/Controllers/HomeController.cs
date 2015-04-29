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

namespace VilaStella.WebAdminClient.Controllers
{
    public class HomeController : Controller
    {
        private IDeletableRepository<Image> images;
        private IGenericRepositoy<GeneralSettings> settings;
        private IDeletableRepository<Reservation> reservations;

        public HomeController(IDeletableRepository<Image> images, IGenericRepositoy<GeneralSettings> settings, IDeletableRepository<Reservation> reservations)
        {
            this.images = images;
            this.settings = settings;
            this.reservations = reservations;
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
        public ActionResult Add(ReservationsInputModel reservation)
        {
            if (ModelState.IsValid)
            {
                var bla = reservation;
            }

            return null;
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
            var currentDate = DateTime.Now.Date;
            var overlappingDates = new Dictionary<DateTime, int>();

            var viableReservations = this.reservations.All()
                                         .Where(x => x.Status == Status.Approved && (x.From >= currentDate || x.To >= currentDate));

            foreach (var reservation in viableReservations)
            {
                foreach (var date in reservation.Dates)
                {
                    if (date != reservation.To)
                    {
                        if (overlappingDates.ContainsKey(date))
                        {
                            overlappingDates[date]++;
                        }
                        else
                        {
                            overlappingDates.Add(date, 1);
                        }
                    }
                }
            }

            var datesList = new List<string>();

            foreach (var date in overlappingDates)
            {
                if (date.Value >= 2)
                {
                    string dateString = date.Key.ToString("dd.M.yyyy");
                    datesList.Add(dateString);
                    continue;
                }

                var previousDate = date.Key.AddDays(-1);
                var nextDate = date.Key.AddDays(1);
                bool previousDateExcluded = (overlappingDates.ContainsKey(previousDate) && overlappingDates[previousDate] >= 2);
                bool nextDateExcluded = (overlappingDates.ContainsKey(nextDate) && overlappingDates[nextDate] >= 2);

                if (previousDateExcluded && nextDateExcluded)
                {
                    datesList.Add(date.Key.ToString("dd.M.yyyy"));
                }
            }

            datesList = datesList.OrderBy(x => DateTime.Parse(x))
                                 .ToList();

            var dates = new { Dates = datesList };

            return Json(dates, JsonRequestBehavior.AllowGet);
        }
    }
}