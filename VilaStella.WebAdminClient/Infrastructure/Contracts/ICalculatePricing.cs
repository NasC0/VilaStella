using System;
using VilaStella.Models;
using VilaStella.WebAdminClient.Areas.Admin.ViewModels;

namespace VilaStella.WebAdminClient.Infrastructure.Contracts
{
    public interface ICalculatePricing
    {
        Pricing GetPricing(DateTime from, DateTime to);
    }
}
