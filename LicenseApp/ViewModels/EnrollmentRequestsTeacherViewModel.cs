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
    public class EnrollmentRequestsTeacherViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public const int UNPERMITTED_STATUS = 3;
        public const int PERMITTED_STATUS = 2;
        public const int WAITING_STATUS = 1;

        private ObservableCollection<Student> studentsList;
        public ObservableCollection<Student> StudentsList
        {
            get
            {
                return this.studentsList;
            }
            set
            {
                if (this.studentsList != value)
                {

                    this.studentsList = value;
                    OnPropertyChanged("StudentsList");
                }
            }
        }

        private int amountOfRequests;
        public int AmountOfRequests
        {
            get
            {
                return this.amountOfRequests;
            }
            set
            {
                if (this.amountOfRequests != value)
                {

                    this.amountOfRequests = value;
                    OnPropertyChanged("AmountOfRequests");
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
            CreateCollection();
        }
        #endregion

        public EnrollmentRequestsTeacherViewModel()
        {
            StudentsList = new ObservableCollection<Student>();
            IsRefreshing = false;
            CreateCollection();
        }

        public async void CreateCollection()
        {
            IsRefreshing = true;

            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            App app = (App)App.Current;
            ObservableCollection<Student> students = new ObservableCollection<Student>();
            if (app.CurrentUser is Instructor)
            {
                Instructor instructor = (Instructor)app.CurrentUser;
                students = await proxy.GetAllWaitingStudentsByInstructor(instructor.InstructorId);
                if (students.Count != 0)
                {
                    foreach (Student i in students)
                    {
                        StudentsList.Add(i);
                    }

                    AmountOfRequests = StudentsList.Count;
                }
                IsRefreshing = false;
            }
        }

        public ICommand DisapproveCommand => new Command(OnDisapprove);
        public async void OnDisapprove(Object obj)
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();

            if (obj is Student)
            {
                Student s = (Student)obj;
                s.EStatusId = UNPERMITTED_STATUS;

                bool ok = await proxy.ChangeUserStatus(s);
                if (ok)
                {
                    OnRefresh();
                    ((App)App.Current).UIRefresh();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("שגיאה", "פעולה נכשלה!", "בסדר");
                }
            }

            else
            {
                await App.Current.MainPage.DisplayAlert("שגיאה", "פעולה נכשלה!", "בסדר");
            }
        }


        public ICommand ApproveCommand => new Command(OnApprove);
        public async void OnApprove(object obj)
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();

            if (obj is Student)
            {
                Student i = (Student)obj;
                i.EStatusId = PERMITTED_STATUS;

                bool ok = await proxy.ChangeUserStatus(i);
                if (ok)
                {
                    OnRefresh();
                    ((App)App.Current).UIRefresh();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("שגיאה", "פעולה נכשלה!", "בסדר");
                }
            }

            else
            {
                await App.Current.MainPage.DisplayAlert("שגיאה", "פעולה נכשלה!", "בסדר");
                //await App.Current.MainPage.Navigation.PopModalAsync();
            }
        }
    }
}
