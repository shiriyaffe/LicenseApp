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

        private const int WAITING_STATUS = 1;
        private const int NO_STATUS = 4;
        private const int DENIED_STATUS = 3;

        #region driving school
        //רשימת בתי הספר הקיימים במערכת של האפליקציה
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

        //תכונה המתארת את תוכן הודעת השגיאה
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

        //תכונה המגדירה האם הודעת השגיאה תופיע בפני המשתמש
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

        //פעולה הבודקת אם נבחר בית ספר ומשנה את תוכן הודעת השגיאה בהתאם
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
        //פעולה השולחת בקשת רישום של מורה חדש למנהל בית הספר הנבחר
        public async void SendEnrollment()
        {
            App app = (App)App.Current;

            if (ValidateDrivingSchool())
            {
                //בדיקה אם המשתמש המחובר הינו מורה
                if (app.CurrentUser is Instructor)
                {
                    Instructor instructor = (Instructor)app.CurrentUser;
                    //בדיקה שסטטוס המורה הוא "אין סטטוס" או ""נדחה"
                    if (instructor.EStatusId == DENIED_STATUS || instructor.EStatusId == NO_STATUS)
                    {
                        //בניית אובייקט חדש של בקשת רישום
                        EnrollmentRequest er = new EnrollmentRequest
                        {
                            InstructorId = instructor.InstructorId,
                            StatusId = WAITING_STATUS,
                            SchoolId = DrivingSchool.SchoolId
                        };

                        LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                        //הוספת בקשת הרישום החדשה למסד הנתונים
                        EnrollmentRequest newEm = await proxy.AddEnrollmentRequest(er);

                        //בדיקה אם ההוספה התבצעה בהצלחה והצגת הודעת למשתמש בהתאם
                        if (er == null)
                        {
                            await App.Current.MainPage.DisplayAlert("שגיאה", "בקשתך לרישום נכשלה! נסה שנית מאוחר יותר", "אישור", FlowDirection.RightToLeft);
                        }
                        else
                        {
                            //עדכון סטטוס המורה המחובר ל"בהמתנה"
                            instructor.EStatusId = WAITING_STATUS;
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
