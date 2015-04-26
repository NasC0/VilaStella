using System.Linq;
using VilaStella.Models;

namespace VilaStella.Web.Common.Contracts
{
    public interface IFilterStrategy
    {
        IQueryable<Reservation> Filter(IQueryable<Reservation> reservations);
    }
}
