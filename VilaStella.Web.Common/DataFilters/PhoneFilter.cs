using System.Linq;
using VilaStella.Models;
using VilaStella.Web.Common.Classes;
using VilaStella.Web.Common.Contracts;

namespace VilaStella.Web.Common.DataFilters
{
    public class PhoneFilter : BaseFilter, IFilterStrategy
    {
        public PhoneFilter(FilterOptions filterOptions)
            : base(filterOptions)
        {
        }

        public IQueryable<Reservation> Filter(IQueryable<Reservation> reservations)
        {
            string phoneObject = this.filterOptions.Phone.ToLower();

            var filteredResults = reservations
                .Where(x => x.Phone.ToLower().Contains(phoneObject));

            return filteredResults;
        }
    }
}
