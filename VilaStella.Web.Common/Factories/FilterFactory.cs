using System;
using VilaStella.Web.Common.Classes;
using VilaStella.Web.Common.Contracts;
using VilaStella.Web.Common.DataFilters;

namespace VilaStella.Web.Common.Factories
{
    public class FilterFactory : IFilterFactory
    {
        public IFilterStrategy GetFilter(FilterOptions options)
        {
            return this.DetermineFilter(options);
        }

        private IFilterStrategy DetermineFilter(FilterOptions options)
        {
            IFilterStrategy filterStrategy = null;

            if (options.Filter == FilterBy.Name && !string.IsNullOrEmpty(options.Name))
            {
                filterStrategy = new NameFilter(options);
            }
            else if (options.Filter == FilterBy.Email && !string.IsNullOrEmpty(options.Email))
            {
                filterStrategy = new EmailFilter(options);
            }
            else if (options.Filter == FilterBy.Phone && !string.IsNullOrEmpty(options.Phone))
            {
                filterStrategy = new PhoneFilter(options);
            }
            else if (options.Filter == FilterBy.From && options.From.HasValue)
            {
                filterStrategy = new FromFilter(options);
            }
            else if (options.Filter == FilterBy.To && options.To.HasValue)
            {
                filterStrategy = new ToFilter(options);
            }
            else if (options.Filter == FilterBy.Status)
            {
                filterStrategy = new StatusFIlter(options);
            }
            else if (options.Filter == FilterBy.Date && options.Date.HasValue)
            {
                filterStrategy = new DateFilter(options);
            }

            return filterStrategy;
        }
    }
}
