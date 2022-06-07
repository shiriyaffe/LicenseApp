using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LicenseApp.Services;

namespace LicenseApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SManagerProfileView : ContentPage
    {
        public SManagerProfileView()
        {
            SManagerProfileViewModel smp = new SManagerProfileViewModel();
            smp.SetImageSourceEvent += Smp_SetImageSourceEvent;
            this.BindingContext = smp;
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

        private void Smp_SetImageSourceEvent(ImageSource obj)
        {
            theImage.Source = obj;
        }
    }
}