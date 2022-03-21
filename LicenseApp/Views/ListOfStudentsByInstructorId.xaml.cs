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
    public partial class ListOfStudentsByInstructorId : ContentPage
    {
        public ListOfStudentsByInstructorId(int instructorId)
        {
            ListOfStudentsByInstructorIdViewModel lst = new ListOfStudentsByInstructorIdViewModel();
            lst.CreateStudentCollection(instructorId);
            this.BindingContext = lst;
            InitializeComponent();
        }
    }
}