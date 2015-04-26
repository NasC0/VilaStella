using System.Linq;
using System.Data.Linq.SqlClient;
using VilaStella.Models;
using VilaStella.Web.Common.Classes;
using VilaStella.Web.Common.Contracts;

namespace VilaStella.Web.Common.DataFilters
{
    public class NameFilter : BaseFilter, IFilterStrategy
    {
        public NameFilter(FilterOptions filterOptions)
            : base(filterOptions)
        {
        }

        public IQueryable<Reservation> Filter(IQueryable<Reservation> reservations)
        {
            string nameObject = this.filterOptions.Name.ToLower();

            var filteredReservations = reservations
                .Where(x => x.FirstName.ToLower().Contains(nameObject) 
                    || x.LastName.ToLower().Contains(nameObject));

            return filteredReservations;
        }
    }
}
