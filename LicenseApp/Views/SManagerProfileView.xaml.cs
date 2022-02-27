using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LicenseApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SManagerProfileView : ContentPage
    {
        public SManagerProfileView()
        {
            this.BindingContext = new SManagerProfileViewModel();
            InitializeComponent();
        }

        private void logout_Clicked(object sender, EventArgs e)
        {
            App app = (App)App.Current;
            app.CurrentUser = null;
            Page p = new OpenningPageView();
            app.MainPage = new NavigationPage(p);
        }
    }
}