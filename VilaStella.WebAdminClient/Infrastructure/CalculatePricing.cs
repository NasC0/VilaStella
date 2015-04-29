using System.Linq;
using VilaStella.Data.Common.Repositories;
using VilaStella.Models;
using VilaStella.Web.Common.Contracts;
using VilaStella.WebAdminClient.Areas.Admin.ViewModels;
using VilaStella.WebAdminClient.Infrastructure.Contracts;

namespace VilaStella.WebAdminClient.Infrastructure
{
    public class CalculatePricing : ICalculatePricing
    {
        private const int FULL_PRICE_AT_NIGHTS = 5;

        private ICapparoFactory capparoFactory;
        private IGenericRepositoy<GeneralSettings> generalSettings;

        public CalculatePricing(ICapparoFactory factory, IGenericRepositoy<GeneralSettings> settings)
        {
            this.capparoFactory = factory;
            this.generalSettings = settings;
        }

        /// <summary>
        /// Calculates the full stay and capparo prices.
        /// If the stay is longer than the specified full price per nights stay,
        /// the clients get a night for free.
        /// </summary>
        /// <param name="reservation">The DB reservation object</param>
        /// <returns></returns>
        public Pricing GetPricing(Reservation reservation)
        {
            var currentSettings = this.generalSettings.All()
                .OrderByDescending(x => x.ID)
                .FirstOrDefault();

            decimal pricePerNight = currentSettings.Pricing;
            int nightsSpent = (reservation.To - reservation.From).Days;

            if (nightsSpent > FULL_PRICE_AT_NIGHTS)
            {
                nightsSpent--;
            }

            decimal fullPrice = pricePerNight * nightsSpent;
            decimal capparo = this.capparoFactory.GetCalculator(nightsSpent).CalculateCapparo(pricePerNight, nightsSpent);

            return new Pricing
            {
                Capparo = capparo,
                FullPrice = fullPrice
            };
        }
    }
}