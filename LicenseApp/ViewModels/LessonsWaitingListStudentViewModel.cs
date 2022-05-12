using LicenseApp.Models;
using LicenseApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace LicenseApp.ViewModels
{
    public class LessonsWaitingListStudentViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private const int WAITING = 1;
        private const int APPROVED = 2;
        private const int NO_STATUS = 4;

        private ObservableCollection<Lesson> upComingLessonsList;
        public ObservableCollection<Lesson> UpComingLessonsList
        {
            get
            {
                return this.upComingLessonsList;
            }
            set
            {
                if (this.upComingLessonsList != value)
                {

                    this.upComingLessonsList = value;
                    OnPropertyChanged("UpComingLessonsList");
                }
            }
        }


        private ObservableCollection<Lesson> waitingLessonsList;
        public ObservableCollection<Lesson> WaitingLessonsList
        {
            get
            {
                return this.waitingLessonsList;
            }
            set
            {
                if (this.waitingLessonsList != value)
                {

                    this.waitingLessonsList = value;
                    OnPropertyChanged("WaitingLessonsList");
                }
            }
        }

        #region Refresh
        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                if (this.isRefreshing != value)
                {
                    this.isRefreshing = value;
                    OnPropertyChanged(nameof(IsRefreshing));
                }
            }
        }
        public ICommand RefreshCommand => new Command(OnRefresh);
        public void OnRefresh()
        {
            CreateLessonsList();
        }
        #endregion

        public LessonsWaitingListStudentViewModel()
        {
            UpComingLessonsList = new ObservableCollection<Lesson>();
            WaitingLessonsList = new ObservableCollection<Lesson>();
            IsRefreshing = false;
            App app = (App)App.Current;
            app.RefreshUI += OnRefresh;
            CreateLessonsList();
        }

        public async void CreateLessonsList()
        {
            IsRefreshing = true;

            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            App app = (App)App.Current;

            if (app.CurrentUser is Student)
            {
                Student s = (Student)app.CurrentUser;
                ObservableCollection<Lesson> list = new ObservableCollection<Lesson>();
                list = await proxy.GetStudentLessonsAsync(s.StudentId);
                UpComingLessonsList.Clear();
                WaitingLessonsList.Clear();
                if (list != null)
                {
                    foreach (Lesson l in list)
                    {
                        if (!l.HasDone && l.EStatusId == APPROVED)
                            UpComingLessonsList.Add(l);
                        else if (!l.HasDone && l.EStatusId == WAITING)
                            waitingLessonsList.Add(l);
                    }
                }
            }

            IsRefreshing = false;
        }

        public ICommand CancelLessonCommand => new Command(CancelLesson);

        public async void CancelLesson(Object obj)
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();

            if(obj is Lesson)
            {
                Lesson chosen = (Lesson)obj;

                Lesson l = new Lesson
                {
                    Ldate = chosen.Ldate,
                    Lday = chosen.Lday,
                    LessonId = chosen.LessonId,
                    Ltime = chosen.Ltime,
                    IsAvailable = true,
                    IsPaid = false,
                    HasDone = false,
                    StuudentId = null,
                    EStatusId = NO_STATUS,
                    InstructorId = chosen.InstructorId,
                    ReviewId = null
                };

                Lesson cancelled = await proxy.CancelLesson(l);

                if (cancelled != null)
                {
                    await App.Current.MainPage.DisplayAlert("", "שיעורך בוטל בהצלחה! מומלץ לקבוע שיעור נוסף במקודם:)", "בסדר");
                    OnRefresh();
                    ((App)App.Current).UIRefresh();
                }
                else
                    await App.Current.MainPage.DisplayAlert("שגיאה", "אירעה שגיאה בעת ביטול השיעור. נסת שנית מאוחר יותר", "בסדר");
            }
        }
    }
}
