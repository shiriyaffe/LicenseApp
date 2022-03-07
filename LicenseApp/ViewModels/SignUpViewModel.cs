﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using LicenseApp.Models;
using LicenseApp.Views;
using LicenseApp.Services;
using Xamarin.Essentials;
using System.Windows.Input;

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
            this.ShowConditions = false;

            this.ShowNameError = (string.IsNullOrEmpty(Name) || !Regex.IsMatch(this.Name, @"^[\u0590-\u05FF ]+$"));
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
            this.ShowConditions = false;

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
                ValidatePass();
                OnPropertyChanged("OriginalPass");
            }
        }

        //private string pass;

        //public string Pass
        //{
        //    get => pass;
        //    set
        //    {
        //        pass = value;
        //        ValidatePass();
        //        OnPropertyChanged("Pass");
        //    }
        //}

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
            this.ShowConditions = true;

            this.ShowPassError = string.IsNullOrEmpty(OriginalPass);
            if (!this.ShowPassError)
            {
                if (!Regex.IsMatch(this.OriginalPass, @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"))
                {
                    this.ShowPassError = true;
                    this.PassError = "הסיסמה אינה תקינה";
                }
            }
            else
                this.PassError = null;
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
            this.ShowConditions = false;

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
                        if (day > DateTime.Today.Day)
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
            this.ShowConditions = false;

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

        private string nextError;

        public string NextError
        {
            get => nextError;
            set
            {
                nextError = value;
                OnPropertyChanged("NextError");
            }
        }

        private bool showNextError;

        public bool ShowNextError
        {
            get => showNextError;
            set
            {
                showNextError = value;
                OnPropertyChanged("ShowNextError");
            }
        }

        private bool showConditions;

        public bool ShowConditions
        {
            get => showConditions;
            set
            {
                showConditions = value;
                OnPropertyChanged("ShowConditions");
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

        public SignUpViewModel()
        {
            this.ShowNameError = false;
            this.ShowMailError = false;
            this.ShowPassError = false;
            this.ShowDateError = false;
            this.ShowNumberError = false;
            ShowConditions = false;
            ShowNextError = false;

            this.UserImgSrc = DEFAULT_PHOTO_SRC;
            this.imageFileResult = null;

            Date = new DateTime(DateTime.Today.Year - 16, DateTime.Today.Month, DateTime.Today.Day);
            this.SaveDataCommand = new Command(() => SaveData());
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
            if (ShowNameError || ShowMailError || ShowDateError || ShowNumberError)
                return false;
            return true;
        }

        public Command SaveDataCommand { protected set; get; }
        private async void SaveData()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            App app = (App)App.Current;

            if(imageFileResult != null)
            {
                UserImgSrc = imageFileResult.FullPath;
            }
            else
            {
                UserImgSrc = null;
            }

            if (ValidateForm())
            {
                app.TempUser = new User
                {
                    Email = Mail,
                    Name = Name,
                    UserPswd = OriginalPass,
                    PhoneNumber = PhoneNumber,
                    BirthDate = Date,
                    Gender = Gender,
                    UserImg = UserImgSrc
                };

                Page p = new SignUpAsView();
                p.Title = "הרשם בתור";
                app.MainPage.Navigation.PushAsync(p);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("שגיאה", "אירעה שגיאה! לא ניתן להמשיך בהרשמה", "בסדר");
                //NextError = "אירעה שגיאה! לא ניתן להמשיך בהרשמה";
                //ShowNextError = true;
            }
                
        }


    }
}

