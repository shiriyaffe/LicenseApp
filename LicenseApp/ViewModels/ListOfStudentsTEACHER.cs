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

        private const int APPROVED = 2;

        private ObservableCollection<Student> AllStudents;

        //רשימת התלמידים המשויכים למורה
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

        //מספר התלמידים של המורה
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
            App theApp = (App)Application.Current;
            theApp.RefreshUI += TheApp_RefreshUI;
        }

        //פעולה המרעננת את המסך
        private void TheApp_RefreshUI()
        {
            CreateStudentCollection();
        }

        //פעולה ממלאת את רשימת התלמידים בערכים בהתאם למורה המחובר
        public async void CreateStudentCollection()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            App app = (App)App.Current;
            AllStudents.Clear();
            if (app.CurrentUser is Instructor)
            {
                //קריאת נתוניהם של כלל התלמידים המשויכים למורה
                ObservableCollection<Student> studentsByID = await proxy.GetStudentsByInstructorAsync(((Instructor)app.CurrentUser).InstructorId);
                foreach (Student i in studentsByID)
                {
                    i.GetLessonsCount();
                    //הוספת התלמידים שאושרו על ידי המורה בלבד
                    if(i.EStatusId == APPROVED)
                        this.AllStudents.Add(i);
                }
            }

            StudentsCount = AllStudents.Count;

            //סידור הרשימה לפני שמות התלמידים
            StudentList = new ObservableCollection<Student>(this.AllStudents.OrderBy(i => i.Sname));
            this.SearchTerm = string.Empty;
        }

        //פעולה המחזירה את הגיל המדוייק של התלמיד
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
        //פעולה המופעלת בעת בחירת תלמיד מסוים מהרשימה ומציגה למורה את פרטיו המלאים של תלמיד זה
        public async void OnSelectionChanged(Object obj)
        {
            if (obj is Student)
            {
                LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                Student chosenStudent = (Student)obj;
                chosenStudent.GetLessonsCount();

                //העברת נתוני התלמיד הנבחר למסך הבא
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
                
                    Page p = new ShowStudentInfoView(studentContext);
                    await App.Current.MainPage.Navigation.PushAsync(p);
            }
        }

        //שדה החיפוש שהזין המורה
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

        //סינון רשימת התלמידים לפי שדה החיפוש שהזין המורה
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
