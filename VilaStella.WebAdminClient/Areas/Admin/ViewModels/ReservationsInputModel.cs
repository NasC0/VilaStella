using System;
using System.ComponentModel.DataAnnotations;
using VilaStella.Models;
using VilaStella.Web.Common.Mappings;

namespace VilaStella.WebAdminClient.Areas.Admin.ViewModels
{
    public class ReservationsInputModel : IMapFrom<Reservation>
    {
        public Guid ID { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(15)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyy}", ApplyFormatInEditMode = true)]
        public DateTime From { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyy}", ApplyFormatInEditMode = true)]
        public DateTime To { get; set; }

        [Required]
        public int PartySize { get; set; }

        [Required]
        public Status Status { get; set; }
    }
}