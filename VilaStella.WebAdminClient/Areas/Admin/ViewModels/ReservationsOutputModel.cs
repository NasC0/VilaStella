using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using VilaStella.Models;
using VilaStella.Web.Common.Mappings;

namespace VilaStella.WebAdminClient.Areas.Admin.ViewModels
{
    public class ReservationsOutputModel : IMapFrom<Reservation>, IHaveCustomMappings
    {
        public Guid ID { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Phone { get; set; }

        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }

        [Required]
        public decimal Capparo { get; set; }

        [Required]
        public decimal FullPrice { get; set; }

        [Required]
        public int PartySize { get; set; }

        public Status Status { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Reservation, ReservationsOutputModel>()
                         .ForMember(r => r.FullName, opts => opts.MapFrom(x => x.FirstName + " " + x.LastName));
        }
    }
}