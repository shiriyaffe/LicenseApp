﻿using LicenseApp.Models;
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
    public class MyInstructorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        const int APPROVED = 2;
        const int NO_RATING = 0;

        private string iName;
        public string IName
        {
            get => iName;
            set
            {
                iName = value;
                OnPropertyChanged("IName");
            }
        }

        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }

        private string phoneNum;
        public string PhoneNum
        {
            get => phoneNum;
            set
            {
                phoneNum = value;
                OnPropertyChanged("PhoneNum");
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

        private string review;
        public string Review
        {
            get { return review; }
            set
            {
                review = value;
                OnPropertyChanged("Review");
            }
        }

        private int ratingValue;
        public int RatingValue
        {
            get { return ratingValue; }
            set
            {
                ratingValue = value;
                OnPropertyChanged("RatingValue");
            }
        }

        public MyInstructorViewModel()
        {
            App app = (App)App.Current;
            if (app.CurrentUser is Student)
            {
                GetInfo();
            }
        }

        private async void GetInfo()
        {
            App app = (App)App.Current;
            if (app.CurrentUser is Student)
            {
                Student s = (Student)app.CurrentUser;

                LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                
                Instructor instructor = await proxy.GetInstructorById((int)s.InstructorId);

                if(instructor != null)
                {
                    this.ImageUrl = instructor.PhotoURI;
                    this.Email = instructor.Email;
                    this.IName = instructor.Iname;
                    this.PhoneNum = instructor.PhoneNumber;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("שגיאה", "לא נרשמת עדיין אצל מורה", "בסדר");
                    app.MainPage = new NavigationPage(new StudentMainTabView());
                }
            }
        }

        public ICommand AddReviewCommand => new Command(AddReview);

        public async void AddReview()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            App app = (App)App.Current;

            Review r = new Review
            {
                Content = Review,
                WrittenOn = DateTime.Today
            };

            Review newR = await proxy.AddReview(r);

            if (newR != null)
            {
                InstructorReview ir = new InstructorReview
                {
                    ReviewId = newR.ReviewId,
                    InstructorId = (int)(((Student)app.CurrentUser).InstructorId)
                };

                InstructorReview newReview = await proxy.AddInstructorReview(ir);

                if (newReview == null)
                {
                    await App.Current.MainPage.DisplayAlert("שגיאה", "שליחת ביקורת נכשלה", "בסדר");
                }
                else
                {
                    Review = "";
                    await App.Current.MainPage.DisplayAlert("", "ביקורתך נקלטה בהצלחה! תודה על מתן הביקורת", "בסדר");
                    ((App)App.Current).UIRefresh();
                }
            }
            else
                await App.Current.MainPage.DisplayAlert("שגיאה", "שליחת ביקורת נכשלה", "בסדר");
        }

        public ICommand BookALessonCommand => new Command(BookALesson);

        public void BookALesson()
        {
            App app = (App)App.Current;
            Page p = new AvailableLessonsView();
            app.MainPage.Navigation.PushAsync(p);
        }

        public ICommand AddRatingCommand => new Command(AddRating);

        public async void AddRating()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            App app = (App)App.Current;

            Instructor i = ((Student)app.CurrentUser).Instructor;
            if(i != null)
            {
                if (i.RateId == NO_RATING)
                {
                    i.RateId = RatingValue;
                }
                else
                {
                    i.RateId = (i.RateId + RatingValue) / 2;
                }

                bool updated = await proxy.ChangeRating(i);

                if(updated)
                {
                    RatingValue = 0;
                    await App.Current.MainPage.DisplayAlert("", "דירוגך נקלט בהצלחה! תודה על מתן הביקורת", "בסדר");
                    ((App)App.Current).UIRefresh();
                }
                else
                    await App.Current.MainPage.DisplayAlert("שגיאה", "הוספת הדירוג נכשלה", "בסדר");
            }
            else
                await App.Current.MainPage.DisplayAlert("שגיאה", "הוספת הדירוג נכשלה", "בסדר");
        }
    }
}
