using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using LicenseApp.Models;
using LicenseApp.Views;
using System.Collections.ObjectModel;
using LicenseApp.Services;



namespace LicenseApp.ViewModels
{
    public class ManagerSignInViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<Area> Areas
        {
            get
            {
                if (((App)App.Current).Tables != null)
                    return ((App)App.Current).Tables.Areas;
                return new List<Area>();
            }
        }

        private Area area;
        public Area Area
        {
            get { return area; }
            set
            {
                area = value;
                OnPropertyChanged("Area");
            }
        }

        #region שם בית ספר
        private string schoolName;
        public string SchoolName
        {
            get { return schoolName; }
            set
            {
                schoolName = value;
                ValidateName();
                OnPropertyChanged("SchoolName");
            }
        }

        private bool showNameError;

        public bool ShowNameError
        {
            get => showNameError;
            set
            {
                showNameError = value;
                OnPropertyChanged("ShowNameError");
            }
        }

        private string nameError;

        public string NameError
        {
            get => nameError;
            set
            {
                nameError = value;
                OnPropertyChanged("NameError");
            }
        }

        private void ValidateName()
        {
            this.ShowNameError = string.IsNullOrEmpty(SchoolName);
            if (ShowNameError)
                NameError = "השם הוא שדה חובה";
            else
                NameError = null;
        }
        #endregion

        #region מספר מורים
        private string numOfTeachers;
        public string NumOfTeachers
        {
            get { return numOfTeachers; }
            set
            {
                numOfTeachers = value;
                ValidateNumber();
                OnPropertyChanged("NumOfTeachers");
            }
        }

        private bool showNumberError;

        public bool ShowNumberError
        {
            get => showNumberError;
            set
            {
                showNumberError = value;
                OnPropertyChanged("ShowNumberError");
            }
        }

        private string numberError;

        public string NumberError
        {
            get => numberError;
            set
            {
                numberError = value;
                OnPropertyChanged("NumberError");
            }
        }

        private void ValidateNumber()
        {
            this.ShowNumberError = string.IsNullOrEmpty(NumOfTeachers);
            if (ShowNumberError)
                NumberError = "שדה זה הוא שדה חובה";
            else
                NumberError = null;
        }
        #endregion

        private string submitError;

        public string SubmitError
        {
            get => submitError;
            set
            {
                submitError = value;
                OnPropertyChanged("SubmitError");
            }
        }

        private bool showError;
        public bool ShowError
        {
            get => showError;
            set
            {
                showError = value;
                OnPropertyChanged("ShowNextError");
            }
        }

        public ManagerSignInViewModel()
        {
            ShowError = false;
            this.ShowNameError = false;
            this.ShowNumberError = false;
        }
    }
}
