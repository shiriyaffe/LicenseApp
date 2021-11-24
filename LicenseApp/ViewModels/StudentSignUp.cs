using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using LicenseApp.Models;
using LicenseApp.Views;
using System.Collections.ObjectModel;
using LicenseApp.Services;



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

        public List<City> Cities 
        { 
            get 
            {
                if (((App)App.Current).Tables != null)  
                    return ((App)App.Current).Tables.Cities;
                return new List<City>();
            } 
        }

        private City city;
        public City City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged("City");
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

        private string address;
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }

        private string submitError;

        public string SubmitError
        {
            get => submitError;
            set
            {
                submitError = value;
                OnPropertyChanged("SubmitError");
            }
        }

        private bool showError;

        public bool ShowError
        {
            get => showError;
            set
            {
                showError = value;
                OnPropertyChanged("ShowNextError");
            }
        }

        public StudentSignUp()
        {
            SliderValue = 0;
        }

        public Command SignUpCommand => new Command(SignUpAsStudent);

        public async void SignUpAsStudent()
        {
            App app = (App)App.Current;
            
            Student s = new Student
            {
                Sname = app.TempUser.Name,
                Email = app.TempUser.Email,
                Pass = app.TempUser.UserPswd,
                Birthday = app.TempUser.BirthDate,
                PhoneNumber = app.TempUser.PhoneNumber,
                GenderId = app.TempUser.GenderID.GenderId,
                Saddress = address,
                CityId = City.CityId,
                GearboxId = Gearbox.GearboxId,
                LicenseTypeId = LicenseType.LicenseTypeId,
                TeacherGender = Gender.GenderId,
                HighestPrice = SliderValue,
                LessonsCount = 0,
                RegistrationDate = DateTime.Today
            };

            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            Student student = await proxy.StudentSignUpAsync(s);

            if(student != null)
            {
                app.CurrentUser = s;
                app.MainPage = new NavigationPage(new HomePageView());
            }
            else
            {
                SubmitError = "ההרשמה נכשלה! נסה שנית";
                ShowError = true;
            }
        }
    }
}
