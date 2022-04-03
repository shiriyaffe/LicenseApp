using System;
using System.Collections.Generic;
using System.Text;

namespace LicenseApp.Models
{
    public class DrivingSchool
    {
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public int AreaId { get; set; }
        public int EstablishmentYear { get; set; }
        public int NumOfTeachers { get; set; }

        public virtual Area Area { get; set; }
        public virtual ICollection<EnrollmentRequest> EnrollmentRequests { get; set; }
        public virtual ICollection<Instructor> Instructors { get; set; }
        public virtual ICollection<SchoolManager> SchoolManagers { get; set; }
    }
}
