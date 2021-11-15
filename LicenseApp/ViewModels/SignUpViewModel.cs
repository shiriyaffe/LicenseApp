using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using LicenseApp.Models;


namespace LicenseApp.ViewModels
{
    public class SignUpViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    
        #region שם
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

        private string name;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                ValidateName();
                OnPropertyChanged("Name");
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
            this.ShowNameError = string.IsNullOrEmpty(Name);
            if (ShowNameError)
                NameError = "השם אינו תקין";
            else
                NameError = null;
        }
        #endregion

        #region אימייל
        private string mail;

        public string Mail
        {
            get => mail;
            set
            {
                mail = value;
                ValidateMail();
                OnPropertyChanged("Mail");
            }
        }

        private string mailError;

        public string MailError
        {
            get => mailError;
            set
            {
                mailError = value;
                OnPropertyChanged("MailError");
            }
        }

        private bool showMailError;

        public bool ShowMailError
        {
            get => showMailError;
            set
            {
                showMailError = value;
                OnPropertyChanged("ShowMailError");
            }
        }
        private void ValidateMail()
        {
            this.ShowMailError = string.IsNullOrEmpty(Mail);
            if (!this.ShowMailError)
            {
                if (!Regex.IsMatch(this.Mail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                {
                    this.ShowMailError = true;
                    this.MailError = "האימייל אינו תקין";
                }
            }
            else
                this.MailError = null;
        }
        #endregion

        #region סיסמה
        private string originalPass;

        public string OriginalPass
        {
            get => originalPass;
            set
            {
                originalPass = value;
                OnPropertyChanged("OriginalPass");
            }
        }

        private string pass;

        public string Pass
        {
            get => pass;
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
            get => PassError;
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
            this.ShowPassError = Pass != OriginalPass;
        }
        #endregion

        #region תאריך
        private DateTime date;
        public DateTime Date
        {
            get => date;
            set
            {
                date = value;
                ValidateDate();
                OnPropertyChanged("Date");
            }
        }

        private string dateError;

        public string DateError
        {
            get => dateError;
            set
            {
                dateError = value;
                OnPropertyChanged("DateError");
            }
        }

        private bool showDateError;

        public bool ShowDateError
        {
            get => showDateError;
            set
            {
                showDateError = value;
                OnPropertyChanged("ShowDateError");
            }
        }
        private void ValidateDate()
        {
            int year = Date.Year;
            int month = Date.Month;
            int day = Date.Day;

            if (year > DateTime.Today.Year - 16)
                this.ShowDateError = true;
            else
            {
                if (year == DateTime.Today.Year - 16)
                {
                    if (month > DateTime.Today.Month)
                        this.ShowDateError = true;
                    else if (month == DateTime.Today.Month)
                    {
                        if (day < DateTime.Today.Day)
                            this.ShowDateError = true;
                    }
                }
            }

            if (ShowDateError)
                this.DateError = "הגיל חייב להיות מעל 16";
            else
                this.DateError = null;
        }
        #endregion

        #region מספר טלפון
        private string number;

        public string PhoneNumber
        {
            get => number;
            set
            {
                number = value;
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
            this.ShowNumberError = string.IsNullOrEmpty(PhoneNumber);
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

        #endregion

        public SignUpViewModel()
        {
            //this.NameError = "השם הוא שדה חובה";
            this.ShowNameError = false;
            //this.MailError = "האימייל אינו תקין";
            this.ShowMailError = false;
            //this.PassError = "אין התאמה בין הסיסמה לאימות";
            this.ShowPassError = false;
            //this.DateError = "הגיל חייב להיות מעל 16";
            this.ShowDateError = false;
            //this.NumberError = "מספר טלפון חייב להיות בעל 10 ספרות";
            this.ShowNumberError = false;
            Date = new DateTime(DateTime.Today.Year - 16, DateTime.Today.Month, DateTime.Today.Day);
            this.SaveDataCommand = new Command(() => SaveData());
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

        private bool ValidateForm()
        {
            //Validate all fields first
            ValidateName();
            ValidateMail();
            ValidatePass();
            ValidateDate();
            ValidateNumber();


            //check if any validation failed
            if (ShowNameError || ShowMailError || ShowDateError || ShowNumberError || ShowPassError)
                return false;
            return true;
        }

        public Command SaveDataCommand { protected set; get; }
        private async void SaveData()
        {
            App app = (App)App.Current;
            if (ValidateForm())
            {
                app.CurrentUser = new User
                {
                    Email = Mail,
                    Name = Name,
                    UserPswd = Pass,
                    PhoneNumber = PhoneNumber,
                    BirthDate = Date
                };
                
                await App.Current.MainPage.DisplayAlert("שמירת נתונים", "הנתונים נבדקו ונשמרו", "אישור", FlowDirection.RightToLeft);
            }
            else
                await App.Current.MainPage.DisplayAlert("שמירת נתונים", "יש בעיה עם הנתונים", "אישור", FlowDirection.RightToLeft);
        }


    }
}

