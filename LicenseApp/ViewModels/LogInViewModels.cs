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

namespace LicenseApp.ViewModels
{
    class LogInViewModels : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

        public LogInViewModels()
        {
            ErrorMessage = "";
        }

        public ICommand LogInCommand => new Command(OnSubmit);

        public async void OnSubmit()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            Object user = await proxy.LoginAsync(Email, Password);
            if (user == null)
            {
                ErrorMessage = "התחברות נכשלה, בדוק שם משתמש וסיסמה ונסה שוב";
            }
            else
            {
                App theApp = (App)App.Current;
                theApp.CurrentUser = user;

                //ErrorMessage = "התחברות התקיימה בהצלחה!";
                Page p = new HomePageView();
                App.Current.MainPage.Navigation.PushAsync(p);
            }
        }
    }
}
