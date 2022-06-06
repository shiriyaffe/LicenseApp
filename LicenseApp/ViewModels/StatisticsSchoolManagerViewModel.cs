using LicenseApp.Models;
using LicenseApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LicenseApp.ViewModels
{
    class StatisticsSchoolManagerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        const int APPROVED = 2;
        private const int NO_RATING = 0;

        //תכונות השומרות את מספר התלמידים החדשים שהצטרפו לבית הספר בפרק זמן מסוים
        private int newMonth;
        public int NewMonth
        {
            get
            {
                return this.newMonth;
            }
            set
            {
                if (this.newMonth != value)
                {

                    this.newMonth = value;
                    OnPropertyChanged("NewMonth");
                }
            }
        }

        private int newWeek;
        public int NewWeek
        {
            get
            {
                return this.newWeek;
            }
            set
            {
                if (this.newWeek != value)
                {

                    this.newWeek = value;
                    OnPropertyChanged("NewWeek");
                }
            }
        }

        private int newToday;
        public int NewToday
        {
            get
            {
                return this.newToday;
            }
            set
            {
                if (this.newToday != value)
                {

                    this.newToday = value;
                    OnPropertyChanged("NewToday");
                }
            }
        }

        //דירוג ממוצע של כלל המורים בבית הספר
        private int ratingValue;
        public int RatingValue
        {
            get { return ratingValue; }
            set
            {
                ratingValue = value;
                OnPropertyChanged("RatingValue");
            }
        }

        //פעולה המחשבת את ממוצע הדירוגים של כל המורים המשויכים למנהל המחובר
        private int GetAverage()
        {
            App app = (App)App.Current;
            int counter = 0;
            int sumRate = 0;

            if(app.CurrentUser is SchoolManager)
            {
                SchoolManager current = (SchoolManager)app.CurrentUser;

                foreach(Instructor i in current.Instructors)
                {
                    if(i.RateId != NO_RATING)
                    {
                        sumRate += (int)i.RateId;
                        counter++;
                    }
                }

                return sumRate / counter;
            }

            return counter;
        }

        public StatisticsSchoolManagerViewModel()
        {
            NewMonth = 0;
            NewWeek = 0;
            NewToday = 0;
            RatingValue = GetAverage();

            App app = (App)App.Current;
            app.RefreshUI += OnRefresh;

            GetNewStudentsThisMonth();
            GetNewStudentsThisWeek();
            GetNewStudentsToday();
        }

        //פעולה המרעננת את המסך
        public void OnRefresh()
        {
            GetNewStudentsThisMonth();
            GetNewStudentsThisWeek();
            GetNewStudentsToday();
            GetAverage();
        }

        //פעולה המעדכנת את מספר התלמידים שהצטרפו החודש לבית הספר
        private async void GetNewStudentsThisMonth()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            App app = (App)App.Current;

            List<Student> students = new List<Student>();
            students = await proxy.GetNewStudents(1);

            if (students != null)
            {
                foreach(Student s in students)
                {
                    if(s.EStatusId == APPROVED)
                    if (s.Instructor != null && s.Instructor.SchoolManagerId != ((SchoolManager)app.CurrentUser).SmanagerId)
                        students.Remove(s);
                }

                NewMonth = students.Count;
            }   

        }

        //פעולה המעדכנת את מספר התלמידים שהצטרפו השבוע לבית הספר
        private async void GetNewStudentsThisWeek()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            App app = (App)App.Current;

            List<Student> students = new List<Student>();
            students = await proxy.GetNewStudents(2);

            if (students != null)
            {
                foreach (Student s in students)
                {
                    if (s.EStatusId == APPROVED)
                        if (s.Instructor != null && s.Instructor.SchoolManagerId != ((SchoolManager)app.CurrentUser).SmanagerId)
                        students.Remove(s);
                }
                NewWeek = students.Count;
            }
        }

        //פעולה המעדכנת את מספר התלמידים שהצטרפו היום לבית הספר
        private async void GetNewStudentsToday()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            App app = (App)App.Current;

            List<Student> students = new List<Student>();
            students = await proxy.GetNewStudents(3);

            if (students != null)
            {
                foreach (Student s in students)
                {
                    if (s.EStatusId == APPROVED)
                        if (s.Instructor != null && s.Instructor.SchoolManagerId != ((SchoolManager)app.CurrentUser).SmanagerId)
                        students.Remove(s);
                }

                NewToday = students.Count;
            }
        }
    }
}
