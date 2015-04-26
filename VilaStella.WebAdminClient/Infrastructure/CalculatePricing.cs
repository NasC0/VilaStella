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
        private ICapparoFactory capparoFactory;
        private IGenericRepositoy<GeneralSettings> generalSettings;

        public CalculatePricing(ICapparoFactory factory, IGenericRepositoy<GeneralSettings> settings)
        {
            this.capparoFactory = factory;
            this.generalSettings = settings;
        }

        public Pricing GetPricing(Reservation reservation)
        {
            var currentSettings = this.generalSettings.All()
                .OrderByDescending(x => x.ID)
                .FirstOrDefault();

            decimal pricePerNight = currentSettings.Pricing;
            int daysSpent = (reservation.To - reservation.From).Days;
            int nightsSpent = daysSpent;

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