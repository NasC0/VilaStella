using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VilaStella.Models;

namespace VilaStella.WebAdminClient.Infrastructure.Contracts
{
    public interface IEmailManager
    {
        void SendConfirmationEmail(Reservation reservation);
    }
}