namespace SharedLibPhoneBook
{
    public class PhoneBookDetail : PhoneBook
    {
        public string? Phone { get; set; }
        public string? Adres { get; set; }
        public string? Description { get; set; }
    }
    public class PhoneBook
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
    }
}