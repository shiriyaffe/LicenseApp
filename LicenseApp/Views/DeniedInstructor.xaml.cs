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
    public partial class DeniedInstructor : ContentPage
    {
        public DeniedInstructor()
        {
            this.BindingContext = new DeniedInstructorViewModel();
            InitializeComponent();
        }
    }
}