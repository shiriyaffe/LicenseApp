using System;
using System.Collections.Generic;
using System.Text;

namespace LicenseApp.Models
{
    public class Estatus
    {
        public int StatusId { get; set; }
        public string StatusMeaning { get; set; }

        public virtual ICollection<EnrollmentRequest> EnrollmentRequests { get; set; }
        public virtual ICollection<Instructor> Instructors { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
