using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VilaStella.Web.Common.Contracts;

namespace VilaStella.Web.Common.CapparoCalculator
{
    public class DiscountCapparoCalculator : ICapparoCalculator
    {
        private const int CAPPARO_PERCENTAGE = 20;

        public decimal CalculateCapparo(decimal basePrice, int nightsSpent)
        {
            decimal fullPrice = basePrice * nightsSpent;
            decimal percentage = (decimal)CAPPARO_PERCENTAGE / 100;
            decimal discountCapparo = fullPrice * percentage;

            return discountCapparo;
        }
    }
}
