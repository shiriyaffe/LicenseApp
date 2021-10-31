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

    }
}
