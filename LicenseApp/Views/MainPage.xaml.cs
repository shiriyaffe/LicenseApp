using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using LicenseApp.Services;
using LicenseApp.ViewModels;


namespace LicenseApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            this.BindingContext = new HelloViewModels();
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

    }
}
