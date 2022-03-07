using LicenseApp.Models;
using LicenseApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LicenseApp.ViewModels
{
    public class InstructorProfileViewModel : INotifyPropertyChanged
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
            await App.Current.MainPage.DisplayAlert("הסיסמה חייבת להכיל:", "- ספרה אחת\n- אות אחת באנגלית\n- אורך מקסימלי 8 ספרות", "אישור", FlowDirection.RightToLeft);
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

        #region area
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

        private string areaName;
        public string AreaName
        {
            get { return areaName; }
            set
            {
                areaName = value;
                OnPropertyChanged("AreaName");
            }
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
        #endregion

        #region Working Hours
        public List<WorkingHour> WorkingHours
        {
            get
            {
                if (((App)App.Current).Tables != null)
                    return ((App)App.Current).Tables.WorkingHours;
                return new List<WorkingHour>();
            }
        }

        private WorkingHour startHour;
        public WorkingHour StartHour
        {
            get { return startHour; }
            set
            {
                startHour = value;
                OnPropertyChanged("StartHour");
            }
        }

        private string starTime;
        public string StartTime
        {
            get { return starTime; }
            set
            {
                starTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        private string endTime;
        public string EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                OnPropertyChanged("EndTime");
            }
        }

        private WorkingHour endHour;
        public WorkingHour EndHour
        {
            get { return endHour; }
            set
            {
                endHour = value;
                OnPropertyChanged("EndHour");
            }
        }
        #endregion

        #region פרטי המורה
        private string instructorDetails;

        public string InstructorDetails
        {
            get => instructorDetails;
            set
            {
                instructorDetails = value;
                ValidateDetails();
                OnPropertyChanged("InstructorDetails");
            }
        }

        private bool showDetailError;
        public bool ShowDetailError
        {
            get => showDetailError;
            set
            {
                showDetailError = value;
                OnPropertyChanged("ShowDetailError");
            }
        }

        private string detailError;

        public string DetailError
        {
            get => detailError;
            set
            {
                detailError = value;
                OnPropertyChanged("DetailError");
            }
        }

        public void ValidateDetails()
        {
            this.ShowDetailError = (string.IsNullOrEmpty(InstructorDetails));
            if (ShowDetailError)
                DetailError = "שדה זה אינו תקין";
            else
                DetailError = null;
        }
        #endregion

        #region UserImgSrc
        private string userImgSrc;
        public string UserImgSrc
        {
            get => userImgSrc;
            set
            {
                userImgSrc = value;
                OnPropertyChanged("UserImgSrc");
            }
        }
        private const string DEFAULT_PHOTO_SRC = "defaultPhoto.png";
        #endregion


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

       
        public InstructorProfileViewModel()
        {
            App app = (App)App.Current;
            if (app.CurrentUser != null && app.CurrentUser is Instructor)
            {
                Instructor i = (Instructor)app.CurrentUser;
                GetArea(i);
                GetGearbox(i);
                GetLessonLength(i);
                ImageUrl = i.PhotoURI;
                SliderValue = i.Price;
                PhoneNumber = i.PhoneNumber;
                Pass = i.Pass;
                InstructorDetails = i.Details;
                StartTime = i.StartTime;
                EndTime = i.EndTime;
                this.UserImgSrc = i.PhotoURI;
            }
            this.ShowPassError = false;
            this.ShowNumberError = false;
            ShowDetailError = false;
            this.SaveDataCommand = new Command(() => SaveData());
            this.PassConditions = new Command(() => ShowConditions());
        }

        private async void GetArea(Instructor i)
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            Area a = await proxy.GetAreaById(i.AreaId);
            if (a != null)
            {
                this.Area = a;
                this.AreaName = a.AreaName;
            }
            else
                this.AreaName = "אזור לימוד:";
        }

        private async void GetGearbox(Instructor i)
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            Gearbox g = await proxy.GetGearboxById(i.GearboxId);
            if (g != null)
            {
                this.Gearbox = g;
                this.GearboxName = g.Type;
            }
            else
                this.GearboxName = "תיבת הילוכים:";
        }

        private async void GetLessonLength(Instructor i)
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            LessonLength l = await proxy.GetLessonLengthById(i.LessonLengthId);
            if (l != null)
            {
                this.LessonLength = l;
                this.LessonLengthMin = l.Slength;
            }
            else
                this.GearboxName = "אורך שיעור מועדף (בדקות):";
        }

        private bool ValidateForm()
        {
            //Validate all fields first
            ValidatePass();
            ValidateNumber();
            //ValidateDetails();


            //check if any validation failed
            if (ShowNumberError || ShowPassError)
                return false;
            return true;
        }

        public Command SaveDataCommand { protected set; get; }

        private async void SaveData()
        {
            if (ValidateForm())
            {
                App theApp = (App)App.Current;
                Instructor newInstructor = new Instructor()
                {
                    InstructorId = ((Instructor)theApp.CurrentUser).InstructorId,
                    Email = ((Instructor)theApp.CurrentUser).Email,
                    Iname = ((Instructor)theApp.CurrentUser).Iname,
                    GenderId = ((Instructor)theApp.CurrentUser).GenderId,
                    Birthday = ((Instructor)theApp.CurrentUser).Birthday,
                    LicenseTypeId = ((Instructor)theApp.CurrentUser).LicenseTypeId,
                    DrivingSchoolId = ((Instructor)theApp.CurrentUser).DrivingSchoolId,
                    SchoolManagerId = ((Instructor)theApp.CurrentUser).SchoolManagerId,
                    RateId = ((Instructor)theApp.CurrentUser).RateId,
                    RegistrationDate = ((Instructor)theApp.CurrentUser).RegistrationDate,
                    StartTime = this.StartHour.Whour,
                    EndTime = this.EndHour.Whour,
                    Pass = this.Pass,
                    PhoneNumber = this.PhoneNumber,
                    AreaId = this.Area.AreaId,
                    GearboxId = this.Gearbox.GearboxId,
                    LessonLengthId = this.LessonLength.LessonLengthId,
                    Price = this.SliderValue,
                    Details = InstructorDetails
                };

                LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                Instructor instructor = await proxy.UpdateInstructor(newInstructor);

                if (instructor == null)
                {
                    await App.Current.MainPage.DisplayAlert("שגיאה", "העדכון נכשל", "אישור", FlowDirection.RightToLeft);
                }
                else
                {
                    //if (this.imageFileResult != null)
                    //{
                    //    bool success = await proxy.UploadImage(new Models.FileInfo()
                    //    {
                    //        Name = this.imageFileResult.FullPath
                    //    }, $"{instructor.InstructorId}.jpg");

                    //    //if (!success)
                    //    //{
                    //    //    if (SetImageSourceEvent != null)
                    //    //        SetImageSourceEvent(theApp.CurrentUser.PhotoURL);
                    //    //    await App.Current.MainPage.DisplayAlert("עדכון", "יש בעיה בהעלאת התמונה", "אישור", FlowDirection.RightToLeft);
                    //    //}
                    //}

                    //theApp.CurrentUser = instructor;
                    //await App.Current.MainPage.Navigation.PopModalAsync();
                    //Page page = new AdultMainTab();
                    //page.Title = $"שלום {user.UserName}";
                    //App.Current.MainPage = new NavigationPage(page) { BarBackgroundColor = Color.FromHex("#81cfe0") };
                    //await App.Current.MainPage.DisplayAlert("עדכון", "העדכון בוצע בהצלחה", "אישור", FlowDirection.RightToLeft);

                    theApp.CurrentUser = instructor;
                    await App.Current.MainPage.DisplayAlert("עדכון", "העדכון בוצע בהצלחה", "אישור", FlowDirection.RightToLeft);
                }
            }
            else
                await App.Current.MainPage.DisplayAlert("שמירת נתונים", " יש בעיה עם הנתונים בדוק ונסה שוב", "אישור", FlowDirection.RightToLeft);
        }
    }
}
