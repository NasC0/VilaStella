using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VilaStella.Web.Common.Classes
{
    public static class SetHelpers
    {
        public static List<DateTime> BuildDateSet(DateTime from, DateTime to)
        {
            var datesSet = new List<DateTime>();
            var fromClone = from;

            for (; fromClone <= to; fromClone = fromClone.AddDays(1))
            {
                datesSet.Add(fromClone);
            }

            return datesSet;
        }
    }
}
