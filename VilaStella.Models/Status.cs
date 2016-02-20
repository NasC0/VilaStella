using System.ComponentModel.DataAnnotations;

namespace VilaStella.Models
{
    public enum Status
    {
        [Display(Name = "Изчакване")]
        Pending,
        [Display(Name = "Отменена")]
        Cancelled,
        [Display(Name = "Одобрена")]
        Approved
    }
}
