using System;
using System.Collections.Generic;
using System.Text;

namespace LicenseApp.Models
{
    public class Lesson
    {
        public int LessonId { get; set; }
        public TimeSpan Ltime { get; set; }
        public DateTime Ldate { get; set; }
        public string Lday { get; set; }
        public bool IsAvailable { get; set; }
        public int? StuudentId { get; set; }
        public bool IsPaid { get; set; }
        public bool HasDone { get; set; }
        public int InstructorId { get; set; }
        public int ReviewId { get; set; }

        public virtual Instructor Instructor { get; set; }
        public virtual Student Stuudent { get; set; }
        public virtual ICollection<EnrollmentRequest> EnrollmentRequests { get; set; }
    }
}
