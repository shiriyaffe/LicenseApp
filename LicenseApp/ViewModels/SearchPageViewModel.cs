using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Essentials;
using System.ComponentModel;
using LicenseApp.Models;
using LicenseApp.Services;
using LicenseApp.Views;



namespace LicenseApp.ViewModels
{
    public class SearchPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<Instructor> instructors;
        public List<Instructor> instructorsList;


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

        public List<Area> Areas
        {
            get
            {
                if (((App)App.Current).Tables != null)
                    return ((App)App.Current).Tables.Areas;
                return new List<Area>();
            }
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

        public List<Gender> Genders
        {
            get
            {
                if (((App)App.Current).Tables != null)
                    return ((App)App.Current).Tables.Genders;
                return new List<Gender>();
            }
        }

        private Gender gender;
        public Gender Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                OnPropertyChanged("Gender");
            }
        }

        public List<Gearbox> Gearboxes
        {
            get
            {
                if (((App)App.Current).Tables != null)
                    return ((App)App.Current).Tables.GearBoxes;
                return new List<Gearbox>();
            }
        }

        private Gearbox gearbox;
        public Gearbox Gearbox
        {
            get { return gearbox; }
            set
            {
                gearbox = value;
                OnPropertyChanged("Gearbox");
            }
        }

        public List<LicenseType> LicenseTypes
        {
            get
            {
                if (((App)App.Current).Tables != null)
                    return ((App)App.Current).Tables.LicenseTypes;
                return new List<LicenseType>();
            }
        }

        private LicenseType licenseType;
        public LicenseType LicenseType
        {
            get { return licenseType; }
            set
            {
                licenseType = value;
                OnPropertyChanged("LicenseType");
            }
        }

        public SearchPageViewModel()
        {
            SliderValue = 0;
        }

        public ICommand SearchCommand => new Command(SearchInstructor);

        public async void SearchInstructor()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            instructors = await proxy.GetAllInstructorsAsync();
            HomePageViewModel page = new HomePageViewModel();

            foreach (Instructor i in instructors)
            {
                if(i.AreaId == Area.AreaId && i.Price == SliderValue && i.GenderId == Gender.GenderId && i.GearboxId == Gearbox.GearboxId && LicenseType.LicenseTypeId == i.LicenseTypeId)
                {
                    page.InstructorList.Add(i);
                }
            }

            Page p = new HomePageView();
            p.BindingContext = page;
            App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
