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
    public class ListOfStudentsByInstructorIdViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private const int APPROVED = 2;

        private ObservableCollection<Student> AllStudents;

        //רשימת התלמידים המוצגים במסך
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

        //מספר התלמידים הרשומים לבית הספר
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

        public ListOfStudentsByInstructorIdViewModel()
        {
            AllStudents = new ObservableCollection<Student>();
        }

        //פעולה הממלאת את רשימת התלמידים בערכים בהתאם למנהל המחובר
        public async void CreateStudentCollection(int instructorId)
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            App app = (App)App.Current;
            if (app.CurrentUser is SchoolManager)
            {
                //קריאת נתוניהם של כלל התלמידים המשויכים לבית הספר של המורה המחובר
                ObservableCollection<Student> studentsByID = await proxy.GetStudentsByInstructorAsync(instructorId);
                foreach (Student i in studentsByID)
                {
                    i.GetLessonsCount();
                    //הכנסה לרשימה רק תלמידים שבקשתם רישומם אושרה על ידי המורה
                    if(i.EStatusId == APPROVED)
                        this.AllStudents.Add(i);
                }
            }

            StudentsCount = AllStudents.Count;
            //סידור התלמידים ברשימה לפני השם
            StudentList = new ObservableCollection<Student>(this.AllStudents.OrderBy(s => s.Sname));
            this.SearchTerm = string.Empty;
        }

        //שמירת שדה החיפוש שהזין המנהל
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

        //פעולה המופעלת בעת שינוי שדה החיפוש ומסננת את רשימת התלמידים לפיו
        public void OnTextChanged(string search)
        {
            App app = (App)App.Current;
            //Filter the list of contacts based on the search term
            if (this.AllStudents == null)
                return;
            if (String.IsNullOrWhiteSpace(search) || String.IsNullOrEmpty(search))
            {
                foreach (Student s in this.AllStudents)
                {
                    if (!this.StudentList.Contains(s))
                        this.StudentList.Add(s);
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
