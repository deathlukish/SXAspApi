
namespace SharedLibPhoneBook
{
    public class UserApi
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public RoleApi Role { get; set; }
    }
    public class RoleApi
    { 
        public string RoleName { get; set; }
        public string RoleDesc { get; set; }
    }
}
