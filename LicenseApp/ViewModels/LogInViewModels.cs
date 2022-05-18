using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Essentials;
using System.ComponentModel;
using LicenseApp.Services;
using LicenseApp.Views;
using LicenseApp.Models;

namespace LicenseApp.ViewModels
{
    class LogInViewModels : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private const string OPENEYE_PHOTO_SRC = "openEye.png";
        private const string CLOSEDEYE_PHOTO_SRC = "closedEye.png";

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        private bool showPass;
        public bool ShowPass
        {
            get { return showPass; }

            set
            {
                if (this.showPass != value)
                {
                    this.showPass = value;
                    OnPropertyChanged(nameof(ShowPass));
                }
            }
        }

        private string imgSource;
        public string ImgSource
        {
            get => imgSource;
            set
            {
                imgSource = value;
                OnPropertyChanged("ImgSource");
            }
        }

        public LogInViewModels()
        {
            ErrorMessage = "";
            ShowPass = true;
            imgSource = OPENEYE_PHOTO_SRC;
            PassCommand = new Command(OnShowPass);
        }

        public ICommand PassCommand { protected set; get; }

        public void OnShowPass()
        {
            if (ShowPass == false)
            { ShowPass = true; }

            else { ShowPass = false; }

            if (ImgSource == CLOSEDEYE_PHOTO_SRC)
            { ImgSource = OPENEYE_PHOTO_SRC; }

            else { ImgSource = CLOSEDEYE_PHOTO_SRC; }
        }

        public ICommand LogInCommand => new Command(OnSubmit);

        public async void OnSubmit()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            Object user = await proxy.LoginAsync(Email, Password);
            if (user == null)
            {
                await App.Current.MainPage.DisplayAlert("שגיאה", "התחברות נכשלה, בדוק שם משתמש וסיסמה ונסה שוב", "בסדר");
            }
            else
            {
                App theApp = (App)App.Current;
                theApp.CurrentUser = user;

                if (user is Student)
                {
                    App.Current.MainPage = new NavigationPage(new StudentMainTabView());
                }
                else if (user is Instructor)
                {
                    if (((Instructor)user).EStatusId == 1)
                        await App.Current.MainPage.DisplayAlert("שגיאה", "מנהל בית הספר עדיין לא אישר אותך... נסה במועד מאוחר יותר", "בסדר");
                    else if(((Instructor)user).EStatusId == 3)
                    {
                        await App.Current.MainPage.DisplayAlert("שגיאה", "לצערנו, מנהל בית הספר לא אישר אותך...", "בסדר");
                        App.Current.MainPage = new NavigationPage(new DeniedInstructorTabPage());
                    }  
                    else
                        App.Current.MainPage = new NavigationPage(new InstructorMainTabView());
                }
                else if (user is SchoolManager)
                {
                    if (((SchoolManager)user).EStatusId == 1)
                        await App.Current.MainPage.DisplayAlert("שגיאה", "מנהל האפליקציה עדיין לא אישר אותך... נסה במועד מאוחר יותר", "בסדר");
                    else if (((SchoolManager)user).EStatusId == 3)
                        await App.Current.MainPage.DisplayAlert("שגיאה", "מנהל האפליקציה לא אישר אותך...", "בסדר");
                    else
                        App.Current.MainPage = new NavigationPage(new SchoolManagerMainTabView());
                }
            }
        }
    }
}
