using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using LicenseApp.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace LicenseApp.ViewModels
{
    public class ShowInstructorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

        public ICommand SendEnrollmentCommand => new Command(SendEnrollmentRequest);

        public void SendEnrollmentRequest()
        {
            App app = (App)App.Current;
        }
    }
}
