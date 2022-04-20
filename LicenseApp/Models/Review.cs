using System;
using System.Collections.Generic;
using System.Text;

namespace LicenseApp.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string Content { get; set; }
        public DateTime WrittenOn { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
