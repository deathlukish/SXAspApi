using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SharedLibPhoneBook
{
    public class User
    {

        [Required(ErrorMessage = "Введите логин")]
        [Display(Name = "Логин : ")]
        public string? UserId { get; set; } = null;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Введите пароль")]
        [Display(Name = "Пароль : ")]
        public string? Password { get; set; } = null;

        public string ReturnUrl { get; set; } = "/";

    }
}

