using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using LicenseApp.Models;
using LicenseApp.Services;
using Xamarin.Forms;

namespace LicenseApp.ViewModels
{
    class AvailableLessonsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private const int NO_STATUS = 4;
        private const int WAITING_STATUS = 1;

        private ObservableCollection<WorkingHour> availableList;
        public ObservableCollection<WorkingHour> AvailableList
        {
            get
            {
                return this.availableList;
            }
            set
            {
                if (this.availableList != value)
                {

                    this.availableList = value;
                    OnPropertyChanged("AvailableList");
                }
            }
        }

        private DateTime chosenDate;
        public DateTime ChosenDate
        {
            get
            {
                return this.chosenDate;
            }
            set
            {
                if (this.chosenDate != value)
                {

                    this.chosenDate = value;
                    OnPropertyChanged("ChosenDate");
                }
            }
        }

        public void OnRefresh()
        {
            CreateLessonsList();
        }

        public AvailableLessonsViewModel()
        {
            ChosenDate = new DateTime(2022, 04, 25);
            AvailableList = new ObservableCollection<WorkingHour>();
            App app = (App)App.Current;
            app.RefreshUI += OnRefresh;
            CreateLessonsList();
        }

        public async void CreateLessonsList()
        {
            App app = (App)App.Current;
            Student current = new Student();
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            AvailableList.Clear();
            ObservableCollection<Student> allStudents = await proxy.GetStudentsByInstructorAsync((int)(((Student)app.CurrentUser).InstructorId));
            foreach(Student s in allStudents)
            {
                if (s.StudentId == ((Student)app.CurrentUser).StudentId)
                    current = s;
            }

            List<WorkingHour> allHours = app.Tables.WorkingHours;
            Lesson l = new Lesson();
            bool available = false;

            int startId = 0;
            int endId = 0;
            foreach (WorkingHour w in allHours)
            {
                if (w.Whour.Equals(current.Instructor.StartTime))
                    startId = w.HourId;
                if (w.Whour.Equals(current.Instructor.EndTime))
                    endId = w.HourId;
            }

            foreach (WorkingHour wh in allHours)
            {
                l = new Lesson
                {
                    Ldate = ChosenDate,
                    Lday = "",
                    LessonId = 0,
                    Ltime = wh.Whour,
                    IsAvailable = false,
                    IsPaid = false,
                    HasDone = false,
                    StuudentId = null,
                    EStatusId = NO_STATUS,
                    InstructorId = (int)current.InstructorId,
                    ReviewId = null
                };

                available = await proxy.CheckIfAvailable(l);
                
                if (available)
                {
                    if(wh.HourId >= startId && wh.HourId <= endId)
                        AvailableList.Add(wh);
                }
            }
        }

        public ICommand SelctionChanged => new Command<Object>(OnSelectionChanged);
        public async void OnSelectionChanged(Object obj)
        {
            if (obj is WorkingHour)
            {
                LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                WorkingHour chosenHour = (WorkingHour)obj;

                App app = (App)App.Current;
                Student current = (Student)app.CurrentUser;

                bool uSure = await App.Current.MainPage.DisplayAlert("שים לב!", $"האם ברצונך לשלוח בקשה לשיעור בשעה {chosenHour.Whour}?", "כן", "לא");

                if(uSure)
                {
                    Lesson l = new Lesson
                    {
                        Ldate = ChosenDate,
                        Lday = ChosenDate.DayOfWeek.ToString(),
                        Ltime = chosenHour.Whour,
                        IsAvailable = false,
                        IsPaid = false,
                        HasDone = false,
                        StuudentId = current.StudentId,
                        EStatusId = WAITING_STATUS,
                        InstructorId = current.StudentId,
                    };

                    Lesson newL = await proxy.AddNewLesson(l);

                    if(newL != null)
                    {
                        await App.Current.MainPage.DisplayAlert("", $"בקשתך לשיעור נשלחה למורה בהצלחה!", "בסדר");
                        OnRefresh();
                        ((App)App.Current).UIRefresh();
                    }
                    else
                        await App.Current.MainPage.DisplayAlert("שגיאה!", $"בקשתך לשיעור נכשלה.. נסה שוב", "בסדר");
                }
            }
        }
    }
}
