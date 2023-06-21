
using System.ComponentModel.DataAnnotations;

namespace SharedLibPhoneBook
{
    public class UserApi : User
    {
        [Required(ErrorMessage = "Введите почту")]
        public string? Email { get; set; }
        public RoleApi? Role { get; set; } = new RoleApi() { RoleName = "User" };
    }
    public class RoleApi
    { 
        public string? RoleName { get; set; }
        public string? RoleDesc { get; set; }
    }
}
