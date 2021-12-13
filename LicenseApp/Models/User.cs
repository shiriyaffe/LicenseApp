using System;
using System.Collections.Generic;
using System.Text;

namespace LicenseApp.Models
{
    public class User
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string UserPswd { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
    }
}
