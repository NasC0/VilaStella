using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VilaStella.Data.Common.Repositories;
using VilaStella.Models;
using VilaStella.Web.Common.Classes;
using VilaStella.WebAdminClient.Models;
using VilaStella.WebAdminClient.Infrastructure.Contracts;

namespace VilaStella.WebAdminClient.Infrastructure
{
    public class ReservationManager : IReservationManager
    {
        private const string FROM_DATE_BEFORE_TO_DATE_ERROR = "Датата \"До\" трябва да е след датата \"От\"";
        private const string OVERLAPPING_RESERVATION_DATES = "Припокриване на дати за резервация";
        private const int ROOMS_COUNT = 2;

        private IDeletableRepository<Reservation> reservations;
        private ICalculatePricing pricingCalculator;
        private IOverlapDatesManager datesManager;

        public ReservationManager(IDeletableRepository<Reservation> reservations, ICalculatePricing pricingCalculator, IOverlapDatesManager datesManager)
        {
            this.reservations = reservations;
            this.pricingCalculator = pricingCalculator;
            this.datesManager = datesManager;
        }

        public bool ValidateReservation(ModelStateDictionary modelState, ReservationsInputModel inputReservation)
        {
            bool isValid = true;

            // Check whether the To date is before the From date
            if (inputReservation.To <= inputReservation.From)
            {
                modelState.AddModelError("Date", FROM_DATE_BEFORE_TO_DATE_ERROR);
                isValid = false;
            }

            // Check for overlapping reservation days for all available rooms
            // Offset the start date so we can account for checkouts on the same From date
            // var offsetStartDate = inputReservation.From.AddDays(1);
            var dates = SetHelpers.BuildDateSet(inputReservation.From, inputReservation.To);

            var allDates = this.datesManager.GetOverlappedDates(inputReservation);

            foreach (var date in dates)
            {
                if (allDates.ContainsKey(date) && allDates[date] >= ROOMS_COUNT)
                {
                    modelState.AddModelError("Date", OVERLAPPING_RESERVATION_DATES);
                    isValid = false;
                    break;
                }
            }

            return isValid;
        }

        /// <summary>
        /// Creates the database reservation model from the reservation input model.
        /// </summary>
        /// <param name="inputReservation">The reservation input model from the website.</param>
        /// <param name="isSeen">Determines whether the reservation should be visible in the New tab of the Admin panel. By default it is set to "false" for reservations added through the Admin tool and "true" for user reservations.</param>
        /// <returns>The derived database reservation model.</returns>
        public Reservation CreateReservation(ReservationsInputModel inputReservation, bool isSeen)
        {
            var reservation = new Reservation()
            {
                FirstName = inputReservation.FirstName,
                LastName = inputReservation.LastName,
                Email = inputReservation.Email,
                Phone = inputReservation.Phone,
                From = inputReservation.From,
                To = inputReservation.To,
                PartySize = inputReservation.PartySize,
                Status = inputReservation.Status,
                PaymentMethod = inputReservation.PaymentMethod,
                IsSeen = isSeen
            };

            var pricing = this.GetPricing(reservation.From, reservation.To);
            reservation.Capparo = pricing.Capparo;
            reservation.FullPrice = pricing.FullPrice;

            return reservation;
        }

        public Pricing GetPricing(DateTime from, DateTime to)
        {
            var pricing = this.pricingCalculator.GetPricing(from, to);
            return pricing;
        }
    }
}