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

        const int STATUS_ID = 4;

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

        #region city
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

        public int cityPicker;
        public int CityPicker
        {
            get { return cityPicker; }
            set
            {
                cityPicker = value;
                ValidateCity();
                OnPropertyChanged("CityPicker");
            }
        }

        private string cityError;

        public string CityError
        {
            get => cityError;
            set
            {
                cityError = value;
                OnPropertyChanged("CityError");
            }
        }

        private bool showCityError;

        public bool ShowCityError
        {
            get => showCityError;
            set
            {
                showCityError = value;
                OnPropertyChanged("ShowCityError");
            }
        }

        public void ValidateCity()
        {
            this.ShowCityError = CityPicker == -1;
            if (this.ShowCityError)
            {
                this.CityError = "מגדר הוא שדה חובה!";
            }
            else
                this.CityError = null;
        }
        #endregion

        #region gender
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

        public int genderPicker;
        public int GenderPicker
        {
            get { return genderPicker; }
            set
            {
                genderPicker = value;
                ValidateGender();
                OnPropertyChanged("GenderPicker");
            }
        }

        private string genderError;

        public string GenderError
        {
            get => genderError;
            set
            {
                genderError = value;
                OnPropertyChanged("GenderError");
            }
        }

        private bool showGenderError;

        public bool ShowGenderError
        {
            get => showGenderError;
            set
            {
                showGenderError = value;
                OnPropertyChanged("ShowGenderError");
            }
        }

        public void ValidateGender()
        {
            this.ShowGenderError = GenderPicker == -1;
            if (this.ShowGenderError)
            {
                this.GenderError = "מגדר הוא שדה חובה!";
            }
            else
                this.GenderError = null;
        }
        #endregion

        #region gearbox
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

        public int gearboxPicker;
        public int GearboxPicker
        {
            get { return gearboxPicker; }
            set
            {
                gearboxPicker = value;
                ValidateGearbox();
                OnPropertyChanged("GearboxPicker");
            }
        }

        private string gearboxError;

        public string GearboxError
        {
            get => gearboxError;
            set
            {
                gearboxError = value;
                OnPropertyChanged("GearboxError");
            }
        }

        private bool showGearboxError;

        public bool ShowGearboxError
        {
            get => showGearboxError;
            set
            {
                showGearboxError = value;
                OnPropertyChanged("ShowGearboxError");
            }
        }

        public void ValidateGearbox()
        {
            this.ShowGearboxError = GearboxPicker == -1;
            if (this.ShowGearboxError)
            {
                this.GearboxError = "תיבת הילוכים הוא שדה חובה!";
            }
            else
                this.GearboxError = null;
        }
        #endregion

        #region license type
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

        public int licenseTypePicker;
        public int LicenseTypePicker
        {
            get { return licenseTypePicker; }
            set
            {
                licenseTypePicker = value;
                ValidateLicenseType();
                OnPropertyChanged("LicenseTypePicker");
            }
        }

        private string licenseTypeError;

        public string LicenseTypeError
        {
            get => licenseTypeError;
            set
            {
                licenseTypeError = value;
                OnPropertyChanged("LicenseTypeError");
            }
        }

        private bool showLicenseTypeError;

        public bool ShowLicenseTypeError
        {
            get => showLicenseTypeError;
            set
            {
                showLicenseTypeError = value;
                OnPropertyChanged("ShowLicenseTypeError");
            }
        }

        public void ValidateLicenseType()
        {
            this.ShowLicenseTypeError = LicenseTypePicker == -1;
            if (this.ShowLicenseTypeError)
            {
                this.LicenseTypeError = "סוג רישיון הוא שדה חובה!";
            }
            else
                this.LicenseTypeError = null;
        }
        #endregion

        #region lesson length
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

        public int lessonLengthPicker;
        public int LessonLengthPicker
        {
            get { return lessonLengthPicker; }
            set
            {
                lessonLengthPicker = value;
                ValidateLessonLength();
                OnPropertyChanged("LessonLengthPicker");
            }
        }

        private string lessonLengthError;

        public string LessonLengthError
        {
            get => lessonLengthError;
            set
            {
                lessonLengthError = value;
                OnPropertyChanged("LessonLengthError");
            }
        }

        private bool showLessonLengthError;

        public bool ShowLessonLengthError
        {
            get => showLessonLengthError;
            set
            {
                showLessonLengthError = value;
                OnPropertyChanged("ShowLessonLengthError");
            }
        }

        public void ValidateLessonLength()
        {
            this.ShowLessonLengthError = LessonLengthPicker == -1;
            if (this.ShowLessonLengthError)
            {
                this.LessonLengthError = "אורך שיעור הוא שדה חובה!";
            }
            else
                this.LessonLengthError = null;
        }
        #endregion

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
            GenderPicker = -1;
            CityPicker = -1;
            GearboxPicker = -1;
            LicenseTypePicker = -1;
            LessonLengthPicker = -1;

            ShowLicenseTypeError = false;
            ShowLessonLengthError = false;
            ShowGearboxError = false;
            ShowCityError = false;
            ShowGenderError = false;


            SliderValue = 0;
            ShowError = false;
            this.StudentImgSrc = DEFAULT_PHOTO_SRC;
        }

        private bool ValidateForm()
        {
            //Validate all fields first
            ValidateGender();
            ValidateCity();
            ValidateGearbox();
            ValidateLessonLength();
            ValidateLicenseType();


            //check if any validation failed
            if (ShowGenderError || ShowCityError || ShowGearboxError || ShowLessonLengthError || ShowLicenseTypeError)
                return false;
            return true;
        }

        public Command SignUpCommand => new Command(SignUpAsStudent);

        public async void SignUpAsStudent()
        {
            App app = (App)App.Current;
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();

            if (ValidateForm())
            {
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
                    EStatusId = STATUS_ID
                };


                Student student = await proxy.StudentSignUpAsync(s);

                if (student != null)
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
                    await App.Current.MainPage.DisplayAlert("שגיאה", "ההרשמה נכשלה! בדוק את הפרטים שהזנת ונסה שנית", "בסדר");
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("שגיאה", "אירעה שגיאה! לא ניתן להמשיך בהרשמה", "בסדר");
            }
        }
    }
}
