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

        //רשימת השעות הפנויות
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

        //התאריך שנבחר על ידי התלמיד
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

        //פעולה המופעלת בכל פעם שהתאריך משתנה והיא בודקת שהתאריך הנבחר אינו עבר
        private void ValidateDate()
        {
            //הצגת הודעת שגיאה על המסך בצידה והתאריך אינו תקין
            if(DateTime.Compare(DateTime.Today, ChosenDate) > 0)
            {
                App.Current.MainPage.DisplayAlert("", "תאריך זה כבר עבר", "בסדר");
                ChosenDate = DateTime.Today;
            }
            else //רענון המסך במידה והתאריך הנבחר תקין
                OnRefresh();
        }

        //פעולה הבונה המרעננת את המסך על ידי בניה מחדש של רשימת השעות הפנויות
        public void OnRefresh()
        {
            CreateLessonsList();
        }

        public AvailableLessonsViewModel()
        {
            AvailableList = new ObservableCollection<WorkingHour>();
            App app = (App)App.Current;
            //app.RefreshUI += OnRefresh;
            //הגדרת תאריך אוטומטי לתאריך הנוכחי
            ChosenDate = new DateTime();
            ChosenDate = DateTime.Today;

            CreateLessonsList();
        }

        //פעולה הממלאת את רשימת השעות הפנויות בערכים, בהתאם לתאריך הנבחר
        public async void CreateLessonsList()
        {
            App app = (App)App.Current;
            Student current = new Student();
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            AvailableList.Clear();
            
            current = (Student)app.CurrentUser;

            List<WorkingHour> allHours = new List<WorkingHour>();
            allHours = app.Tables.WorkingHours;
            Lesson l = new Lesson();
            bool available = false;

            int startId = 0;
            int endId = 0;
            //בדיקה שהשעות הפנויות שיוצגו יהיו בהתאם לשעות העבודה של המורה של תלמיד המחובר
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
                //בדיקה במסד הנתונים אם למורה כבר יש שיעור מאושר בזמן והיום שנבחרו
                available = await proxy.CheckIfAvailable(l);
                
                //במידה ואין שיעור זהה, הוספת השעה לרשימת השעות הפנויות
                if (available)
                {
                    if(wh.HourId >= startId && wh.HourId <= endId)
                        AvailableList.Add(wh);
                }
            }
        }

        public ICommand SelctionChanged => new Command<Object>(BookALesson);
        //פעולה הפועלת בעת בחירה של שעה מסוימת והיא שולחת בקשה לקביעת שיעור חדש
        public async void BookALesson(Object obj)
        {
            if (obj is WorkingHour)
            {
                LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                WorkingHour chosenHour = (WorkingHour)obj;

                App app = (App)App.Current;
                Student current = (Student)app.CurrentUser;
                //בדיקה אם התלמיד רוצה בוודאות לשלוח בקשה לתאריך ושעה הספציפיים שנבחרו 
                bool uSure = await App.Current.MainPage.DisplayAlert("שים לב!", $"האם ברצונך לשלוח בקשה לשיעור בשעה {chosenHour.Whour}?", "כן", "לא");

                if(uSure)
                {
                    //בניית אובייקט שיעור חדש עם הפרטים המתאימים לתלמיד
                    Lesson l = new Lesson
                    {
                        Ldate = ChosenDate.Date,
                        Lday = ChosenDate.DayOfWeek.ToString(),
                        Ltime = chosenHour.Whour,
                        IsAvailable = false,
                        IsPaid = false,
                        HasDone = false,
                        StuudentId = current.StudentId,
                        EStatusId = WAITING_STATUS,
                        InstructorId = (int)current.InstructorId
                    };

                    //הוספת השיעור החדש לטבלת השיעורים במסד הנתונים
                    Lesson newL = await proxy.AddNewLesson(l);

                    //בדיקה אם הוספת השיעור התבצעה בהצלחה והצגת הודעה על המסך בהתאם
                    if(newL != null)
                    {
                        await App.Current.MainPage.DisplayAlert("", $"בקשתך לשיעור נשלחה למורה בהצלחה!", "בסדר");
                        //רענון המסכים באפליקציה כולה
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
