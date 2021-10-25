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
    public partial class LogIn : ContentPage
    {
        public LogIn()
        {
            this.BindingContext = new LogInViewModels();
            InitializeComponent();
        }
    }
}