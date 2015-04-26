using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VilaStella.Data.Common.Models;

namespace VilaStella.Models
{
    public class GeneralSettings : AuditInfo, IAuditInfo
    {
        public int ID { get; set; }

        [Required]
        public decimal Pricing { get; set; }

        [Required]
        public bool AreReservationsOpen { get; set; }
    }
}
