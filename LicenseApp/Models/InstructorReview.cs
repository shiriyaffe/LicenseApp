using System;
using System.Collections.Generic;
using System.Text;

namespace LicenseApp.Models
{
    public class InstructorReview
    {
        public int ReviewId { get; set; }
        public int InstructorId { get; set; }

        public virtual Instructor Instructor { get; set; }
        public virtual Review Review { get; set; }
    }
}
