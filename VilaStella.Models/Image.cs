using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VilaStella.Data.Common.Models;

namespace VilaStella.Models
{
    public class Image : AuditInfo, IDeletableEntity
    {
        public int ID { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public byte[] Picture { get; set; }

        public string Caption { get; set; }

        [Required]
        public string ImageType { get; set; }

        public bool IsShown { get; set; }

        [Index]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
