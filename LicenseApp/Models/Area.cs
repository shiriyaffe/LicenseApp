using System;
using System.Collections.Generic;
using System.Text;

namespace LicenseApp.Models
{
    public class Area
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public virtual ICollection<City> Cities { get; set; }
        public virtual ICollection<DrivingSchool> DrivingSchools { get; set; }
    }
}
