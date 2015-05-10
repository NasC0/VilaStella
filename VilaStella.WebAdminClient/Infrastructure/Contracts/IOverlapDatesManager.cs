using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VilaStella.WebAdminClient.Models;

namespace VilaStella.WebAdminClient.Infrastructure.Contracts
{
    public interface IOverlapDatesManager
    {
        Dictionary<DateTime, int> GetOverlappedDates(ReservationsInputModel inputReservation = null);

        List<string> AvailableDates(Dictionary<DateTime, int> overlappingDates);
    }
}