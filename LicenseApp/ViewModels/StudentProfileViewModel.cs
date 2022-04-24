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
using Xamarin.Essentials;
using System.Collections.ObjectModel;

namespace LicenseApp.ViewModels
{
    public class StudentProfileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        const string ERROR_PIC = "Error.png";
        const string Correct_PIC = "Correct.png";

        #region password
        private string pass;
        public string Pass
        {
            get { return pass; }
            set
            {
                pass = value;
                ValidatePass();
                OnPropertyChanged("Pass");
            }
        }

        private string passError;

        public string PassError
        {
            get => passError;
            set
            {
                passError = value;
                OnPropertyChanged("PassError");
            }
        }

        private bool showPassError;

        public bool ShowPassError
        {
            get => showPassError;
            set
            {
                showPassError = value;
                OnPropertyChanged("ShowPassError");
            }
        }

        private void ValidatePass()
        {

            this.ShowPassError = string.IsNullOrEmpty(Pass);
            if (!this.ShowPassError)
            {
                if (!Regex.IsMatch(this.Pass, @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"))
                {
                    this.ShowPassError = true;
                    this.PassError = ERROR_PIC;
                }
                else
                {
                    this.ShowPassError = false;
                    this.PassError = Correct_PIC;
                }
            }
            else
            {
                this.ShowPassError = true;
                this.PassError = ERROR_PIC;
            }
        }

        public Command PassConditions { protected set; get; }

        private async void ShowConditions()
        {
            await App.Current.MainPage.DisplayAlert("הסיסמה חייבת להכיל:", "- ספרה אחת\n- אות אחת באנגלית\n- אורך מינימלי 8 ספרות", "אישור", FlowDirection.RightToLeft);
        }
        #endregion

        #region phone number
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
            if (!string.IsNullOrEmpty(PhoneNumber))
            {
                if (!Regex.IsMatch(this.PhoneNumber, @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"))
                {
                    this.ShowNumberError = true;
                    this.NumberError = ERROR_PIC;
                }
                else
                { 
                    this.NumberError = Correct_PIC;
                    this.ShowNumberError = false;
                }
            }
            else
            {
                this.NumberError = ERROR_PIC;
                this.ShowNumberError = true;
            }
        }
        #endregion

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
        #endregion

        #region image
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

        FileResult imageFileResult;
        public event Action<ImageSource> SetImageSourceEvent;
        #region PickImage
        public ICommand PickImageCommand => new Command(OnPickImage);
        public async void OnPickImage()
        {
            FileResult result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions()
            {
                Title = "בחר תמונה"
            });

            if (result != null)
            {
                this.imageFileResult = result;

                var stream = await result.OpenReadAsync();
                ImageSource imgSource = ImageSource.FromStream(() => stream);
                if (SetImageSourceEvent != null)
                    SetImageSourceEvent(imgSource);
            }
        }
        #endregion

        //The following command handle the take photo button
        #region CameraImage
        public ICommand CameraImageCommand => new Command(OnCameraImage);
        public async void OnCameraImage()
        {
            var result = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions()
            {
                Title = "צלם תמונה"
            });

            if (result != null)
            {
                this.imageFileResult = result;
                var stream = await result.OpenReadAsync();
                ImageSource imgSource = ImageSource.FromStream(() => stream);
                if (SetImageSourceEvent != null)
                    SetImageSourceEvent(imgSource);
            }
        }
        #endregion
        #endregion

        private ObservableCollection<Lesson> lessonsList;
        public ObservableCollection<Lesson> LessonsList
        {
            get
            {
                return this.lessonsList;
            }
            set
            {
                if (this.lessonsList != value)
                {

                    this.lessonsList = value;
                    OnPropertyChanged("LessonsList");
                }
            }
        }

        public async void CreateLessonsCollection()
        {
            App app = (App)App.Current;
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            ObservableCollection<Lesson> lessons = await proxy.GetStudentLessonsAsync(((Student)app.CurrentUser).StudentId);
            foreach (Lesson l in lessons)
            {
                if(l.HasDone)
                    this.LessonsList.Add(l);
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
                ImageUrl = s.PhotoURI;
            }
            this.ShowNumberError = false;
            this.ShowPassError = false;
            this.SaveDataCommand = new Command(() => SaveData());
            this.PassConditions = new Command(() => ShowConditions());

            CreateLessonsCollection();
        }


        public Command SaveDataCommand { protected set; get; }

        private bool ValidateForm()
        {
            //Validate all fields first
            ValidatePass();
            ValidateNumber();


            //check if any validation failed
            if (ShowNumberError || ShowPassError)
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
                    if (this.imageFileResult != null)
                    {
                        bool success = await proxy.UploadImage(new Models.FileInfo()
                        {
                            Name = this.imageFileResult.FullPath
                        }, $"Students\\{student.StudentId}.jpg");
                    }

                    theApp.CurrentUser = student;
                    await App.Current.MainPage.DisplayAlert("", "העדכון בוצע בהצלחה", "אישור", FlowDirection.RightToLeft);
                }
            }
            else
                await App.Current.MainPage.DisplayAlert("שמירת נתונים", " יש בעיה עם הנתונים בדוק ונסה שוב", "אישור", FlowDirection.RightToLeft);
        }
    }
}
