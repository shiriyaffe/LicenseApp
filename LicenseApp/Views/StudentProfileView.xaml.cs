using LicenseApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LicenseApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentProfileView : ContentPage
    {
        public StudentProfileView()
        {
            StudentProfileViewModel sp = new StudentProfileViewModel();
            sp.SetImageSourceEvent += Sp_SetImageSourceEvent;
            this.BindingContext = sp;
            InitializeComponent();
        }

        private async void logout_Clicked(object sender, EventArgs e)
        {
            App app = (App)App.Current;
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            bool loggedOut = await proxy.Logout();
            if (loggedOut)
            {
                app.CurrentUser = null;
                Page p = new OpenningPageView();
                app.MainPage = new NavigationPage(p);
            }
            else
                await App.Current.MainPage.DisplayAlert("שגיאה", "ההתנתקות נכשלה!", "בסדר");
        }

        private void Sp_SetImageSourceEvent(ImageSource obj)
        {
            theImage.Source = obj;
        }
    }
}