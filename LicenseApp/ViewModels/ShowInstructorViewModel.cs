using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using LicenseApp.Models;
using System.Windows.Input;
using Xamarin.Forms;
using LicenseApp.Services;

namespace LicenseApp.ViewModels
{
    public class ShowInstructorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        const int WAITING_STATUS = 1;

        private Area area;
        public Area Area
        {
            get { return area; }
            set
            {
                area = value;
                OnPropertyChanged("Area");
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

        private string details;
        public string Details
        {
            get { return details; }
            set
            {
                details = value;
                OnPropertyChanged("Details");
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

        private string teachingArea;
        public string TeachingArea
        {
            get { return teachingArea; }
            set
            {
                teachingArea = value;
                OnPropertyChanged("TeachingArea");
            }
        }

        private string workingTime;
        public string WorkingTime
        {
            get { return workingTime; }
            set
            {
                workingTime = value;
                OnPropertyChanged("WorkingTime");
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

        public int instructorID;
        public int InstructorID
        {
            get { return instructorID; }
            set
            {
                instructorID = value;
                OnPropertyChanged("InstructorID");
            }
        }

        public ICommand SendEnrollmentCommand => new Command(SendEnrollmentRequest);

        public async void SendEnrollmentRequest()
        {
            App app = (App)App.Current;
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();

            EnrollmentRequest em = new EnrollmentRequest
            {
                StudentId = ((Student)app.CurrentUser).StudentId,
                InstructorId = InstructorID,
                StatusId = WAITING_STATUS
            };

            EnrollmentRequest newEm = await proxy.AddEnrollmentRequest(em);
            
            if(newEm != null)
            {
                await App.Current.MainPage.DisplayAlert("שגיאה", "בקשתך לרישום נשלחה בהצלחה למורה! חזור במועד מאוחר יותר על מנת לראות האם אושרת", "בסדר");
            }
            else
                await App.Current.MainPage.DisplayAlert("שגיאה", "אירעה שגיאה! בקשתך לא נשלחה. נסה שוב", "בסדר");
        }
    }
}
