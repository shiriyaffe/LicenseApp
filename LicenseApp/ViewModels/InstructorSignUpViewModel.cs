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
using Xamarin.Essentials;
using System.Windows.Input;

namespace LicenseApp.ViewModels
{
    public class InstructorSignUpViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        const int STATUS_ID = 1;

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

        #region working hour
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

        public int startPicker;
        public int StartPicker
        {
            get { return startPicker; }
            set
            {
                startPicker = value;
                ValidateWHour();
                OnPropertyChanged("StartPicker");
            }
        }

        public int endPicker;
        public int EndPicker
        {
            get { return endPicker; }
            set
            {
                endPicker = value;
                ValidateWHour();
                OnPropertyChanged("EndPicker");
            }
        }

        private string wHourError;

        public string WHourError
        {
            get => wHourError;
            set
            {
                wHourError = value;
                OnPropertyChanged("WHourError");
            }
        }

        private bool showWHourError;

        public bool ShowWHourError
        {
            get => showWHourError;
            set
            {
                showWHourError = value;
                OnPropertyChanged("ShowWHourError");
            }
        }

        public void ValidateWHour()
        {
            this.ShowWHourError = StartPicker == -1 || EndPicker == -1;
            if (this.ShowWHourError)
            {
                this.WHourError = "שעות התחלה וסיום הם שדות חובה!";
            }
            else
                this.WHourError = null;
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

        public int areaPicker;
        public int AreaPicker
        {
            get { return areaPicker; }
            set
            {
                areaPicker = value;
                ValidateArea();
                OnPropertyChanged("AreaPicker");
            }
        }

        private string areaError;

        public string AreaError
        {
            get => areaError;
            set
            {
                areaError = value;
                OnPropertyChanged("AreaError");
            }
        }

        private bool showAreaError;

        public bool ShowAreaError
        {
            get => showAreaError;
            set
            {
                showAreaError = value;
                OnPropertyChanged("ShowAreaError");
            }
        }

        public void ValidateArea()
        {
            this.ShowAreaError = AreaPicker == -1;
            if (this.ShowAreaError)
            {
                this.AreaError = "אזור לימוד הוא שדה חובה!";
            }
            else
                this.AreaError = null;
        }
        #endregion

        #region driving school
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

        public int drivingSchoolPicker;
        public int DrivingSchoolPicker
        {
            get { return drivingSchoolPicker; }
            set
            {
                drivingSchoolPicker = value;
                ValidateDrivingSchool();
                OnPropertyChanged("DrivingSchoolPicker");
            }
        }

        private string drivingSchoolError;

        public string DrivingSchoolError
        {
            get => drivingSchoolError;
            set
            {
                drivingSchoolError = value;
                OnPropertyChanged("DrivingSchoolError");
            }
        }

        private bool showDrivingSchoolError;

        public bool ShowDrivingSchoolError
        {
            get => showDrivingSchoolError;
            set
            {
                showDrivingSchoolError = value;
                OnPropertyChanged("ShowDrivingSchoolError");
            }
        }

        public void ValidateDrivingSchool()
        {
            this.ShowDrivingSchoolError = DrivingSchoolPicker == -1;
            if (this.ShowDrivingSchoolError)
            {
                this.DrivingSchoolError = "בית ספר לנהיגה הוא שדה חובה!";
            }
            else
                this.DrivingSchoolError = null;
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

        //פעולה הבודקת את תקינות הפרטים שהזין המורה
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
        //פעולה המופעלת בעת שהמשתמש בוחר תמונה חדשה מהמחשב ושומרת את התמונה שבחר
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
        //פעולה המופעלת בעת שהמשתמש מצלם תמונה חדשה והיא שומרת את התמונה שצילם
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

        public InstructorSignUpViewModel()
        {
            StartPicker = -1;
            EndPicker = -1;
            LessonLengthPicker = -1;
            LicenseTypePicker = -1;
            GearboxPicker = -1;
            DrivingSchoolPicker = -1;
            AreaPicker = -1;

            ShowWHourError = false;
            ShowLessonLengthError = false;
            ShowLicenseTypeError = false;
            ShowGearboxError = false;
            ShowDrivingSchoolError = false;
            ShowAreaError = false;

            SliderValue = 0;
            ShowError = false;
        }

        //פעולה הבודקת את תיקנות כל השדות שהזין המשתמש
        private bool ValidateForm()
        {
            //Validate all fields first
            ValidateArea();
            ValidateDrivingSchool();
            ValidateGearbox();
            ValidateLessonLength();
            ValidateLicenseType();
            ValidateWHour();
            ValidateDetails();


            //check if any validation failed
            if (ShowAreaError || ShowDetailError || ShowDrivingSchoolError || ShowWHourError || ShowGearboxError || ShowLessonLengthError || ShowLicenseTypeError)
                return false;
            return true;
        }

        public Command SignUpCommand => new Command(SignUpAsInstructor);
        //פעולה זו מוסיפה את פרטי המורה החדש למסד הנתונים
        public async void SignUpAsInstructor()
        {
            App app = (App)App.Current;
            //בדיקה שהטופס תקין
            if (ValidateForm())
            {
                //בניית אובייקט של מורה חדש לפי הערכים שהזין
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
                    RateId = 0,
                    StartTime = StartHour.Whour,
                    EndTime = EndHour.Whour,
                    DrivingSchoolId = DrivingSchool.SchoolId,
                    RegistrationDate = DateTime.Today,
                    Details = instructorDetails,
                    EStatusId = STATUS_ID
                };

                LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                //הוספת המורה החדש למסד הנתונים
                Instructor instructor = await proxy.InstructorSignUpAsync(i);

                if (instructor != null)
                {
                    //שמירת תמונת הפרופיל של המורה בשרת
                    if (app.TempUser.UserImg != null)
                    {
                        bool success = await proxy.UploadImage(new Models.FileInfo()
                        {
                            Name = this.imageFileResult.FullPath
                        }, $"Instructors\\{instructor.InstructorId}.jpg");
                    }

                    //בניית אובייקט חדש של בקשת רישום
                    EnrollmentRequest er = new EnrollmentRequest
                    {
                        InstructorId = instructor.InstructorId,
                        StatusId = STATUS_ID,
                        SchoolId = instructor.DrivingSchoolId
                    };
                    //הוספת בקשת הרישום למסד הנתונים
                    EnrollmentRequest newEm = await proxy.AddEnrollmentRequest(er);

                    await App.Current.MainPage.DisplayAlert("", "ההרשמה בוצעה בהצלחה! לפני השימוש באפליקציה, עלייך לחכות שמנהל בית הספר יאשר אותך", "בסדר");
                    app.MainPage = new NavigationPage(new OpenningPageView());
                }
                else
                    await App.Current.MainPage.DisplayAlert("שגיאה", "אירעה שגיאה! לא ניתן להמשיך בהרשמה", "בסדר");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("שגיאה", "ההרשמה נכשלה! בדוק את הפרטים שהזנת ונסה שנית", "בסדר");
            }
        }
    }
}

