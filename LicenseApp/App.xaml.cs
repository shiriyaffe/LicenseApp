using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LicenseApp.Views;

namespace LicenseApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new LogIn();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static bool IsDevEnv
        {
            get
            {
                return true;
            }
        }
    }
}
