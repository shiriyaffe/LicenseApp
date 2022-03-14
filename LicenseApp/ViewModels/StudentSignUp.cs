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

        public List<LessonLength> LessonLengths
        {
            get
            {
                if (((App)App.Current).Tables != null)
                    return ((App)App.Current).Tables.LessonLengths;
                return new List<LessonLength>();
            }
        }

        private LessonLength lessonLength;
        public LessonLength LessonLength
        {
            get { return lessonLength; }
            set
            {
                lessonLength = value;
                OnPropertyChanged("LessonLength");
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

        #region מקור התמונה
        private string studentImgSrc;

        public string StudentImgSrc
        {
            get => studentImgSrc;
            set
            {
                studentImgSrc = value;
                OnPropertyChanged("StudentImgSrc");
            }
        }
        private const string DEFAULT_PHOTO_SRC = "defaultPhoto.png";
        #endregion

        public StudentSignUp()
        {
            SliderValue = 0;
            ShowError = false;
            this.StudentImgSrc = DEFAULT_PHOTO_SRC;
        }

        public Command SignUpCommand => new Command(SignUpAsStudent);

        public async void SignUpAsStudent()
        {
            App app = (App)App.Current;
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();

            Student s = new Student
            {
                Sname = app.TempUser.Name,
                Email = app.TempUser.Email,
                Pass = app.TempUser.UserPswd,
                Birthday = app.TempUser.BirthDate,
                PhoneNumber = app.TempUser.PhoneNumber,
                GenderId = app.TempUser.Gender.GenderId,
                Saddress = address,
                CityId = City.CityId,
                GearboxId = Gearbox.GearboxId,
                LicenseTypeId = LicenseType.LicenseTypeId,
                LessonLengthId = LessonLength.LessonLengthId,
                TeacherGender = Gender.GenderId,
                HighestPrice = SliderValue,
                LessonsCount = 0,
                RegistrationDate = DateTime.Today,
                InstructorId = 1
            };

           
            Student student = await proxy.StudentSignUpAsync(s);

            if(student != null)
            {
                if (app.TempUser.UserImg != null)
                {
                    bool success = await proxy.UploadImage(new FileInfo()
                    {
                        Name = app.TempUser.UserImg
                    }, $"Students\\{student.StudentId}.jpg");
                }

                app.CurrentUser = student;

                app.MainPage = new NavigationPage(new StudentMainTabView());
            }
            else
            {
                //SubmitError = "ההרשמה נכשלה! נסה שנית";
                //ShowError = true;
                await App.Current.MainPage.DisplayAlert("שגיאה", "ההרשמה נכשלה! בדוק את הפרטים שהזנת ונסה שנית", "בסדר");
            }
        }
    }
}
