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
    public partial class ListOfInstructorsView : ContentPage
    {
        public ListOfInstructorsView()
        {
            this.BindingContext = new LstOfInstructorsViewModel();
            InitializeComponent();
        }

        private void collectionName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            collectionName.SelectedItem = null;
        }
    }
}