using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LicenseApp.ViewModels;

namespace LicenseApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpView : ContentPage
    {
        public SignUpView()
        {
            SignUpViewModel su = new SignUpViewModel();
            su.SetImageSourceEvent += Su_SetImageSourceEvent;
            this.BindingContext = su;
            InitializeComponent();
        }

        private void Su_SetImageSourceEvent(ImageSource obj)
        {
            theImage.Source = obj;
        }
    }
}