using System.Linq;
using VilaStella.Models;
using VilaStella.Web.Common.Classes;
using VilaStella.Web.Common.Contracts;

namespace VilaStella.Web.Common.DataFilters
{
    public class EmailFilter : BaseFilter, IFilterStrategy
    {
        public EmailFilter(FilterOptions filterOptions)
            : base(filterOptions)
        {
        }

        public IQueryable<Reservation> Filter(IQueryable<Reservation> reservations)
        {
            string emailObject = this.filterOptions.Email.ToLower();

            var filteredResults = reservations
                .Where(x => x.Email.ToLower().Contains(emailObject));

            return filteredResults;
        }
    }
}
