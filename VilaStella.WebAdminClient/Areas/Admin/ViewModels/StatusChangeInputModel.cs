using System;
using VilaStella.Models;

namespace VilaStella.WebAdminClient.Areas.Admin.ViewModels
{
    public class StatusChangeInputModel
    {
        public Guid ID { get; set; }

        public Status Status { get; set; }
    }
}