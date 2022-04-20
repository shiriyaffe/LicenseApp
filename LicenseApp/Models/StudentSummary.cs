using System;
using System.Collections.Generic;
using System.Text;

namespace LicenseApp.Models
{
    public class StudentSummary
    {
        public int ReviewId { get; set; }
        public int StudentId { get; set; }

        public virtual Review Review { get; set; }
        public virtual Student Student { get; set; }
    }
}
