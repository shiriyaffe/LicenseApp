using LicenseApp.Models;
using LicenseApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LicenseApp.ViewModels
{
    public class AllStudentsListInSchoolViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private const int APPROVED = 2;

        private ObservableCollection<Student> StudentList;
        //רשימת תלמידים
        private ObservableCollection<Student> filteredStudentList;
        public ObservableCollection<Student> FilteredStudentList
        {
            get
            {
                return this.filteredStudentList;
            }
            set
            {
                if (this.filteredStudentList != value)
                {

                    this.filteredStudentList = value;
                    OnPropertyChanged("FilteredStudentList");
                }
            }
        }
        //מספר התלמידים
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

        public AllStudentsListInSchoolViewModel()
        {
            StudentList = new ObservableCollection<Student>();
            FilteredStudentList = new ObservableCollection<Student>();
            //בניית רשימת התלמידים שתוצג במסך
            CreateStudentCollection();
        }

        //פעולה הבונה את רשימת התלמידים
        public async void CreateStudentCollection()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            App app = (App)App.Current;
            //בדיקה האם המשתמש המחובר הוא מנהל בית ספר
            if (app.CurrentUser is SchoolManager)
            {
                //קריאת נתוני התלמידים המשויכים למנהל המחובר ממסד הנתונים
                ObservableCollection<Student> studentsBySchool = await proxy.GetStudentsBySchoolAsync(((SchoolManager)app.CurrentUser).SmanagerId);
                foreach (Student i in studentsBySchool)
                {
                    i.GetLessonsCount();
                    //בדיקה האם בקשת הרישום של התלמיד למורה אושרה, או שנמצא ברשימת המתנה
                    if(i.EStatusId == APPROVED)
                        this.StudentList.Add(i);
                }
            }

            //שמירת מספר התלמידים המשויכים למורים העובדים תחת המנהל המחובר
            StudentsCount = StudentList.Count;
            foreach(Student s in StudentList)
            {
                FilteredStudentList.Add(s);
            }
            //FilteredStudentList = (ObservableCollection<Student>)this.FilteredStudentList.OrderBy(s => s.Sname);
            this.SearchTerm = string.Empty;
        }

        //שדה החיפוש שהוזן על ידי המשתמש
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

        //פעולה הפועלת בכל פעם ששדה החיפוש משתנה והיא מסננת את רשימת התלמידים לפי שדה החיפוש
        public void OnTextChanged(string search)
        {
            App app = (App)App.Current;
            //בדיקה שישנם תלמידים ברשימה
            if (this.StudentList == null)
                return;
            if (String.IsNullOrWhiteSpace(search) || String.IsNullOrEmpty(search))
            {
                //החזרת כלל התלמידים לרשימה במידה ושדה החיפוש ריק
                foreach (Student s in this.StudentList)
                {
                    if (!this.FilteredStudentList.Contains(s))
                        this.FilteredStudentList.Add(s);
                }
            }
            else
            {
                //סינון רשימת התלמידים לפי שדה החיפוש
                foreach (Student ins in this.StudentList)
                {
                    string instructorString = $"{ins.Sname}";

                    if (!this.FilteredStudentList.Contains(ins))
                        this.FilteredStudentList.Add(ins);
                    else if (this.FilteredStudentList.Contains(ins) && !instructorString.Contains(search))
                        this.FilteredStudentList.Remove(ins);
                }
            }
            //סידור הרשימה לפי שם התלמידים
            this.FilteredStudentList = new ObservableCollection<Student>(this.FilteredStudentList.OrderBy(i => i.Sname));
        }
    }
}
