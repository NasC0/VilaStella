using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VilaStella.Data.Common.Repositories;
using VilaStella.Models;
using VilaStella.Web.Common.Helpers;
using VilaStella.WebAdminClient.Infrastructure.Contracts;

namespace VilaStella.WebAdminClient.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class GenerationController : Controller
    {
        private IDeletableRepository<Reservation> reservations;
        private IRandomReservationGenerator reservationGenerator;
        private ICalculatePricing pricingCalculator;

        public GenerationController(IDeletableRepository<Reservation> reservations, IRandomReservationGenerator reservationGenerator, ICalculatePricing calculator)
        {
            this.reservations = reservations;
            this.reservationGenerator = reservationGenerator;
            this.pricingCalculator = calculator;
        }

        // GET: Admin/Generation
        public ActionResult Index()
        {
            var randomReservations = this.reservationGenerator.Generate(20);

            foreach (var reservation in randomReservations)
            {
                var pricing = this.pricingCalculator.GetPricing(reservation.From, reservation.To);

                reservation.Capparo = pricing.Capparo;
                reservation.FullPrice = pricing.FullPrice;

                this.reservations.Add(reservation);
            }

            this.reservations.SaveChanges();

            return View();
        }
    }
}