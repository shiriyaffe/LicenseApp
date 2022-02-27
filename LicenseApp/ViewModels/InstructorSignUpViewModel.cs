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
    public class InstructorSignUpViewModel : INotifyPropertyChanged
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

        public List<DrivingSchool> DrivingSchools
        {
            get
            {
                if (((App)App.Current).Tables != null)
                    return ((App)App.Current).Tables.DrivingSchools;
                return new List<DrivingSchool>();
            }
        }

        private DrivingSchool drivingSchool;
        public DrivingSchool DrivingSchool
        {
            get { return drivingSchool; }
            set
            {
                drivingSchool = value;
                OnPropertyChanged("DrivingSchool");
            }
        }

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
            this.ShowDetailError = (string.IsNullOrEmpty(InstructorDetails) || !Regex.IsMatch(this.InstructorDetails, @"^[\u0590-\u05FF ]+$"));
            if (ShowDetailError)
                DetailError = "שדה זה אינו תקין";
            else
                DetailError = null;
        }
        #endregion

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
                OnPropertyChanged("ShowError");
            }
        }

        #region מקור התמונה
        private string instructorImgSrc;

        public string InstructorImgSrc
        {
            get => instructorImgSrc;
            set
            {
                instructorImgSrc = value;
                OnPropertyChanged("InstructorImgSrc");
            }
        }
        private const string DEFAULT_PHOTO_SRC = "Default.jpg";
        #endregion

        public InstructorSignUpViewModel()
        {
            SliderValue = 0;
            ShowError = false;
        }

        public Command SignUpCommand => new Command(SignUpAsInstructor);

        public async void SignUpAsInstructor()
        {
            App app = (App)App.Current;

            if (!ShowDetailError)
            {
                Instructor i = new Instructor
                {
                    Iname = app.TempUser.Name,
                    Email = app.TempUser.Email,
                    Pass = app.TempUser.UserPswd,
                    Birthday = app.TempUser.BirthDate,
                    PhoneNumber = app.TempUser.PhoneNumber,
                    GenderId = app.TempUser.Gender.GenderId,
                    Price = SliderValue,
                    AreaId = Area.AreaId,
                    GearboxId = Gearbox.GearboxId,
                    LicenseTypeId = LicenseType.LicenseTypeId,
                    LessonLengthId = LessonLength.LessonLengthId,
                    RateId = 6,
                    StartTime = StartHour.Whour,
                    EndTime = EndHour.Whour,
                    DrivingSchoolId = DrivingSchool.SchoolId,
                    RegistrationDate = DateTime.Today,
                    Details = instructorDetails
                };

                LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                Instructor instructor = await proxy.InstructorSignUpAsync(i);

                if (instructor != null)
                {
                    app.CurrentUser = instructor;
                    app.MainPage = new NavigationPage(new InstructorMainTabView());
                }
                else
                    await App.Current.MainPage.DisplayAlert("שגיאה", "אירעה שגיאה! לא ניתן להמשיך בהרשמה", "בסדר");
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

