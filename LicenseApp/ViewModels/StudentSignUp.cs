using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using LicenseApp.Services;
using LicenseApp.Models;
using System.Collections.ObjectModel;


namespace LicenseApp.ViewModels
{
    class StudentSignUp : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int sliderValue;
        public int SliderValue
        {
            get { return sliderValue; }
            set
            {
                sliderValue = value;
                OnPropertyChanged("SliderValue");
            }
        }

        public StudentSignUp()
        {
            App app = (App)App.Current;
            SliderValue = 0;
        }
    }
}
