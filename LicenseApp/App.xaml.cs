﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LicenseApp.Views;
using LicenseApp.Models;
using LicenseApp.Services;
using System.Collections.Generic;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;
using Application = Xamarin.Forms.Application;

namespace LicenseApp
{
    public partial class App : Application
    {
        public Object CurrentUser { get; set; }
        public User TempUser { get; set; }
        public LookupTables Tables { get; set; }
        public List<int> ListOfYears { get; set; }
        
        public event Action RefreshUI;

        public App()
        {
            InitializeComponent();
            Application.Current.On<Windows>().SetImageDirectory("Assets");
            FillListOfYears();
            CurrentUser = null;
            TempUser = null;
            MainPage = new Loading();
        }

        private void FillListOfYears()
        {
            ListOfYears = new List<int>();

            for (int i = 1900; i <= DateTime.Today.Year; i++)
            {
                ListOfYears.Add(i);
            }
        }

        protected async override void OnStart()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            Tables = await proxy.GetLookups();
            MainPage = new NavigationPage(new OpenningPageView());
        }

        public void UIRefresh() { this.RefreshUI?.Invoke(); }

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
