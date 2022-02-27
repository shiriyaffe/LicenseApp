using LicenseApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LicenseApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentProfileView : ContentPage
    {
        public StudentProfileView()
        {
            this.BindingContext = new StudentProfileViewModel();
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