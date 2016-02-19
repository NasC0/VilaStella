using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VilaStella.Data.Common.Models;

namespace VilaStella.Models
{
    public class ForgottenPassword : AuditInfo, IDeletableEntity
    {
        public int ID { get; set; }

        public virtual Guid? PasswordResetKey { get; set; }

        public virtual bool IsDeleted { get; set; }

        public virtual DateTime? DeletedOn { get; set; }

        public virtual string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
