using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VilaStella.Data.Common.Models;

namespace VilaStella.Models
{
    public class Reservation : AuditInfo, IDeletableEntity
    {
        public Reservation()
        {
            this.ID = Guid.NewGuid();
        }

        public Guid ID { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(15)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyy}")]
        public DateTime From { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyy}")]
        public DateTime To { get; set; }

        [NotMapped]
        public List<DateTime> Dates
        {
            get
            {
                var datesList = new List<DateTime>();

                var fromClone = this.From;
                for (; fromClone < this.To; fromClone = fromClone.AddDays(1))
                {
                    datesList.Add(fromClone);
                }

                return datesList;
            }
        }

        [Required]
        public decimal Capparo { get; set; }

        [Required]
        public decimal FullPrice { get; set; }

        [Required]
        public int PartySize { get; set; }

        public Status Status { get; set; }

        public bool IsSeen { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
