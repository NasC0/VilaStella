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

            if (options.Filter == FilterBy.Name)
            {
                filterStrategy = new NameFilter(options);
            }
            else if (options.Filter == FilterBy.Email)
            {
                filterStrategy = new EmailFilter(options);
            }
            else if (options.Filter == FilterBy.Phone)
            {
                filterStrategy = new PhoneFilter(options);
            }
            else if (options.Filter == FilterBy.From)
            {
                filterStrategy = new FromFilter(options);
            }
            else if (options.Filter == FilterBy.To)
            {
                filterStrategy = new ToFilter(options);
            }
            else if (options.Filter == FilterBy.Status)
            {
                filterStrategy = new StatusFIlter(options);
            }
            else if (options.Filter == FilterBy.Date)
            {
                filterStrategy = new DateFilter(options);
            }

            return filterStrategy;
        }
    }
}
