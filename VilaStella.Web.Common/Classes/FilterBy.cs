using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VilaStella.Web.Common.Classes
{
    public enum FilterBy
    {
        [Display(Name = "Име")]
        Name,
        Email,
        [Display(Name = "Телефон")]
        Phone,
        [Display(Name = "От")]
        From,
        [Display(Name = "До")]
        To,
        [Display(Name = "Статус")]
        Status,
        [Display(Name = "Дата")]
        Date
    }
}
