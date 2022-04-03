using System;
using System.Collections.Generic;
using System.Text;


namespace LicenseApp.Models
{
    public class LookupTables
    {
        public List<City> Cities { get; set; }
        public List<Area> Areas { get; set; }
        public List<Gearbox> GearBoxes { get; set; }
        public List<Gender> Genders { get; set; }
        public List<LicenseType> LicenseTypes { get; set; }
        public List<LessonLength> LessonLengths { get; set; }
        public List<DrivingSchool> DrivingSchools { get; set; }
        public List<WorkingHour> WorkingHours { get; set; }
        public List<Estatus> Status { get; set; }
        public LookupTables() { }
    }
}
