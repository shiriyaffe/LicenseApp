using LicenseApp.Models;
using LicenseApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace LicenseApp.ViewModels
{
    class DeniedInstructorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private const int STATUS_ID = 1;

        #region driving school
        public List<DrivingSchool> DrivingSchools
        {
            get
            {
                if (((App)App.Current).Tables != null)
                    return ((App)App.Current).Tables.DrivingSchools;
                return new List<DrivingSchool>();
            }
        }

        private DrivingSchool drivingSchool;
        public DrivingSchool DrivingSchool
        {
            get { return drivingSchool; }
            set
            {
                drivingSchool = value;
                OnPropertyChanged("DrivingSchool");
            }
        }

        public int drivingSchoolPicker;
        public int DrivingSchoolPicker
        {
            get { return drivingSchoolPicker; }
            set
            {
                drivingSchoolPicker = value;
                ValidateDrivingSchool();
                OnPropertyChanged("DrivingSchoolPicker");
            }
        }

        private string drivingSchoolError;

        public string DrivingSchoolError
        {
            get => drivingSchoolError;
            set
            {
                drivingSchoolError = value;
                OnPropertyChanged("DrivingSchoolError");
            }
        }

        private bool showDrivingSchoolError;

        public bool ShowDrivingSchoolError
        {
            get => showDrivingSchoolError;
            set
            {
                showDrivingSchoolError = value;
                OnPropertyChanged("ShowDrivingSchoolError");
            }
        }

        public bool ValidateDrivingSchool()
        {
            this.ShowDrivingSchoolError = DrivingSchoolPicker == -1;
            if (this.ShowDrivingSchoolError)
            {
                this.DrivingSchoolError = "בית ספר לנהיגה הוא שדה חובה!";
                return false;
            }
            else
            {
                this.DrivingSchoolError = null;
                return true;
            }
        }
        #endregion

        public DeniedInstructorViewModel()
        {
            DrivingSchoolPicker = -1;
            ShowDrivingSchoolError = false;
        }

        public Command SendEnrollmentCommand => new Command(SendEnrollment);

        public async void SendEnrollment()
        {
            App app = (App)App.Current;

            if (ValidateDrivingSchool())
            {
                if (app.CurrentUser is Instructor)
                {
                    Instructor instructor = (Instructor)app.CurrentUser;
                    if (instructor.EStatusId != STATUS_ID)
                    {
                        EnrollmentRequest er = new EnrollmentRequest
                        {
                            InstructorId = instructor.InstructorId,
                            StatusId = STATUS_ID,
                            SchoolId = DrivingSchool.SchoolId
                        };

                        LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                        EnrollmentRequest newEm = await proxy.AddEnrollmentRequest(er);

                        if (er == null)
                        {
                            await App.Current.MainPage.DisplayAlert("שגיאה", "בקשתך לרישום נכשלה! נסה שנית מאוחר יותר", "אישור", FlowDirection.RightToLeft);
                        }
                        else
                        {
                            instructor.EStatusId = STATUS_ID;
                            bool changed = await proxy.ChangeUserStatus(instructor);

                            if (changed)
                                await App.Current.MainPage.DisplayAlert("", "בקשתך לרישום הושלמה בהצלחה! יישלח לך מייל כאשר סטטוס הבקשה יתעדכן", "אישור", FlowDirection.RightToLeft);
                        }
                    }
                }
            }
        }
    }
}
