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
    public partial class OpenningPageView : ContentPage
    {
        public OpenningPageView()
        {
            this.BindingContext = new SearchPageViewModel();
            InitializeComponent();
        }

        private void signin_Clicked(object sender, EventArgs e)
        {
            App app = (App)App.Current;
            Page p = new SignUpView();
            p.Title = "הרשמה";
            app.MainPage.Navigation.PushAsync(p);
        }

        private void login_Clicked(object sender, EventArgs e)
        {
            App app = (App)App.Current;
            Page p = new LogIn();
            p.Title = "התחברות";
            app.MainPage.Navigation.PushAsync(p);
        }
    }
}