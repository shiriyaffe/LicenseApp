using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using LicenseApp.Models;
using LicenseApp.Views;

namespace LicenseApp.ViewModels
{
    class SignUpAsViewModel
    {
        public Command SignUpAsCommand => new Command<int>(NextPage);

        public void NextPage(int index)
        {
            App app = (App)App.Current;

            if (index == 1)
            {
                Page p = new StudentSignUpView();
                app.MainPage.Navigation.PushAsync(p);
            }
            if (index == 2)
            {
                Page p = new StudentSignUpView();
                app.MainPage.Navigation.PushAsync(p);
            }
            if (index == 3)
            {
                Page p = new StudentSignUpView();
                app.MainPage.Navigation.PushAsync(p);
            }
        }
    }
}
