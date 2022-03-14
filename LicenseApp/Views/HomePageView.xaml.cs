using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel;
using System.Collections.ObjectModel;
using LicenseApp.Models;

namespace LicenseApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePageView : ContentPage
    {
        public HomePageView()
        {
            HomePageViewModel hpvm = new HomePageViewModel();
            hpvm.CreateInstructorCollection();
            hpvm.ShowNavigationBar = false;
            this.BindingContext = hpvm;
            InitializeComponent();
        }

        public HomePageView(ObservableCollection<Instructor> instructors)
        {
            HomePageViewModel hpvm = new HomePageViewModel();
            hpvm.InstructorList = instructors;
            hpvm.Search = true;
            hpvm.ShowNavigationBar = true;
            this.BindingContext = hpvm;
            InitializeComponent();
        }
    }
}