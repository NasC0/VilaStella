using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VilaStella.Web.Common.CapparoCalculator;
using VilaStella.Web.Common.Contracts;

namespace VilaStella.Web.Common.Factories
{
    public class CapparoFactory : ICapparoFactory
    {
        public ICapparoCalculator GetCalculator(int nightsSpent)
        {
            return new FullCapparoCalculator();
        }
    }
}
