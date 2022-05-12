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

        public StatisticsSchoolManagerViewModel()
        {
            NewMonth = 0;
            NewWeek = 0;
            NewToday = 0;

            App app = (App)App.Current;
            app.RefreshUI += OnRefresh;

            GetNewStudentsThisMonth();
            GetNewStudentsThisWeek();
            GetNewStudentsToday();
        }

        public void OnRefresh()
        {
            GetNewStudentsThisMonth();
            GetNewStudentsThisWeek();
            GetNewStudentsToday();
        }

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
