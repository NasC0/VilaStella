using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayPal.Api;
using VilaStella.Models;

namespace VilaStella.WebAdminClient.Infrastructure.Contracts
{
    public interface IPayPalManager
    {
        Invoice MakeInvoice(Reservation reservation);

        void SendInvoice(Invoice invoiceToSend);
    }
}