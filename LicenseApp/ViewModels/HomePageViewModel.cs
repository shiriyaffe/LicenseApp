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

        private const int APPROVED_STATUS = 2;

        //רשימת מורים מסוננת
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

        //מגדיר האם בוצע חיפוש
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

        //תכונה המגדירה האם יהיה סרגל בראש העמוד
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

        //פעולה הממלאת את רשימת המורים בעקכים
        public async void CreateInstructorCollection()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            //קריאת נתוני המורים הרשומים לאפליקציה
            ObservableCollection<Instructor> instructors = await proxy.GetAllInstructorsAsync();
            foreach (Instructor i in instructors)
            {
                //בדיקה שרישום המורה אושר על ידי מנהל ומשויך לבית ספר
                if(i.EStatusId == APPROVED_STATUS)
                    this.InstructorList.Add(i);
            }
        }

        public ICommand SelctionChanged => new Command<Object>(OnSelectionChanged);
        //פעולה הפועלת בעת לחיצה על מורה מהרשימה ומציגה פרטים מורחבים עליו בפני המשתמש
        public async void OnSelectionChanged(Object obj)
        {
            if (obj is Instructor)
            {
                LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                Instructor chosenInstructor = (Instructor)obj;
                //העברת נתוני המורה הנבחר למסך הבא
                ShowInstructorViewModel instructorContext = new ShowInstructorViewModel
                {
                    IName = chosenInstructor.Iname,
                    ImageUrl = chosenInstructor.PhotoURI,
                    Details = chosenInstructor.Details,
                    PhoneNum = chosenInstructor.PhoneNumber,
                    TeachingArea = (await proxy.GetAreaById(chosenInstructor.AreaId)).AreaName,
                    WorkingTime = $"{chosenInstructor.StartTime}-{chosenInstructor.EndTime}",
                    Price = chosenInstructor.Price,
                    InstructorID = chosenInstructor.InstructorId
                };

                App app = (App)App.Current;
                //בדיקה האם המשתמש מחובר לאפליקציה
                if(app.CurrentUser != null)
                {
                    //במידה וכן, הצגת פרטי המורה המלאים
                    Page p = new ShowInstructorView(instructorContext);
                    await App.Current.MainPage.Navigation.PushAsync(p);
                }
                else
                    //במידה ולא, הצגת הודעה מתאימה בפני המשתמש
                    await App.Current.MainPage.DisplayAlert("שגיאה", "יש להתחבר למערכת כדי לגשת לפרטים נוספים של מורה", "בסדר");
            }
        }

        public ICommand SearchPageCommand => new Command(OpenSearchPage);
        //פעולה זו מופעלת בעת לחיצה על אייקון החיפוש ומציגה את מסך החיפוש
        public async void OpenSearchPage()
        {
            App app = (App)App.Current;
            Page p = new SearchPageView();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
