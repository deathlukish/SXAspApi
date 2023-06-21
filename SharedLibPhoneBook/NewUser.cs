using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SharedLibPhoneBook
{
    public class NewUser : UserApi
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Введите пароль")]
        [Display(Name = "Повторите : ")]
        public string? RepPaswword { get; set; }
    }
}
