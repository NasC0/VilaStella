using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VilaStella.Models;

namespace VilaStella.Web.Common.Helpers
{
    public interface IRandomReservationGenerator
    {
        IEnumerable<Reservation> Generate(int count);
    }
}
