using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LicenseApp.Views;
using LicenseApp.Models;
using LicenseApp.Services;

namespace LicenseApp
{
    public partial class App : Application
    {
        public Object CurrentUser { get; set; }
        public User TempUser { get; set; }
        public LookupTables Tables { get; set; }

        public App()
        {
            InitializeComponent();
            CurrentUser = null;
            TempUser = null;
            MainPage = new Loading();
        }

        protected async override void OnStart()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            Tables = await proxy.GetLookups();
            Page p = new InstructorSignUpView();
            p.Title = "הרשמה";
            MainPage = new NavigationPage(p);
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
