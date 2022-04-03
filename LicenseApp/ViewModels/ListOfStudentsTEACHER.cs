using LicenseApp.Models;
using LicenseApp.Services;
using LicenseApp.Views;
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
    public class ListOfStudentsTEACHER : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Student> AllStudents;

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


        private int studentsCount;
        public int StudentsCount
        {
            get
            {
                return this.studentsCount;
            }
            set
            {
                if (this.studentsCount != value)
                {

                    this.studentsCount = value;
                    OnPropertyChanged("StudentsCount");
                }
            }
        }

        public ListOfStudentsTEACHER()
        {
            AllStudents = new ObservableCollection<Student>();
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
                    this.AllStudents.Add(i);
                }
            }

            StudentsCount = AllStudents.Count;
            StudentList = new ObservableCollection<Student>(this.AllStudents.OrderBy(i => i.Sname));
            this.SearchTerm = string.Empty;
        }

        private double GetAge(DateTime birthday)
        {
            int month = 0;
            if(birthday.Month > DateTime.Today.Month)
            {
                if(birthday.Day > DateTime.Today.Day)
                {
                    month = DateTime.Today.Month - birthday.Month;
                }
                else
                {
                    month = (DateTime.Today.Month - birthday.Month) + 1;
                }
            }

            return (DateTime.Today.Year - birthday.Year) + (month * 0.1);
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
                    ImageUrl = chosenStudent.PhotoURI,
                    StudentId = chosenStudent.StudentId,
                    SName = chosenStudent.Sname,
                    SAge = GetAge(chosenStudent.Birthday),
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

        private string searchTerm;
        public string SearchTerm
        {
            get
            {
                return this.searchTerm;
            }
            set
            {
                if (this.searchTerm != value)
                {
                    this.searchTerm = value;
                    OnTextChanged(value);
                    OnPropertyChanged("SearchTerm");
                }
            }
        }

        public void OnTextChanged(string search)
        {
            App app = (App)App.Current;
            //Filter the list of contacts based on the search term
            if (this.AllStudents == null)
                return;
            if (String.IsNullOrWhiteSpace(search) || String.IsNullOrEmpty(search))
            {
                foreach (Student i in this.AllStudents)
                {
                    if (!this.StudentList.Contains(i))
                        this.StudentList.Add(i);
                }
            }
            else
            {
                foreach (Student ins in this.AllStudents)
                {
                    string instructorString = $"{ins.Sname}";

                    if (!this.StudentList.Contains(ins))
                        this.StudentList.Add(ins);
                    else if (this.StudentList.Contains(ins) && !instructorString.Contains(search))
                        this.StudentList.Remove(ins);
                }
            }

            this.StudentList = new ObservableCollection<Student>(this.StudentList.OrderBy(i => i.Sname));
        }
    }
}
