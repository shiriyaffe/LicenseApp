using LicenseApp.Models;
using LicenseApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using LicenseApp.Views;
using System.Linq;

namespace LicenseApp.ViewModels
{
    public class LstOfInstructorsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private const int APPROVED_STATUS = 2;

        //רשימת המורים המשויכים לבית הספר שבבעלתו של המנהל המחובר
        private ObservableCollection<Instructor> filteredInstructorList;
        public ObservableCollection<Instructor> FilteredInstructorList
        {
            get
            {
                return this.filteredInstructorList;
            }
            set
            {
                if (this.filteredInstructorList != value)
                {

                    this.filteredInstructorList = value;
                    OnPropertyChanged("FilteredInstructorList");
                }
            }
        }

        private ObservableCollection<Instructor> AllInstructors;

        //מספר המורים העובדים תחת המנהל המחובר
        private int instructorsCount;
        public int InstructorsCount
        {
            get
            {
                return this.instructorsCount;
            }
            set
            {
                if (this.instructorsCount != value)
                {

                    this.instructorsCount = value;
                    OnPropertyChanged("InstructorsCount");
                }
            }
        }

        public LstOfInstructorsViewModel()
        {
            this.AllInstructors = new ObservableCollection<Instructor>();
            App theApp = (App)Application.Current;
            theApp.RefreshUI += TheApp_RefreshUI;
            CreateInstructorCollection();
        }

        //פעולה המרעננת את המסך
        private void TheApp_RefreshUI()
        {
            CreateInstructorCollection();
        }

        //פעולה הממלאת את רשימת המורים בהתאם למנהל המחובר
        public async void CreateInstructorCollection()
        {
            App app = (App)App.Current;
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();

            //קריאת נתוניהם של כל המורים הרשומים באפליקציה
            ObservableCollection<Instructor> instructors = await proxy.GetAllInstructorsAsync();
            AllInstructors.Clear();
            foreach (Instructor i in instructors)
            {
                //הכנסה לרישמה של מורים המשויכים למנהל המחובר ושאושרו על ידו
                if (i.SchoolManagerId == ((SchoolManager)app.CurrentUser).SmanagerId && i.EStatusId == APPROVED_STATUS)
                    AllInstructors.Add(i);
            }

            InstructorsCount = AllInstructors.Count;
            FilteredInstructorList = new ObservableCollection<Instructor>(this.AllInstructors.OrderBy(i => i.Iname));
            this.SearchTerm = string.Empty;
        }

        public ICommand SelctionChanged => new Command<Object>(OnSelectionChanged);
        //פעולה המופעלת בעת בחירת מורה מסוים מהרשימה ומציגה למנהל את פרטיו המלאים של מורה זה
        public async void OnSelectionChanged(Object obj)
        {
            if (obj is Instructor)
            {
                LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                Instructor chosenInstructor = (Instructor)obj;

                //העברת נתוני המורה הנבחר למסך הבא
                ShowInstrucorSMViewModel instructorContext = new ShowInstrucorSMViewModel
                {
                    IName = chosenInstructor.Iname,
                    ImageUrl = chosenInstructor.PhotoURI,
                    Email = chosenInstructor.Email,
                    PhoneNum = chosenInstructor.PhoneNumber,
                    LessonLength = chosenInstructor.LessonLengthId,
                    Price = chosenInstructor.Price,
                    InstructorID = chosenInstructor.InstructorId,
                    SLength = (await proxy.GetLessonLengthById(chosenInstructor.LessonLengthId)).Slength,
                    Rating = (int)chosenInstructor.RateId
                };

                    Page p = new ShowInstructorSMView(instructorContext);
                    await App.Current.MainPage.Navigation.PushAsync(p);
            }
        }

        //שדה החיפוש שהזין המנהל המחובר
        private string searchTerm;
        public string SearchTerm
        {
            get
            {
                return this.searchTerm;
            }
            set
            {
                if (this.searchTerm != value)
                {
                    this.searchTerm = value;
                    OnTextChanged(value);
                    OnPropertyChanged("SearchTerm");
                }
            }
        }

        //סינון רשימת המורים לפי שדה החיפוש שהזין המנהל
        public void OnTextChanged(string search)
        {
            App app = (App)App.Current;
            //Filter the list of contacts based on the search term
            if (this.AllInstructors == null)
                return;
            if (String.IsNullOrWhiteSpace(search) || String.IsNullOrEmpty(search))
            {
                foreach (Instructor i in this.AllInstructors)
                {
                    if (!this.FilteredInstructorList.Contains(i) && i.SchoolManagerId == ((SchoolManager)app.CurrentUser).SmanagerId)
                        this.FilteredInstructorList.Add(i);
                }
            }
            else
            {
                foreach (Instructor ins in this.AllInstructors)
                {
                    string instructorString = $"{ins.Iname}";

                    if (!this.FilteredInstructorList.Contains(ins) && instructorString.Contains(search) && ins.SchoolManagerId == ((SchoolManager)app.CurrentUser).SmanagerId)
                        this.FilteredInstructorList.Add(ins);
                    else if (this.FilteredInstructorList.Contains(ins) && !instructorString.Contains(search))
                        this.FilteredInstructorList.Remove(ins);
                }
            }

            this.FilteredInstructorList = new ObservableCollection<Instructor>(this.FilteredInstructorList.OrderBy(i => i.Iname));
        }
    }
}
