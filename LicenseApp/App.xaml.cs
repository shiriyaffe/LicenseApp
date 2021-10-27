using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LicenseApp.Views;
using LicenseApp.Models;

namespace LicenseApp
{
    public partial class App : Application
    {
        public Object CurrentUser { get; set; }
        public LookupTables Tables { get; set; }

        public App()
        {
            InitializeComponent();
            CurrentUser = null;
            MainPage = new StudentSignUpView();
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
