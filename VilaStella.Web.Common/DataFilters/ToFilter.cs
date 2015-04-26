using System;
using System.Linq;
using VilaStella.Models;
using VilaStella.Web.Common.Classes;
using VilaStella.Web.Common.Contracts;

namespace VilaStella.Web.Common.DataFilters
{
    public class ToFilter : BaseFilter, IFilterStrategy
    {
        public ToFilter(FilterOptions filterOptions)
            : base(filterOptions)
        {
        }

        public IQueryable<Reservation> Filter(IQueryable<Reservation> reservations)
        {
            DateTime toDate = (DateTime)this.filterOptions.To;

            var filteredResults = reservations
                .Where(x => x.To == toDate);

            return filteredResults;
        }
    }
}
