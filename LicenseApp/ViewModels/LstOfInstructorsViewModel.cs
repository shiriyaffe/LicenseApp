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

namespace LicenseApp.ViewModels
{
    public class LstOfInstructorsViewModel : INotifyPropertyChanged
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

        private int allInstructors;
        public int AllInstructors
        {
            get
            {
                return this.allInstructors;
            }
            set
            {
                if (this.allInstructors != value)
                {

                    this.allInstructors = value;
                    OnPropertyChanged("AllInstructors");
                }
            }
        }

        public LstOfInstructorsViewModel()
        {
            InstructorList = new ObservableCollection<Instructor>();
            CreateInstructorCollection();
        }

        public async void CreateInstructorCollection()
        {
            App app = (App)App.Current;
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            ObservableCollection<Instructor> instructors = await proxy.GetAllInstructorsAsync();
            foreach (Instructor i in instructors)
            {
                if (i.SchoolManagerId == ((SchoolManager)app.CurrentUser).SmanagerId)
                    InstructorList.Add(i);
            }

            AllInstructors = InstructorList.Count;
        }

        public ICommand SelctionChanged => new Command<Object>(OnSelectionChanged);
        public async void OnSelectionChanged(Object obj)
        {
            if (obj is Instructor)
            {
                LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                Instructor chosenInstructor = (Instructor)obj;

                ShowInstrucorSMViewModel instructorContext = new ShowInstrucorSMViewModel
                {
                    IName = chosenInstructor.Iname,
                    ImageUrl = chosenInstructor.PhotoURI,
                    Email = chosenInstructor.Email,
                    PhoneNum = chosenInstructor.PhoneNumber,
                    LessonLength = chosenInstructor.LessonLengthId,
                    Price = chosenInstructor.Price,
                    InstructorID = chosenInstructor.InstructorId
                };

                App app = (App)App.Current;
                if (app.CurrentUser != null)
                {
                    Page p = new ShowInstructorSMView(instructorContext);
                    await App.Current.MainPage.Navigation.PushAsync(p);
                }
                else
                    await App.Current.MainPage.DisplayAlert("שגיאה", "יש להתחבר למערכת כדי לגשת לפרטים נוספים של מורה", "בסדר");
            }
        }
    }
}
