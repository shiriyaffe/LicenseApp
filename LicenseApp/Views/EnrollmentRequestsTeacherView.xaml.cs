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
    public partial class EnrollmentRequestsTeacherView : ContentPage
    {
        public EnrollmentRequestsTeacherView()
        {
            this.BindingContext = new EnrollmentRequestsTeacherViewModel();
            InitializeComponent();
        }
    }
}