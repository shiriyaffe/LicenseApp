using System;
using System.Collections.Generic;
using System.Text;

namespace LicenseApp.Models
{
    public class SchoolManager
    {
        public int SmanagerId { get; set; }
        public string Smname { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public string PhoneNumber { get; set; }
        public int GenderId { get; set; }
        public DateTime Birthday { get; set; }
        public string DrivingSchool { get; set; }
        public int AreaId { get; set; }
        public int EstablishmentYear { get; set; }
        public int NumOfTeachers { get; set; }
        public DateTime RegistrationDate { get; set; }

        public virtual Area Area { get; set; }
        public virtual Gender Gender { get; set; }
    }
}
