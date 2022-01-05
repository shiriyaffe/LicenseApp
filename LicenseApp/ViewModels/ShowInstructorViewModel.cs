using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace LicenseApp.ViewModels
{
    public class ShowInstructorViewModel : INotifyPropertyChanged
    {
        public string IName { get; set; }
        public string Details { get; set; }
        public string ImageUrl { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
