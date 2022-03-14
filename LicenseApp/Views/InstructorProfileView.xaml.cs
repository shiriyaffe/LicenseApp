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
    public partial class InstructorProfileView : ContentPage
    {
        public InstructorProfileView()
        {
            InstructorProfileViewModel ip = new InstructorProfileViewModel();
            ip.SetImageSourceEvent += Ip_SetImageSourceEvent;
            this.BindingContext = ip;
            InitializeComponent();
        }

        private void logout_Clicked(object sender, EventArgs e)
        {
            App app = (App)App.Current;
            app.CurrentUser = null;
            Page p = new OpenningPageView();
            app.MainPage = new NavigationPage(p);
        }

        private void Ip_SetImageSourceEvent(ImageSource obj)
        {
            theImage.Source = obj;
        }
    }
}