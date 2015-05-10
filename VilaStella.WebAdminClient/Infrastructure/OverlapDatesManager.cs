using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VilaStella.Data.Common.Repositories;
using VilaStella.Models;
using VilaStella.WebAdminClient.Models;
using VilaStella.WebAdminClient.Infrastructure.Contracts;

namespace VilaStella.WebAdminClient.Infrastructure
{
    public class OverlapDatesManager : IOverlapDatesManager
    {
        private const string DATE_FORMAT = "dd.M.yyyy";

        private IDeletableRepository<Reservation> reservations;

        public OverlapDatesManager(IDeletableRepository<Reservation> reservations)
        {
            this.reservations = reservations;
        }

        public Dictionary<DateTime, int> GetOverlappedDates(ReservationsInputModel inputReservation = null)
        {
            var currentDate = DateTime.Now.Date;
            var overlappingDates = new Dictionary<DateTime, int>();

            var viableReservations = this.reservations.All()
                                            .Where(x => x.Status == Status.Approved && (x.From >= currentDate || x.To >= currentDate));

            if (inputReservation != null)
            {
                viableReservations = viableReservations.Where(x => x.ID != inputReservation.ID);
            }

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

            return overlappingDates;
        }

        public List<string> AvailableDates(Dictionary<DateTime, int> overlappingDates)
        {
            var datesList = new List<string>();

            foreach (var date in overlappingDates)
            {
                if (date.Value >= 2)
                {
                    string dateString = date.Key.ToString(DATE_FORMAT);
                    datesList.Add(dateString);
                    continue;
                }

                var previousDate = date.Key.AddDays(-1);
                var nextDate = date.Key.AddDays(1);
                bool previousDateExcluded = (overlappingDates.ContainsKey(previousDate) && overlappingDates[previousDate] >= 2);
                bool nextDateExcluded = (overlappingDates.ContainsKey(nextDate) && overlappingDates[nextDate] >= 2);

                if (previousDateExcluded && nextDateExcluded)
                {
                    datesList.Add(date.Key.ToString(DATE_FORMAT));
                }
            }

            datesList = datesList.OrderBy(x => DateTime.Parse(x))
                                 .ToList();

            return datesList;
        }
    }
}