using LicenseApp.Models;
using LicenseApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace LicenseApp.ViewModels
{
    public class ShowStudentInfoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int studentId;
        public int StudentId
        {
            get { return studentId; }
            set
            {
                studentId = value;
                OnPropertyChanged("StudentId");
            }
        }

        private string sName;
        public string SName
        {
            get { return sName; }
            set
            {
                sName = value;
                OnPropertyChanged("SName");
            }
        }

        private string imageUrl;
        public string ImageUrl
        {
            get { return imageUrl; }
            set
            {
                imageUrl = value;
                OnPropertyChanged("ImageUrl");
            }
        }

        private string phoneNum;
        public string PhoneNum
        {
            get { return phoneNum; }
            set
            {
                phoneNum = value;
                OnPropertyChanged("PhoneNum");
            }
        }

        private int lessonsCount;
        public int LessonsCount
        {
            get { return lessonsCount; }
            set
            {
                lessonsCount = value;
                OnPropertyChanged("LessonsCount");
            }
        }

        private double sAge;
        public double SAge
        {
            get { return sAge; }
            set
            {
                sAge = value;
                OnPropertyChanged("SAge");
            }
        }

        private DateTime summeryDate;
        public DateTime SummeryDate
        {
            get { return summeryDate; }
            set
            {
                summeryDate = value;
                OnPropertyChanged("SummeryDate");
            }
        }

        private string lessonSummery;
        public string LessonSummery
        {
            get { return lessonSummery; }
            set
            {
                lessonSummery = value;
                OnPropertyChanged("LessonSummery");
            }
        }

        private string sCity;
        public string SCity
        {
            get { return sCity; }
            set
            {
                sCity = value;
                OnPropertyChanged("SCity");
            }
        }


        public Command DeleteStudentCommand => new Command(DeleteStudent);

        public async void DeleteStudent()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            Student s = await proxy.DeleteStudent(StudentId);

            Page p = new Views.InstructorMainTabView();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
