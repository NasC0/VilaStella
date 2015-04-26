using System;
using System.Linq;
using VilaStella.Models;
using VilaStella.Web.Common.Classes;
using VilaStella.Web.Common.Contracts;

namespace VilaStella.Web.Common.DataFilters
{
    public class DateFilter : BaseFilter, IFilterStrategy
    {
        public DateFilter(FilterOptions filterOptions)
            : base(filterOptions)
        {
        }

        public IQueryable<Reservation> Filter(IQueryable<Reservation> reservations)
        {
            DateTime dateObject = (DateTime)this.filterOptions.Date;

            var filteredResults = reservations
                .Where(x => dateObject >= x.From && dateObject <= x.To);

            return filteredResults;
        }
    }
}
