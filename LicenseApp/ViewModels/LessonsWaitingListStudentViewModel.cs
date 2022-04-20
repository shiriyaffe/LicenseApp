using LicenseApp.Models;
using LicenseApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace LicenseApp.ViewModels
{
    public class LessonsWaitingListStudentViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private const int WAITING = 1;
        private const int APPROVED = 2;

        private ObservableCollection<Lesson> upComingLessonsList;
        public ObservableCollection<Lesson> UpComingLessonsList
        {
            get
            {
                return this.upComingLessonsList;
            }
            set
            {
                if (this.upComingLessonsList != value)
                {

                    this.upComingLessonsList = value;
                    OnPropertyChanged("UpComingLessonsList");
                }
            }
        }


        private ObservableCollection<Lesson> waitingLessonsList;
        public ObservableCollection<Lesson> WaitingLessonsList
        {
            get
            {
                return this.waitingLessonsList;
            }
            set
            {
                if (this.waitingLessonsList != value)
                {

                    this.waitingLessonsList = value;
                    OnPropertyChanged("WaitingLessonsList");
                }
            }
        }

        public LessonsWaitingListStudentViewModel()
        {
            UpComingLessonsList = new ObservableCollection<Lesson>();
            WaitingLessonsList = new ObservableCollection<Lesson>();

            CreateLessonsList();
        }

        public async void CreateLessonsList()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            App app = (App)App.Current;

            if (app.CurrentUser is Student)
            {
                Student s = (Student)app.CurrentUser;
                ObservableCollection<Lesson> list = new ObservableCollection<Lesson>();
                list = await proxy.GetStudentLessonsAsync(s.StudentId);

                if (list != null)
                {
                    foreach (Lesson l in list)
                    {
                        if (!l.HasDone && l.EStatusId == APPROVED)
                            UpComingLessonsList.Add(l);
                        else if (!l.HasDone && l.EStatusId == WAITING)
                            waitingLessonsList.Add(l);
                    }
                }
            }
        }
    }
}
