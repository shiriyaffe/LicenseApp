using LicenseApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using LicenseApp.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Text.RegularExpressions;

namespace LicenseApp.ViewModels
{
    public class StudentProfileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string pass;
        public string Pass
        {
            get { return pass; }
            set
            {
                pass = value;
                OnPropertyChanged("Pass");
            }
        }

        private string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
                ValidateNumber();
                OnPropertyChanged("PhoneNumber");
            }
        }

        private string numberError;
        public string NumberError
        {
            get => numberError;
            set
            {
                numberError = value;
                OnPropertyChanged("NumberError");
            }
        }

        private bool showNumberError;
        public bool ShowNumberError
        {
            get => showNumberError;
            set
            {
                showNumberError = value;
                OnPropertyChanged("ShowNumberError");
            }
        }

        public void ValidateNumber()
        {
            if (!this.ShowNumberError)
            {
                if (!Regex.IsMatch(this.PhoneNumber, @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"))
                {
                    this.ShowNumberError = true;
                    this.NumberError = "מספר טלפון חייב להיות בעל 10 ספרות";
                }
            }
            else
                this.NumberError = null;
        }

        private string sAddress;
        public string SAddress
        {
            get { return sAddress; }
            set
            {
                sAddress = value;
                OnPropertyChanged("SAddress");
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

        private string cityName;
        public string CityName 
        {
            get { return cityName; }
            set
            {
                cityName = value;
                OnPropertyChanged("CityName");
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

        private string gearboxName;
        public string GearboxName
        {
            get { return gearboxName; }
            set
            {
                gearboxName = value;
                OnPropertyChanged("GearboxName");
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

        private int lessonLengthMin;
        public int LessonLengthMin
        {
            get { return lessonLengthMin; }
            set
            {
                lessonLengthMin = value;
                OnPropertyChanged("LessonLengthMin");
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

        public StudentProfileViewModel()
        {
            App app = (App)App.Current;
            if (app.CurrentUser != null && app.CurrentUser is Student)
            {
                Student s = (Student)app.CurrentUser;
                Pass = s.Pass;
                PhoneNumber = s.PhoneNumber;
                SAddress = s.Saddress;
                GetCity(s);
                GetGearbox(s);
                GetLessonLength(s);
                ImageUrl = "defaultPhoto.png";
                this.ShowNumberError = false;
                this.SaveDataCommand = new Command(() => SaveData());
            }
        }

        private async void GetCity(Student s)
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            City c = await proxy.GetCityById(s.CityId);
            if (c != null)
            {
                this.City = c;
                this.CityName = c.CityName;
            }
            else
                this.CityName = "עיר:";
        }

        private async void GetGearbox(Student s)
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            Gearbox g = await proxy.GetGearboxById(s.GearboxId);
            if (g != null)
            {
                this.Gearbox = g;
                this.GearboxName = g.Type;
            }
            else
                this.GearboxName = "תיבת הילוכים:";
        }

        private async void GetLessonLength(Student s)
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            LessonLength l = await proxy.GetLessonLengthById(s.LessonLengthId);
            if (l != null)
            {
                this.LessonLength = l;
                this.LessonLengthMin = l.Slength;
            }
            else
                this.GearboxName = "אורך שיעור מועדף (בדקות):";
        }

        public Command SaveDataCommand { protected set; get; }

        private bool ValidateForm()
        {
            //Validate all fields first
            //ValidatePass();
            ValidateNumber();


            //check if any validation failed
            if (ShowNumberError)
                return false;
            return true;
        }

        private async void SaveData()
        {
            if (ValidateForm())
            {
                App theApp = (App)App.Current;
                Student newStudent = new Student
                {
                    StudentId = ((Student)theApp.CurrentUser).StudentId,
                    Sname = ((Student)theApp.CurrentUser).Sname,
                    Email = ((Student)theApp.CurrentUser).Email,
                    Pass = this.Pass,
                    PhoneNumber = this.PhoneNumber,
                    GenderId = ((Student)theApp.CurrentUser).GenderId,
                    Birthday = ((Student)theApp.CurrentUser).Birthday,
                    CityId = this.City.CityId,
                    Saddress = this.SAddress,
                    GearboxId = this.Gearbox.GearboxId,
                    LicenseTypeId = ((Student)theApp.CurrentUser).LicenseTypeId,
                    TeacherGender = ((Student)theApp.CurrentUser).TeacherGender,
                    LessonLengthId = this.LessonLength.LessonLengthId,
                    HighestPrice = ((Student)theApp.CurrentUser).HighestPrice,
                    InstructorId = ((Student)theApp.CurrentUser).InstructorId,
                    LessonsCount = ((Student)theApp.CurrentUser).LessonsCount,
                    RegistrationDate = ((Student)theApp.CurrentUser).RegistrationDate
                };

                LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                Student student = await proxy.UpdateStudent(newStudent);

                if (student == null)
                {
                    await App.Current.MainPage.DisplayAlert("שגיאה", "העדכון נכשל", "אישור", FlowDirection.RightToLeft);
                }
                else
                {
                    //if (this.imageFileResult != null)
                    //{
                    //    ServerStatus = "מעלה תמונה...";

                    //    bool success = await proxy.UploadImage(new Models.FileInfo()
                    //    {
                    //        Name = this.imageFileResult.FullPath
                    //    }, $"{user.Id}.jpg");

                    //    //if (!success)
                    //    //{
                    //    //    if (SetImageSourceEvent != null)
                    //    //        SetImageSourceEvent(theApp.CurrentUser.PhotoURL);
                    //    //    await App.Current.MainPage.DisplayAlert("עדכון", "יש בעיה בהעלאת התמונה", "אישור", FlowDirection.RightToLeft);
                    //    //}
                    //}
                    //ServerStatus = "שומר נתונים...";

                    theApp.CurrentUser = student;
                    await App.Current.MainPage.DisplayAlert("עדכון", "העדכון בוצע בהצלחה", "אישור", FlowDirection.RightToLeft);
                }
            }
            else
                await App.Current.MainPage.DisplayAlert("שמירת נתונים", " יש בעיה עם הנתונים בדוק ונסה שוב", "אישור", FlowDirection.RightToLeft);
        }
    }
}
