using LicenseApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LicenseApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AvailableLessonsView : ContentPage
    {
        public AvailableLessonsView()
        {
            this.BindingContext = new AvailableLessonsViewModel();
            InitializeComponent();
        }
    }
}