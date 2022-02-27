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
    public partial class ShowInstructorView : ContentPage
    {
        public ShowInstructorView(ShowInstructorViewModel instructorContext)
        {
            this.BindingContext = instructorContext;
            this.Title = instructorContext.IName;
            InitializeComponent();
        }
    }
}