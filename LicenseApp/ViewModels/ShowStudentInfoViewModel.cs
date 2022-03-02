using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LicenseApp.ViewModels
{
    public class ShowStudentInfoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        private int sAge;
        public int SAge
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

    }
}
