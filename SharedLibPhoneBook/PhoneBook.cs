﻿namespace SharedLibPhoneBook
{
    public class PhoneBook
    {
        public int Id { get; set; }
        public string FirsName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DetailBook DetailBook { get; set; }
        
    }
    public class DetailBook
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Adres { get; set; }
        public string Description { get; set; }
    }
}