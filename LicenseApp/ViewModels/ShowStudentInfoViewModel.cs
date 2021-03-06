using LicenseApp.Models;
using LicenseApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace LicenseApp.ViewModels
{
    public class ShowStudentInfoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int studentId;
        public int StudentId
        {
            get { return studentId; }
            set
            {
                studentId = value;
                OnPropertyChanged("StudentId");
            }
        }

        //פרטי התלמיד המוצגים במסך
        private string sName;
        public string SName
        {
            get { return sName; }
            set
            {
                sName = value;
                OnPropertyChanged("SName");
            }
        }

        private string imageUrl;
        public string ImageUrl
        {
            get { return imageUrl; }
            set
            {
                imageUrl = value;
                OnPropertyChanged("ImageUrl");
            }
        }

        private string phoneNum;
        public string PhoneNum
        {
            get { return phoneNum; }
            set
            {
                phoneNum = value;
                OnPropertyChanged("PhoneNum");
            }
        }

        private int lessonsCount;
        public int LessonsCount
        {
            get { return lessonsCount; }
            set
            {
                lessonsCount = value;
                OnPropertyChanged("LessonsCount");
            }
        }

        private double sAge;
        public double SAge
        {
            get { return sAge; }
            set
            {
                sAge = value;
                OnPropertyChanged("SAge");
            }
        }

        private string sCity;
        public string SCity
        {
            get { return sCity; }
            set
            {
                sCity = value;
                OnPropertyChanged("SCity");
            }
        }

        #region סיכום שיעור
        //רשימת השיעורים שביצע התלמיד הנבחר
        private ObservableCollection<Lesson> lessons;
        public ObservableCollection<Lesson> Lessons
        {
            get { return lessons; }
            set
            {
                lessons = value;
                OnPropertyChanged("Lessons");
            }
        }

        //פעולה הממלאת את רשימת השיעורים בערכים
        public async void GetLessons()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            Lessons = new ObservableCollection<Lesson>();

            ObservableCollection < Lesson > list = await proxy.GetStudentLessonsAsync(StudentId);

            foreach(Lesson l in list)
            {
                if (l.HasDone)
                    Lessons.Add(l);
            }
        }

        //תוכן סיכום השיעור שהזין המורה
        private string lessonSum;
        public string LessonSum
        {
            get { return lessonSum; }
            set
            {
                lessonSum = value;
                OnPropertyChanged("LessonSum");
            }
        }

        private Lesson lesson;
        public Lesson Lesson
        {
            get { return lesson; }
            set
            {
                lesson = value;
                OnPropertyChanged("Lesson");
            }
        }
        #endregion

        public Command DeleteStudentCommand => new Command(DeleteStudent);
        //פעולה המנתקת את הקשר בין התלמיד הנבחר לבין המורה אליו משויך
        public async void DeleteStudent()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            Student s = await proxy.DeleteStudent(StudentId);

            if (s != null)
            {
                ((App)App.Current).UIRefresh();
                await App.Current.MainPage.Navigation.PopToRootAsync();
            }
            else
                await App.Current.MainPage.DisplayAlert("שגיאה", "מחיקת התלמיד נכשלה! נסה שנית מאוחר יותר", "בסדר");
        }

        public Command AddSummaryCommand => new Command(AddSummary);
        //פעולה המוסיפה את סיכום השיעור שכתב המורה למסד הנתונים ומקשקת אותו לשיעור לגביו נכתב
        public async void AddSummary()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();

            //בדיקה שלא נתכב בעבר סיכום על שיעור זה
            bool exist = await proxy.CheckIfSumExists(Lesson.LessonId);

            if (exist)
            {
                await App.Current.MainPage.DisplayAlert("", "כבר כתבת סיכום שיעור לשיעור זה", "בסדר");
            }
            else
            {
                //הוספת סיכום השיעור החדש למסד הנתונים
                Review r = new Review
                {
                    Content = LessonSum,
                    WrittenOn = DateTime.Today
                };

                Review newR = await proxy.AddReview(r);

                if (newR != null)
                {
                    StudentSummary sum = new StudentSummary
                    {
                        ReviewId = newR.ReviewId,
                        StudentId = StudentId,
                        LessonId = Lesson.LessonId
                    };

                    StudentSummary newSum = await proxy.AddSummary(sum);

                    if (newSum != null)
                    {
                        //קישור סיכום השיעור לשיעור לגביו נכתב
                        Lesson l = new Lesson
                        {
                            Ldate = Lesson.Ldate,
                            LessonId = Lesson.LessonId,
                            HasDone = Lesson.HasDone,
                            Lday = Lesson.Lday,
                            IsAvailable = Lesson.IsAvailable,
                            IsPaid = Lesson.IsPaid,
                            StuudentId = Lesson.StuudentId,
                            InstructorId = Lesson.InstructorId,
                            ReviewId = newR.ReviewId
                        };

                        Lesson newL = await proxy.UpdateLessonSum(l);

                        if (newL != null)
                        {
                            ((App)App.Current).UIRefresh();
                            LessonSum = "";
                        }
                    }

                }

                else
                    await App.Current.MainPage.DisplayAlert("שגיאה", "שליחת סיכום השיעור נכשלה", "בסדר");
            }
        }
    }
}
