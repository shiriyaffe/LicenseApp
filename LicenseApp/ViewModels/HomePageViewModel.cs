using LicenseApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Essentials;
using LicenseApp.Models;

namespace LicenseApp.ViewModels
{
    public class HomePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Instructor> InstructorList { get; }

        public HomePageViewModel()
        {
            InstructorList = new ObservableCollection<Instructor>();
            CreateInstructorCollection();
        }

        async void CreateInstructorCollection()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            List<Instructor> instructors = await proxy.GetAllInstructorsAsync();
            foreach (Instructor i in instructors)
            {
                this.InstructorList.Add(i);
            }
        }
    }
}
