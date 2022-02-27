﻿using System;
using System.Collections.Generic;
using System.Text;
using LicenseApp.Services;

namespace LicenseApp.Models
{
    public partial class Instructor
    {
        public int InstructorId { get; set; }
        public string Iname { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public string PhoneNumber { get; set; }
        public int GenderId { get; set; }
        public DateTime Birthday { get; set; }
        public int AreaId { get; set; }
        public int GearboxId { get; set; }
        public int LicenseTypeId { get; set; }
        public int LessonLengthId { get; set; }
        public int Price { get; set; }
        public string Details { get; set; }
        public int? ReviewId { get; set; }
        public int DrivingSchoolId { get; set; }
        public int? SchoolManagerId { get; set; }
        public int RateId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public virtual Area Area { get; set; }
        public virtual DrivingSchool DrivingSchool { get; set; }
        public virtual Gearbox Gearbox { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual LessonLength LessonLength { get; set; }
        public virtual LicenseType LicenseType { get; set; }
        public virtual Rate Rate { get; set; }
        public virtual SchoolManager SchoolManager { get; set; }

        public string PhotoURI
        {
            get
            {
                return $"defaultPhoto.png";

                //LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                //return $"{proxy.GetBasePhotoUri()}{this.InstructorId}.jpg";
            }
        }
    }
}
