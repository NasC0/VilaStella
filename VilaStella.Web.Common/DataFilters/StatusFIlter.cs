using System.Linq;
using VilaStella.Models;
using VilaStella.Web.Common.Classes;
using VilaStella.Web.Common.Contracts;

namespace VilaStella.Web.Common.DataFilters
{
    public class StatusFIlter : BaseFilter, IFilterStrategy
    {
        public StatusFIlter(FilterOptions filterOptions)
            : base(filterOptions)
        {
        }

        public IQueryable<Reservation> Filter(IQueryable<Reservation> reservations)
        {
            Status statusObject = this.filterOptions.StatusFilter;

            var filteredResults = reservations
                .Where(x => x.Status == statusObject);

            return filteredResults;
        }
    }
}
