using LicenseApp.Models;
using LicenseApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        public ListOfStudentsByInstructorIdViewModel()
        {
            StudentList = new ObservableCollection<Student>();
        }

        public async void CreateStudentCollection(int instructorId)
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            App app = (App)App.Current;
            if (app.CurrentUser is SchoolManager)
            {
                ObservableCollection<Student> studentsByID = await proxy.GetStudentsByInstructorAsync(instructorId);
                foreach (Student i in studentsByID)
                {
                    this.StudentList.Add(i);
                }
            }

            AllStudents = StudentList.Count;
        }

    }
}
