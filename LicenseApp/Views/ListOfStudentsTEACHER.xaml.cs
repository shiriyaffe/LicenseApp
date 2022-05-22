using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseApp.ViewModels;
using LicenseApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace LicenseApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListOfStudentsTEACHER : ContentPage
    {
        public ListOfStudentsTEACHER()
        {
            LicenseApp.ViewModels.ListOfStudentsTEACHER listOfStudents = new LicenseApp.ViewModels.ListOfStudentsTEACHER();
            listOfStudents.CreateStudentCollection();
            this.BindingContext = listOfStudents;
            InitializeComponent();
        }

        public ListOfStudentsTEACHER(ObservableCollection<Student> students)
        {
            LicenseApp.ViewModels.ListOfStudentsTEACHER listOfStudents = new LicenseApp.ViewModels.ListOfStudentsTEACHER();
            listOfStudents.StudentList = students;
            this.BindingContext = listOfStudents;
            InitializeComponent();
        }

        private void collectionName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            collectionName.SelectedItem = null;
        }
    }
}