using VilaStella.Web.Common.Classes;

namespace VilaStella.Web.Common.Contracts
{
    public interface IFilterFactory
    {
        IFilterStrategy GetFilter(FilterOptions options);
    }
}
