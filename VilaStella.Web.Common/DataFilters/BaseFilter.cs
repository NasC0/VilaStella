using VilaStella.Web.Common.Classes;

namespace VilaStella.Web.Common.DataFilters
{
    public class BaseFilter
    {
        protected FilterOptions filterOptions;

        public BaseFilter(FilterOptions filterOptions)
        {
            this.filterOptions = filterOptions;
        }
    }
}
