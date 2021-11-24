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
    public partial class SignUpAsView : ContentPage
    {
        public SignUpAsView()
        {
            this.BindingContext = new SignUpAsViewModel();
            InitializeComponent();
        }

        private void Manager_Clicked(object sender, EventArgs e)
        {
            App app = (App)App.Current;
            Page p = new ManagerSignUpView();
            app.MainPage.Navigation.PushAsync(p);
        }

        private void Instructor_Clicked(object sender, EventArgs e)
        {
            App app = (App)App.Current;
            Page p = new InstructorSignUpView();
            app.MainPage.Navigation.PushAsync(p);
        }

        private void Student_Clicked(object sender, EventArgs e)
        {
            App app = (App)App.Current;
            Page p = new StudentSignUpView();
            app.MainPage.Navigation.PushAsync(p);
        }
    }
}