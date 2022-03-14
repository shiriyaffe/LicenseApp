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

        public ManagerSignInViewModel()
        {
            ShowError = false;
            this.ShowNameError = false;
            this.ShowNumberError = false;
        }

        private bool ValidateForm()
        {
            //Validate all fields first
            ValidateName();
            ValidateNumber();


            //check if any validation failed
            if (ShowNameError || ShowNumberError)
                return false;
            return true;
        }

        public Command SignUpCommand => new Command(SignInAsManager);

        public async void SignInAsManager()
        {
            App app = (App)App.Current;
            if (ValidateForm())
            {
                DrivingSchool d = new DrivingSchool
                {
                    SchoolName = SchoolName,
                    NumOfTeachers = NumOfTeachers,
                    EstablishmentYear = EYear,
                    AreaId = Area.AreaId
                };

                LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                DrivingSchool dSchool = await proxy.AddDrivingSchool(d);

                if (dSchool != null)
                {
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
                    SchoolManager schoolM = await proxy.SchoolManagerSignUpAsync(sm);
                    if (schoolM != null)
                    {
                        if (app.TempUser.UserImg != null)
                        {
                            bool success = await proxy.UploadImage(new FileInfo()
                            {
                                Name = app.TempUser.UserImg
                            }, $"SchoolManagers\\{schoolM.SmanagerId}.jpg");
                        }

                        app.CurrentUser = sm;
                        app.MainPage = new NavigationPage(new SchoolManagerMainTabView());
                    }
                }
                else
                {
                    //SubmitError = "ההרשמה נכשלה! נסה שנית";
                    //ShowError = true;
                    await App.Current.MainPage.DisplayAlert("שגיאה", "ההרשמה נכשלה! בדוק את הפרטים שהזנת ונסה שנית", "בסדר");
                }
            }
            else
            {
                //SubmitError = "אירעה שגיאה! לא ניתן להמשיך בהרשמה";
                //ShowError = true;
                await App.Current.MainPage.DisplayAlert("שגיאה", "אירעה שגיאה! לא ניתן להמשיך בהרשמה", "בסדר");
            }
        }
    }
}
