using LicenseApp.Models;
using LicenseApp.Services;
using LicenseApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace LicenseApp.ViewModels
{
    public class ListOfStudentsTEACHER : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Student> studentList;
        public ObservableCollection<Student> StudentList
        {
            get
            {
                return this.studentList;
            }
            set
            {
                if (this.studentList != value)
                {

                    this.studentList = value;
                    OnPropertyChanged("StudentList");
                }
            }
        }

        private int allStudents;
        public int AllStudents 
        {
            get
            {
                return this.allStudents;
            }
            set
            {
                if (this.allStudents != value)
                {

                    this.allStudents = value;
                    OnPropertyChanged("AllStudents");
                }
            }
        }

        public ListOfStudentsTEACHER()
        {
            StudentList = new ObservableCollection<Student>();
        }

        public async void CreateStudentCollection()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            App app = (App)App.Current;
            if (app.CurrentUser is Instructor)
            {
                ObservableCollection<Student> studentsByID = await proxy.GetStudentsByInstructorAsync(((Instructor)app.CurrentUser).InstructorId);
                foreach (Student i in studentsByID)
                {
                    this.StudentList.Add(i);
                }
            }

            AllStudents = StudentList.Count;
        }

        public ICommand SelctionChanged => new Command<Object>(OnSelectionChanged);
        public async void OnSelectionChanged(Object obj)
        {
            if (obj is Student)
            {
                LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                Student chosenStudent = (Student)obj;

                ShowStudentInfoViewModel studentContext = new ShowStudentInfoViewModel
                {
                    ImageUrl = $"defaultPhoto.png",
                    SName = chosenStudent.Sname,
                    SAge = DateTime.Today.Year - ((DateTime)chosenStudent.Birthday).Year,
                    SCity = (await proxy.GetCityById(chosenStudent.CityId)).CityName,
                    LessonsCount = chosenStudent.LessonsCount,
                    PhoneNum = chosenStudent.PhoneNumber
                };

                App app = (App)App.Current;
                if (app.CurrentUser != null)
                {
                    Page p = new ShowStudentInfoView(studentContext);
                    await App.Current.MainPage.Navigation.PushAsync(p);
                }
                else
                    await App.Current.MainPage.DisplayAlert("שגיאה", "יש להתחבר למערכת כדי לגשת לפרטים נוספים של מורה", "בסדר");
            }
        }

        public ICommand SearchPageCommand => new Command(OpenSearchPage);

        public async void OpenSearchPage()
        {
            App app = (App)App.Current;
            Page p = new SearchPageView();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
