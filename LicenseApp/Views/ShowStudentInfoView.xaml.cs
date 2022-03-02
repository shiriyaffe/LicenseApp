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
    public partial class ShowStudentInfoView : ContentPage
    {
        public ShowStudentInfoView(ShowStudentInfoViewModel studentContext)
        {
            this.BindingContext = studentContext;
            this.Title = studentContext.SName;
            InitializeComponent();
        }
    }
}