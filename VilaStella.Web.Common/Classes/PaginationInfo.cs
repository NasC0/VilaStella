using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VilaStella.Web.Common.Classes
{
    public struct PaginationInfo
    {
        public int Page { get; set; }

        public int SkipSize { get; set; }

        public bool IsRedirect { get; set; }
    }
}
