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
    }
}