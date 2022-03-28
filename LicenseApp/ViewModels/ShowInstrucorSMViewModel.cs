using LicenseApp.Models;
using LicenseApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace LicenseApp.ViewModels
{
    public class ShowInstrucorSMViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }

        private string iName;
        public string IName
        {
            get { return iName; }
            set
            {
                iName = value;
                OnPropertyChanged("IName");
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

        public int price;
        public int Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }

        public int lessonLength;
        public int LessonLength
        {
            get { return lessonLength; }
            set
            {
                lessonLength = value;
                OnPropertyChanged("LessonLength");
            }
        }

        public int sLength;
        public int SLength
        {
            get { return sLength; }
            set
            {
                sLength = value;
                OnPropertyChanged(" SLength");
            }
        }

        //private async void GetLessonLength()
        //{
        //    LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
        //    LessonLength l = await proxy.GetLessonLengthById(LessonLength);
        //    if (l != null)
        //    {
        //        this.SLength = l.Slength;
        //    }
        //}

        private int instructorID;
        public int InstructorID
        {
            get { return instructorID; }
            set
            {
                instructorID = value;
                OnPropertyChanged("InstructorID");
            }
        }

        public Command DeleteStudentCommand => new Command(DeleteStudent);

        public async void DeleteStudent()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            Instructor i = await proxy.DeleteInstructor(InstructorID);

            Page p = new Views.SchoolManagerMainTabView();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        public Command OpenStudentsList => new Command(StudentsList);
        public async void StudentsList()
        {
            Page p = new Views.ListOfStudentsByInstructorId(InstructorID);
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

    }
}
