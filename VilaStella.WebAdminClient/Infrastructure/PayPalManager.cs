using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PayPal.Api;
using VilaStella.Models;
using VilaStella.WebAdminClient.Infrastructure.Contracts;

namespace VilaStella.WebAdminClient.Infrastructure
{
    // TODO: Implement auth token expiry check and renewal;
    public class PayPalManager : IPayPalManager
    {
        private Dictionary<string, string> configuration;
        private string authToken;
        private APIContext payPalContext;

        public PayPalManager()
        {
            this.configuration = ConfigManager.Instance.GetProperties();
            this.authToken = new OAuthTokenCredential(this.configuration).GetAccessToken();
            this.payPalContext = new APIContext(this.authToken);
        }

        public Invoice MakeInvoice(Reservation reservation)
        {
            var merchangInfo = new MerchantInfo()
            {
                email = "stelchityyy@abv.bg",
                first_name = "Stela",
                last_name = "Siveva",
                business_name = "Vila Stella"
            };

            var billingInfo = new List<BillingInfo>()
            {
                new BillingInfo()
                {
                    email = reservation.Email,
                    first_name = reservation.FirstName,
                    last_name = reservation.LastName
                }
            };

            var items = new List<InvoiceItem>()
            {
                new InvoiceItem()
                {
                    name = "Reservation payment",
                    unit_price = new Currency()
                    {
                        currency = "EUR",
                        value = reservation.Capparo.ToString("0.##")
                    },
                    quantity = 1
                }
            };

            var invoiceToSend = new Invoice
            {
                merchant_info = merchangInfo,
                billing_info = billingInfo,
                items = items
            };


            var invoice = Invoice.Create(this.payPalContext, invoiceToSend);

            return invoice;
        }

        public void SendInvoice(Invoice invoiceToSend)
        {
            Invoice.Send(this.payPalContext, invoiceToSend.id);
        }
    }
}