using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using LicenseApp.Services;

namespace LicenseApp.Models
{
    public partial class Student
    {
        public int StudentId { get; set; }
        public string Sname { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public string PhoneNumber { get; set; }
        public int GenderId { get; set; }
        public DateTime Birthday { get; set; }
        public int CityId { get; set; }
        public string Saddress { get; set; }
        public int GearboxId { get; set; }
        public int LicenseTypeId { get; set; }
        public int? TeacherGender { get; set; }
        public int LessonLengthId { get; set; }
        public int HighestPrice { get; set; }
        public int? InstructorId { get; set; }
        public int LessonsCount { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int? EStatusId { get; set; }

        public virtual City City { get; set; }
        public virtual Estatus EStatus { get; set; }
        public virtual Gearbox Gearbox { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual Instructor Instructor { get; set; }
        public virtual LessonLength LessonLength { get; set; }
        public virtual LicenseType LicenseType { get; set; }
        public virtual ICollection<EnrollmentRequest> EnrollmentRequests { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<StudentSummary> StudentSummaries { get; set; }

        public string PhotoURI
        {
            get
            {
                LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                //Create a source with cache busting!
                Random r = new Random();
                return $"{proxy.GetBasePhotoUri()}Students/{this.StudentId}.jpg?{r.Next()}";
            }
        }

        //public async void GetLessonsCount()
        //{
        //    int count = 0;
        //    LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
        //    if (StudentId > 0)
        //    {
        //        ObservableCollection<Lesson> lessons = await proxy.GetStudentLessonsAsync(StudentId);
        //        foreach (Lesson l in lessons)
        //        {
        //            if (l.HasDone)
        //                count++;
        //        }
        //    }

        //    LessonsCount = count;
        //}
    }
}
