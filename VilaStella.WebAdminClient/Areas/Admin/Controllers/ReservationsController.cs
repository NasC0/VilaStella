using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using VilaStella.Data.Common.Repositories;
using VilaStella.Models;
using VilaStella.Web.Common.Classes;
using VilaStella.Web.Common.Contracts;
using VilaStella.WebAdminClient.Areas.Admin.ViewModels;
using VilaStella.WebAdminClient.Models;
using VilaStella.WebAdminClient.Infrastructure.Contracts;

namespace VilaStella.WebAdminClient.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReservationsController : Controller
    {
        private const int PAGE_SIZE = 10;
        private IDeletableRepository<Reservation> reservations;
        private IGenericRepositoy<GeneralSettings> setings;
        private IFilterFactory filterFactory;
        private ICalculatePricing pricingCalculator;
        private IReservationManager reservationManager;

        public ReservationsController(IDeletableRepository<Reservation> reservations, IGenericRepositoy<GeneralSettings> settings, IFilterFactory filterFactory, ICalculatePricing pricingCalculator, IReservationManager reservationManager)
        {
            this.reservations = reservations;
            this.setings = settings;
            this.filterFactory = filterFactory;
            this.pricingCalculator = pricingCalculator;
            this.reservationManager = reservationManager;
        }

        // GET: Admin/Basic
        public ActionResult Index(int? page)
        {
            var allReservations = this.reservations.All();
            var pagination = this.Paginate(page, allReservations.Count());

            if (pagination.IsRedirect)
            {
                return RedirectToAction("Index", new { page = pagination.Page });
            }

            ViewBag.CurrentPage = pagination.Page;

            // Holds the view numbering of the reservations
            ViewBag.CountStart = pagination.SkipSize;

            var viewReservations = allReservations
                .OrderByDescending(x => x.CreatedOn)
                .Skip(pagination.SkipSize)
                .Take(PAGE_SIZE)
                .Project()
                .To<ReservationsOutputModel>()
                .ToList();

            return View(viewReservations);
        }

        public ActionResult New()
        {
            var newReservations = this.reservations
                .All()
                .Where(x => x.IsSeen == true)
                .OrderByDescending(x => x.CreatedOn)
                .ToList();

            foreach (var reservation in newReservations)
            {
                reservation.IsSeen = false;
            }

            this.reservations.SaveChanges();
            var viewReservations = newReservations
                .AsQueryable()
                .Project()
                .To<ReservationsOutputModel>()
                .ToList();

            ViewBag.CountStart = 0;
            return View(viewReservations);
        }

        [HttpGet]
        public ActionResult GeneralSettings()
        {
            var dbSettings = this.setings.All()
                .OrderByDescending(x => x.ID)
                .FirstOrDefault();

            var settings = new SettingsOutputModel
            {
                PricePerNight = dbSettings.Pricing,
                AreReservationsOpen = dbSettings.AreReservationsOpen
            };

            return View(settings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GeneralSettings(SettingsOutputModel settings)
        {
            if (ModelState.IsValid)
            {
                var dbSettings = new GeneralSettings()
                {
                    Pricing = settings.PricePerNight,
                    AreReservationsOpen = settings.AreReservationsOpen
                };

                this.setings.Add(dbSettings);
                this.setings.SaveChanges();

                return RedirectToAction("Index", "Reservations");
            }

            return View(settings);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ReservationsInputModel input)
        {
            var isValidReservation = this.reservationManager.ValidateReservation(ModelState, input);

            if (ModelState.IsValid && isValidReservation)
            {
                bool isSeenInNewTab = true;
                var reservation = this.reservationManager.CreateReservation(input, isSeenInNewTab);

                this.reservations.Add(reservation);
                this.reservations.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(input);
        }

        public ActionResult Edit(Guid id)
        {
            if (id != null)
            {
                var reservation = this.reservations.AllWithDeleted()
                                      .Where(x => x.ID == id)
                                      .Project().To<ReservationsInputModel>()
                                      .FirstOrDefault();

                return View(reservation);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReservationsInputModel reservation)
        {
            bool isValidReservation = this.reservationManager.ValidateReservation(ModelState, reservation);

            if (ModelState.IsValid && isValidReservation)
            {
                var dbReservation = this.reservations.Find(reservation.ID);
                if (dbReservation != null)
                {
                    dbReservation.FirstName = reservation.FirstName;
                    dbReservation.LastName = reservation.LastName;
                    dbReservation.Email = reservation.Email;
                    dbReservation.Phone = reservation.Phone;
                    dbReservation.From = reservation.From;
                    dbReservation.To = reservation.To;
                    dbReservation.PaymentMethod = reservation.PaymentMethod;
                    dbReservation.Status = reservation.Status;

                    var updatedPricing = this.reservationManager.GetPricing(dbReservation.From, dbReservation.To);
                    dbReservation.Capparo = updatedPricing.Capparo;
                    dbReservation.FullPrice = dbReservation.FullPrice;

                    this.reservations.Update(dbReservation);
                    this.reservations.SaveChanges();

                    return RedirectToAction("Index", "Reservations");
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }

            return View(reservation);
        }

        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            if (id != null)
            {
                var entry = this.reservations.Find(id);
                this.reservations.Delete(entry);
                this.reservations.SaveChanges();

                var sucessResult = new {
                    Result = true,
                    ID = id
                };

                return Json(sucessResult);
            }

            var failedResult = new {
                Result = false
            };

            return Json(failedResult);
        }

        public ActionResult FilterForm()
        {
            return PartialView("_FilterForm");
        }

        [HttpPost]
        public ActionResult Filter(FilterOptions options)
        {
            var filter = this.filterFactory.GetFilter(options);
            var allReservations = this.reservations.All();
            var filteredReservations = filter.Filter(allReservations)
                .OrderByDescending(x => x.CreatedOn)
                .Project()
                .To<ReservationsOutputModel>()
                .ToList();

            ViewBag.CountStart = 0;
            return View(filteredReservations);
        }

        public ActionResult ChangeStatus(StatusChangeInputModel reservation)
        {
            var dbReservation = this.reservations.Find(reservation.ID);
            if (dbReservation != null)
            {
                dbReservation.Status = reservation.Status;
                this.reservations.SaveChanges();
                return PartialView("_ChangeStatus", reservation);
            }

            return RedirectToAction("Index", "Reservations");
        }

        private PaginationInfo Paginate(int? page, int entriesCount)
        {
            PaginationInfo paginate = new PaginationInfo();
            if (page == null || entriesCount <= PAGE_SIZE)
            {
                paginate.Page = 1;
                return paginate;
            }

            int pageInt = (int)page;
            int maxPages = (entriesCount / PAGE_SIZE);

            if (entriesCount % 10 != 0)
            {
                maxPages += 1;
            }

            if (pageInt <= 0 || pageInt > maxPages)
            {
                if (pageInt <= 0)
                {
                    pageInt = 1;
                }
                else if (pageInt > maxPages)
                {
                    pageInt = maxPages;
                }

                paginate.IsRedirect = true;
            }

            int skipSize = (pageInt - 1) * PAGE_SIZE;

            paginate.Page = pageInt;
            paginate.SkipSize = skipSize;

            return paginate;
        }
    }
}