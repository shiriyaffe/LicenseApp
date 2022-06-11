using LicenseApp.Models;
using LicenseApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace LicenseApp.ViewModels
{
    public class ListOfLessonsInstructorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private const int WAITING = 1;
        private const int APPROVED = 2;
        private const int NO_STATUS = 4;

        //רשימת השיעורים המאושרים המתוכננים למורה בתאריך הנבחר
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

        //רשימת השיעורים הנמצאים בהמתנה לאישור לקיום בתאריך הנבחר
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
        //פעולה המרעננת את המסך
        public void OnRefresh()
        {
            CreateLessonsList();
        }
        #endregion

        //שמירת התאריך שבחר המורה
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
                    ValidateDate();
                    OnPropertyChanged("ChosenDate");
                }
            }
        }

        //בדיקת תקינותו של התאריך הנבחר והצגת הודעה בהתאם
        private void ValidateDate()
        {
            if (DateTime.Compare(DateTime.Today, ChosenDate) > 0)
            {
                App.Current.MainPage.DisplayAlert("", "תאריך זה כבר עבר", "בסדר");
                ChosenDate = DateTime.Today;
            }
            else
                OnRefresh();
        }


        public ListOfLessonsInstructorViewModel()
        {
            UpComingLessonsList = new ObservableCollection<Lesson>();
            WaitingLessonsList = new ObservableCollection<Lesson>();
            IsRefreshing = false;
            ChosenDate = new DateTime();
            ChosenDate = DateTime.Today;

            CreateLessonsList();
        }

        //הפעולה ממלאת את רשימות השיעורים המוצגות למורה בהתאם לתאריך הנבחר
        public async void CreateLessonsList()
        {
            IsRefreshing = true;

            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            App app = (App)App.Current;

            if (app.CurrentUser is Instructor)
            {
                Instructor i = (Instructor)app.CurrentUser;

                //קריאת נתוני השיעורים המשויכים למורה המחובר
                ObservableCollection<Lesson> list = new ObservableCollection<Lesson>();
                list = await proxy.GetInstructorLessonsAsync(i.InstructorId);
                UpComingLessonsList.Clear();
                WaitingLessonsList.Clear();

                if (list != null)
                {
                    //סינון השיעורים ושמירתם ברשימה הרלוונטית
                    foreach (Lesson l in list)
                    {
                        if (l.Ldate.Date == ChosenDate.Date)
                        {
                            if (!l.HasDone && l.EStatusId == APPROVED)
                                UpComingLessonsList.Add(l);
                            else if (!l.HasDone && l.EStatusId == WAITING)
                                WaitingLessonsList.Add(l);
                        }
                    }
                }
            }

            IsRefreshing = false;
        }

        public ICommand CancelLessonCommand => new Command(CancelLesson);
        //פעולה המבטלת קיום שיעור מתוכנן או דוחה בקשה לשיעור הנמצא בהמתנה
        public async void CancelLesson(Object obj)
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();

            if (obj is Lesson)
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
                //עדכון פרטי השיעור הקיים במערכת
                Lesson cancelled = await proxy.CancelLesson(l);

                //הצגת הודעת עדכון למשתמש בהתאם להצלחת ריצת הפעולה
                if (cancelled != null)
                {
                    await App.Current.MainPage.DisplayAlert("", "שיעור זה בוטל בהצלחה!:)", "בסדר");
                    
                    ((App)App.Current).UIRefresh();
                    OnRefresh();
                }
                else
                    await App.Current.MainPage.DisplayAlert("שגיאה", "אירעה שגיאה בעת ביטול השיעור. נסת שנית מאוחר יותר", "בסדר");
            }
        }

        public ICommand ApproveLessonCommand => new Command(ApproveLesson);
        //פעולה זו מאשרת בקשה לקיום שיעור חדש
        public async void ApproveLesson(Object obj)
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();

            if (obj is Lesson)
            {
                Lesson chosen = (Lesson)obj;
                chosen.EStatusId = APPROVED;
                //עדכון סטטוס השיעור ל"מאושר"
                Lesson approved = await proxy.ApproveLesson(chosen);
                
                //הצגת הודעה על המסך בהתאם להצלחת אישור השיעור
                if (approved != null)
                {
                    if (approved.InstructorId == 0)
                    {
                        await App.Current.MainPage.DisplayAlert("אישור השיעור התבטל!", "ישנו שיעור שכבר אושר בתאריך ושעה אלו:)", "בסדר");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("", "שיעור זה אושר בהצלחה!:)", "בסדר");
                        
                        ((App)App.Current).UIRefresh();
                        OnRefresh();
                    }
                }
                else
                    await App.Current.MainPage.DisplayAlert("שגיאה", "אירעה שגיאה בעת אישור השיעור. נסת שנית מאוחר יותר", "בסדר");
            }

        }
    }
}
