using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VilaStella.Models;

namespace VilaStella.Web.Common.Classes
{
    public class FilterOptions
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyy}")]
        public DateTime? From { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyy}")]
        public DateTime? To { get; set; }

        public Status Status { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyy}")]
        public DateTime? Date { get; set; }

        [Required]
        public FilterBy Filter { get; set; }
    }
}
