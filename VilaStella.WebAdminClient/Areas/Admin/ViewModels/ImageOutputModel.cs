using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VilaStella.Models;
using VilaStella.Web.Common.Mappings;

namespace VilaStella.WebAdminClient.Areas.Admin.ViewModels
{
    public class ImageOutputModel : IMapFrom<Image>
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public byte[] Picture { get; set; }

        public string Caption { get; set; }

        public bool IsShown { get; set; }
    }
}