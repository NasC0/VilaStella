using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VilaStella.Models;
using VilaStella.WebAdminClient.Models;

namespace VilaStella.WebAdminClient.Infrastructure.Contracts
{
    public interface IReservationManager
    {
        bool ValidateReservation(ModelStateDictionary modelState, ReservationsInputModel inputReservation);

        Reservation CreateReservation(ReservationsInputModel inputReservation, bool isSeen);

        Pricing GetPricing(DateTime from, DateTime to);
    }
}