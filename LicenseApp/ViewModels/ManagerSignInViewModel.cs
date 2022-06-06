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
    public class ManagerSignInViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        const int STATUS_ID = 4;

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

        //פעולה הבודקת את תקינות האיזור שנבחר
        public void ValidateArea()
        {
            this.ShowAreaError = AreaPicker == -1;
            if (this.ShowAreaError)
            {
                this.AreaError = "אזור לימוד הוא שדה!";
            }
            else
                this.AreaError = null;
        }
        #endregion

        #region year
        public List<int> Years
        {
            get
            {
                return ((App)App.Current).ListOfYears;
            }
        }

        private int eYear;
        public int EYear
        {
            get { return eYear; }
            set
            {
                eYear = value;
                OnPropertyChanged("EYear");
            }
        }

        public int eYearPicker;
        public int EYearPicker
        {
            get { return eYearPicker; }
            set
            {
                eYearPicker = value;
                ValidateEYear();
                OnPropertyChanged("EYearPicker");
            }
        }

        private string eYearError;

        public string EYearError
        {
            get => eYearError;
            set
            {
                eYearError = value;
                OnPropertyChanged("EYearError");
            }
        }

        private bool showEYearError;

        public bool ShowEYearError
        {
            get => showEYearError;
            set
            {
                showEYearError = value;
                OnPropertyChanged("ShowEYearError");
            }
        }

        //פעולה הבודקת את תקינות השנה שנבחרה
        public void ValidateEYear()
        {
            this.ShowEYearError = EYearPicker == -1;
            if (this.ShowEYearError)
            {
                this.EYearError = "שנת הקמה הוא שדה חובה!";
            }
            else
                this.EYearError = null;
        }
        #endregion

        #region שם בית ספר
        private string schoolName;
        public string SchoolName
        {
            get { return schoolName; }
            set
            {
                schoolName = value;
                ValidateName();
                OnPropertyChanged("SchoolName");
            }
        }

        private bool showNameError;

        public bool ShowNameError
        {
            get => showNameError;
            set
            {
                showNameError = value;
                OnPropertyChanged("ShowNameError");
            }
        }

        private string nameError;

        public string NameError
        {
            get => nameError;
            set
            {
                nameError = value;
                OnPropertyChanged("NameError");
            }
        }

        //פעולה הבודקת את תקינות שם בית הספר שהזין המשתמש
        private void ValidateName()
        {
            this.ShowNameError = string.IsNullOrEmpty(SchoolName);
            if (ShowNameError)
                NameError = "השם אינו תקין";
            else
                NameError = null;
        }
        #endregion

        #region מספר מורים
        private int numOfTeachers;
        public int NumOfTeachers
        {
            get { return numOfTeachers; }
            set
            {
                numOfTeachers = value;
                ValidateNumber();
                OnPropertyChanged("NumOfTeachers");
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

        private void ValidateNumber()
        {
            string num = NumOfTeachers.ToString();
            this.ShowNumberError = (string.IsNullOrEmpty(num) || !Regex.IsMatch(num, @"^[0-9]*$"));
            if (ShowNumberError)
                NumberError = "שדה זה אינו תקין";
            else
                NumberError = null;
        }
        #endregion


        public ManagerSignInViewModel()
        {
            AreaPicker = -1;
            EYear = -1;

            this.ShowNameError = false;
            this.ShowNumberError = false;
            this.ShowAreaError = false;
            this.ShowEYearError = false;
        }

        //פעולה הבודקת את תקינות השדות בטופס,
        private bool ValidateForm()
        {
            //Validate all fields first
            ValidateName();
            ValidateNumber();
            ValidateArea();
            ValidateEYear();


            //check if any validation failed
            if (ShowNameError || ShowNumberError || ShowEYearError || ShowAreaError)
                return false;
            return true;
        }

        public Command SignUpCommand => new Command(SignUpAsManager);
        //פעולה השומרת את פרטי המנהל החדש ובית ספרו במסד הנתונים ומגדירה את המנהל כשמתמש המחובר
        public async void SignUpAsManager()
        {
            App app = (App)App.Current;
            //בדיקת תקינות הטופס
            if (ValidateForm())
            {
                //בניית אובייקט חדש של בית ספר
                DrivingSchool d = new DrivingSchool
                {
                    SchoolName = SchoolName,
                    NumOfTeachers = NumOfTeachers,
                    EstablishmentYear = EYear,
                    AreaId = Area.AreaId
                };

                //הוספת בית הספר החדש למסד הנתונים
                LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                DrivingSchool dSchool = await proxy.AddDrivingSchool(d);

                if (dSchool != null)
                {
                    //בניית אובייקט חדש של מנהל
                    SchoolManager sm = new SchoolManager
                    {
                        Smname = app.TempUser.Name,
                        Email = app.TempUser.Email,
                        Pass = app.TempUser.UserPswd,
                        PhoneNumber = app.TempUser.PhoneNumber,
                        Birthday = app.TempUser.BirthDate,
                        GenderId = app.TempUser.Gender.GenderId,
                        SchoolId = dSchool.SchoolId,
                        RegistrationDate = DateTime.Today
                    };

                    //הוספת המנהל למסד הנתונים
                    SchoolManager schoolM = await proxy.SchoolManagerSignUpAsync(sm);
                    if (schoolM != null)
                    {
                        //שמירת תמונת הפרופיל של המנהל בשרת
                        if (app.TempUser.UserImg != null)
                        {
                            bool success = await proxy.UploadImage(new FileInfo()
                            {
                                Name = app.TempUser.UserImg
                            }, $"SchoolManagers\\{schoolM.SmanagerId}.jpg");
                        }

                        //חיבור המנהל לאפליקציה
                        app.CurrentUser = await proxy.LoginAsync(sm.Email, sm.Pass);
                        if(app.CurrentUser != null)
                            app.MainPage = new NavigationPage(new SchoolManagerMainTabView());
                    }
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
