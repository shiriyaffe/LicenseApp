using LicenseApp.Models;
using LicenseApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace LicenseApp.ViewModels
{
    public class SManagerProfileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        const string ERROR_PIC = "Error.png";
        const string Correct_PIC = "Correct.png";

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
            }
            else
                this.PassError = Correct_PIC;
        }

        private string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
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
                ValidateNumber();
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

        public SManagerProfileViewModel()
        {
            App app = (App)App.Current;
            if (app.CurrentUser != null && app.CurrentUser is SchoolManager)
            {
                SchoolManager sm = (SchoolManager)app.CurrentUser;
                Pass = sm.Pass;
                PhoneNumber = sm.PhoneNumber;
                ImageUrl = "defaultPhoto.png";
                ShowNumberError = false;
                this.SaveDataCommand = new Command(() => SaveData());
            }
        }

        public Command SaveDataCommand { protected set; get; }

        private bool ValidateForm()
        {
            //Validate all fields first
            ValidatePass();
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
                SchoolManager newSManager = new SchoolManager
                {
                    Pass = this.Pass,
                    PhoneNumber = this.PhoneNumber,
                    SmanagerId = ((SchoolManager)theApp.CurrentUser).SmanagerId,
                    Smname = ((SchoolManager)theApp.CurrentUser).Smname,
                    Email = ((SchoolManager)theApp.CurrentUser).Email,
                    GenderId = ((SchoolManager)theApp.CurrentUser).GenderId,
                    Birthday = ((SchoolManager)theApp.CurrentUser).Birthday,
                    SchoolId = ((SchoolManager)theApp.CurrentUser).SchoolId,
                    RegistrationDate = ((SchoolManager)theApp.CurrentUser).RegistrationDate
                };

                LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                SchoolManager sManager = await proxy.UpdateSManager(newSManager);

                if (sManager == null)
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

                    theApp.CurrentUser = sManager;
                    await App.Current.MainPage.DisplayAlert("עדכון", "העדכון בוצע בהצלחה", "אישור", FlowDirection.RightToLeft);
                }
            }
            else
                await App.Current.MainPage.DisplayAlert("שמירת נתונים", " יש בעיה עם הנתונים בדוק ונסה שוב", "אישור", FlowDirection.RightToLeft);
        }
    }
}
