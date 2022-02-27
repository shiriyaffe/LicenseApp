﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LicenseApp.Models
{
    public class EnrollmentRequest
    {
        public int EnrollmentId { get; set; }
        public int LessonId { get; set; }
        public int StatusId { get; set; }
        public int StudentId { get; set; }
        public int InstructorId { get; set; }

        public virtual Instructor Instructor { get; set; }
        public virtual Student Student { get; set; }
    }
}
