using System;
using System.Linq;
using VilaStella.Models;
using VilaStella.Web.Common.Classes;
using VilaStella.Web.Common.Contracts;

namespace VilaStella.Web.Common.DataFilters
{
    public class FromFilter : BaseFilter, IFilterStrategy
    {
        public FromFilter(FilterOptions filterOptions)
            : base(filterOptions)
        {
        }

        public IQueryable<Reservation> Filter(IQueryable<Reservation> reservations)
        {
            DateTime fromDate = (DateTime)this.filterOptions.From;

            var filteredResults = reservations
                .Where(x => x.From == fromDate);

            return filteredResults;
        }
    }
}
