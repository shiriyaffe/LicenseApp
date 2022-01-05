using LicenseApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Essentials;
using LicenseApp.Models;
using LicenseApp.Views;
using LicenseApp.ViewModels;


namespace LicenseApp.ViewModels
{
    public class HomePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Instructor> instructorList;
        public ObservableCollection<Instructor> InstructorList
        {
            get
            {
                return this.instructorList;
            }
            set
            {
                if (this.instructorList != value)
                {

                    this.instructorList = value;
                    OnPropertyChanged("InstructorList");
                }
            }
        }

        public HomePageViewModel()
        {
            InstructorList = new ObservableCollection<Instructor>();
            CreateInstructorCollection();
        }

        async void CreateInstructorCollection()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            List<Instructor> instructors = await proxy.GetAllInstructorsAsync();
            foreach (Instructor i in instructors)
            {
                this.InstructorList.Add(i);
            }
        }

        public ICommand SelctionChanged => new Command<Object>(OnSelectionChanged);
        public void OnSelectionChanged(Object obj)
        {
            if (obj is Instructor)
            {
                Instructor chosenInstructor = (Instructor)obj;
                ShowInstructorViewModel instructorContext = new ShowInstructorViewModel
                {
                    IName = chosenInstructor.Iname,
                    ImageUrl = chosenInstructor.PhotoURI,
                    Details = chosenInstructor.Details
                };

                App app = (App)App.Current;
                if(app.CurrentUser != null)
                {
                    Page p = new ShowInstructorView();
                    p.BindingContext = instructorContext;
                    p.Title = instructorContext.IName;
                    App.Current.MainPage.Navigation.PushAsync(p);
                }
            }
        }
    }
}
