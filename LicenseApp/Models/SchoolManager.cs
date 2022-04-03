using System;
using System.Collections.Generic;
using System.Text;
using LicenseApp.Services;

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
        public DateTime RegistrationDate { get; set; }
        public int? SchoolId { get; set; }
        public DateTime Birthday { get; set; }
        public int EStatusId { get; set; }

        public virtual Estatus EStatus { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual DrivingSchool School { get; set; }

        public string PhotoURI
        {
            get
            {
                LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                //Create a source with cache busting!
                Random r = new Random();
                return $"{proxy.GetBasePhotoUri()}SchoolManagers/{this.SmanagerId}.jpg?{r.Next()}";
            }
        }
    }
}
