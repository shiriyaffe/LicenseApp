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

        private bool search;
        public bool Search 
        {
            get
            {
                return this.search;
            }
            set
            {
                if (this.search != value)
                {

                    this.search = value;
                    OnPropertyChanged("Search");
                }
            }
        }

        private string starsRating;
        public string StarsRating
        {
            get
            {
                return this.starsRating;
            }
            set
            {
                if (this.starsRating != value)
                {

                    this.starsRating = value;
                    OnPropertyChanged("StarsRating");
                }
            }
        }

        private bool showNavigationBar;
        public bool ShowNavigationBar 
        {
            get => showNavigationBar;
            set
            {
                showNavigationBar = value;
                OnPropertyChanged("ShowNavigationBar");
            }
        }

        public HomePageViewModel()
        {
            InstructorList = new ObservableCollection<Instructor>();
        }

        public async void CreateInstructorCollection()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            ObservableCollection<Instructor> instructors = await proxy.GetAllInstructorsAsync();
            foreach (Instructor i in instructors)
            {
                this.InstructorList.Add(i);
            }
        }

        public ICommand SelctionChanged => new Command<Object>(OnSelectionChanged);
        public async void OnSelectionChanged(Object obj)
        {
            if (obj is Instructor)
            {
                LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                Instructor chosenInstructor = (Instructor)obj;

                ShowInstructorViewModel instructorContext = new ShowInstructorViewModel
                {
                    IName = chosenInstructor.Iname,
                    ImageUrl = $"defaultPhoto.png",
                    Details = chosenInstructor.Details,
                    PhoneNum = chosenInstructor.PhoneNumber,
                    TeachingArea = await proxy.GetAreaName(chosenInstructor.AreaId),
                    WorkingTime = $"{chosenInstructor.StartTime}-{chosenInstructor.EndTime}",
                    Price = chosenInstructor.Price
                };

                App app = (App)App.Current;
                if(app.CurrentUser != null)
                {
                    Page p = new ShowInstructorView(instructorContext);
                    await App.Current.MainPage.Navigation.PushAsync(p);
                }
                else
                    await App.Current.MainPage.DisplayAlert("שגיאה", "יש להתחבר למערכת כדי לגשת לפרטים נוספים של מורה", "בסדר");
            }
        }

        public ICommand SearchPageCommand => new Command(OpenSearchPage);

        public async void OpenSearchPage()
        {
            App app = (App)App.Current;
            Page p = new SearchPageView();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
