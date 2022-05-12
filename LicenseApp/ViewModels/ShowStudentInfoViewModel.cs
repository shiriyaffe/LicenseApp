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

        #region סיכום שיעור
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

        public Command DeleteStudentCommand => new Command(DeleteStudent);

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

            //Page p = new Views.InstructorMainTabView();
            //await App.Current.MainPage.Navigation.PushAsync(p);
        }

        public Command AddSummaryCommand => new Command(AddSummary);

        public async void AddSummary()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();

            Review r = new Review
            {
                Content = LessonSum,
                WrittenOn = DateTime.Today
            };

            Review newR = await proxy.AddReview(r);

            if(newR != null)
            {
                StudentSummary sum = new StudentSummary
                {
                    ReviewId = newR.ReviewId,
                    StudentId = StudentId
                };

                StudentSummary newSum = await proxy.AddSummary(sum);

                if(newSum != null)
                {
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
