using System.ComponentModel.DataAnnotations;

namespace VilaStella.WebAdminClient.Areas.Admin.ViewModels
{
    public class SettingsOutputModel
    {
        [Required]
        public decimal PricePerNight { get; set; }

        [Required]
        public bool AreReservationsOpen { get; set; }
    }
}