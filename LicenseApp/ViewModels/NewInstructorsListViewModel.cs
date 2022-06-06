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
    public class NewInstructorsListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public const int UNPERMITTED_STATUS = 3;
        public const int PERMITTED_STATUS = 2;
        public const int WAITING_STATUS = 1;

        //רשימת המורים המבקשים להצטרף לבית הספר
        private ObservableCollection<Instructor> instructorsList;
        public ObservableCollection<Instructor> InstructorsList
        {
            get
            {
                return this.instructorsList;
            }
            set
            {
                if (this.instructorsList != value)
                {

                    this.instructorsList = value;
                    OnPropertyChanged("InstructorsList");
                }
            }
        }

        //מספק הבקשות הנמצאות בהמתנה
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
        //פעולה המרעננת את המסך
        public void OnRefresh()
        {
            CreateCollection();
        }
        #endregion

        public NewInstructorsListViewModel()
        {
            InstructorsList = new ObservableCollection<Instructor>();
            IsRefreshing = false;
            CreateCollection();
        }

        //פעולה הממלאת את רשימת המורים, במורים אשר מחכים להצטרף לבית ספרו של המנהל המחובר
        public async void CreateCollection()
        {
            IsRefreshing = true;

            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            App app = (App)App.Current;

            //קריאת נתוניהם של כלל המורים המחוברים לאפליקציה
            ObservableCollection<Instructor> instructors = new ObservableCollection<Instructor>();
            instructors = await proxy.GetAllInstructorsAsync();
            InstructorsList.Clear();
            foreach(Instructor i in instructors)
            {
                //הוספה לרשימת המורים רק את אלו הרלוונטים לדרישות
                if(i.EStatusId == WAITING_STATUS && i.DrivingSchoolId == ((SchoolManager)app.CurrentUser).SchoolId)
                {
                    InstructorsList.Add(i);
                }
            }

            AmountOfRequests = InstructorsList.Count;
            IsRefreshing = false;
        }

        public ICommand DisapproveCommand => new Command(OnDisapprove);
        //פעולה המופעלת כאשר המנהל דוחה בקשה של מורה
        public async void OnDisapprove(Object obj)
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();

            if (obj is Instructor)
            {
                //עדכון סטטוס המורה ל"נדחה"
                Instructor i = (Instructor)obj;
                i.EStatusId = UNPERMITTED_STATUS;

                bool ok = await proxy.ChangeUserStatus(i);
                if (ok)
                {
                    //רענון המסך במידה והשינוי התבצע בהצלחה
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
        //פעולה המופעלת כאשר המנהל מאשר בקשה של מורה
        public async void OnApprove(object obj)
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();

            if (obj is Instructor)
            {
                //עדכון סטטוס התלמיד ל"מאושר"
                Instructor i = (Instructor)obj;
                i.EStatusId = PERMITTED_STATUS;

                bool ok = await proxy.ChangeUserStatus(i);
                if (ok)
                {
                    //רענון המסך במידה והעדכון התבצע בהצלחה
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
    }
}
