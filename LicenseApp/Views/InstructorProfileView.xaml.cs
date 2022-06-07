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
    public partial class InstructorProfileView : ContentPage
    {
        public InstructorProfileView()
        {
            InstructorProfileViewModel ip = new InstructorProfileViewModel();
            ip.SetImageSourceEvent += Ip_SetImageSourceEvent;
            this.BindingContext = ip;
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

        private void Ip_SetImageSourceEvent(ImageSource obj)
        {
            theImage.Source = obj;
        }
    }
}