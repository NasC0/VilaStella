using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VilaStella.Models;
using VilaStella.WebAdminClient.Areas.Admin.ViewModels;

namespace VilaStella.WebAdminClient.Infrastructure.Contracts
{
    public interface IReservationManager
    {
        bool ValidateReservation(ModelStateDictionary modelState, ReservationsInputModel inputReservation);

        Reservation CreateReservation(ReservationsInputModel inputReservation, bool isSeen);

        Pricing GetPricing(Reservation reservation);
    }
}