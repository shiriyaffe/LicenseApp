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

        private ObservableCollection<Student> StudentList;

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
            CreateStudentCollection();
        }

        public async void CreateStudentCollection()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            App app = (App)App.Current;
            if (app.CurrentUser is SchoolManager)
            {
                ObservableCollection<Student> studentsBySchool = await proxy.GetStudentsBySchoolAsync(((SchoolManager)app.CurrentUser).SmanagerId);
                foreach (Student i in studentsBySchool)
                {
                    this.StudentList.Add(i);
                }
            }

            StudentsCount = StudentList.Count;
            foreach(Student s in StudentList)
            {
                FilteredStudentList.Add(s);
            }
            //FilteredStudentList = (ObservableCollection<Student>)this.FilteredStudentList.OrderBy(s => s.Sname);
            this.SearchTerm = string.Empty;
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
            if (this.StudentList == null)
                return;
            if (String.IsNullOrWhiteSpace(search) || String.IsNullOrEmpty(search))
            {
                foreach (Student s in this.StudentList)
                {
                    if (!this.FilteredStudentList.Contains(s))
                        this.FilteredStudentList.Add(s);
                }
            }
            else
            {
                foreach (Student ins in this.StudentList)
                {
                    string instructorString = $"{ins.Sname}";

                    if (!this.FilteredStudentList.Contains(ins))
                        this.FilteredStudentList.Add(ins);
                    else if (this.FilteredStudentList.Contains(ins) && !instructorString.Contains(search))
                        this.FilteredStudentList.Remove(ins);
                }
            }

            this.FilteredStudentList = new ObservableCollection<Student>(this.FilteredStudentList.OrderBy(i => i.Sname));
        }
    }
}
