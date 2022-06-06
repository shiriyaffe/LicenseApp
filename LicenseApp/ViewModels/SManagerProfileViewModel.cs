using LicenseApp.Models;
using LicenseApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LicenseApp.ViewModels
{
    public class SManagerProfileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        const string ERROR_PIC = "Error.png";
        const string Correct_PIC = "Correct.png";

        //פקטי המנהל המוצגים במסך
        #region password
        private string pass;
        public string Pass
        {
            get { return pass; }
            set
            {
                pass = value;
                ValidatePass();
                OnPropertyChanged("Pass");
            }
        }

        private string passError;

        public string PassError
        {
            get => passError;
            set
            {
                passError = value;
                OnPropertyChanged("PassError");
            }
        }

        private bool showPassError;

        public bool ShowPassError
        {
            get => showPassError;
            set
            {
                showPassError = value;
                OnPropertyChanged("ShowPassError");
            }
        }

        private void ValidatePass()
        {

            this.ShowPassError = string.IsNullOrEmpty(Pass);
            if (!this.ShowPassError)
            {
                if (!Regex.IsMatch(this.Pass, @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"))
                {
                    this.ShowPassError = true;
                    this.PassError = ERROR_PIC;
                }
                else
                {
                    this.ShowPassError = false;
                    this.PassError = Correct_PIC;
                }
            }
            else
            {
                this.ShowPassError = true;
                this.PassError = ERROR_PIC;
            }
        }

        public Command PassConditions { protected set; get; }

        private async void ShowConditions()
        {
            await App.Current.MainPage.DisplayAlert("הסיסמה חייבת להכיל:", "- ספרה אחת\n- אות אחת באנגלית\n- אורך מינימלי 8 ספרות", "אישור", FlowDirection.RightToLeft);
        }
        #endregion

        #region phone number
        private string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
                ValidateNumber();
                OnPropertyChanged("PhoneNumber");
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

        public void ValidateNumber()
        {
            if (!string.IsNullOrEmpty(PhoneNumber))
            {
                if (!Regex.IsMatch(this.PhoneNumber, @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"))
                {
                    this.ShowNumberError = true;
                    this.NumberError = ERROR_PIC;
                }
                else
                {
                    this.NumberError = Correct_PIC;
                    this.ShowNumberError = false;
                }
            }
            else
            {
                this.NumberError = ERROR_PIC;
                this.ShowNumberError = true;
            }
        }
        #endregion

        #region image
        private string imageUrl;
        public string ImageUrl
        {
            get { return imageUrl; }
            set
            {
                imageUrl = value;
                OnPropertyChanged("ImageUrl");
            }
        }

        FileResult imageFileResult;
        public event Action<ImageSource> SetImageSourceEvent;
        #region PickImage
        public ICommand PickImageCommand => new Command(OnPickImage);
        public async void OnPickImage()
        {
            FileResult result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions()
            {
                Title = "בחר תמונה"
            });

            if (result != null)
            {
                this.imageFileResult = result;

                var stream = await result.OpenReadAsync();
                ImageSource imgSource = ImageSource.FromStream(() => stream);
                if (SetImageSourceEvent != null)
                    SetImageSourceEvent(imgSource);
            }
        }
        #endregion

        //The following command handle the take photo button
        #region CameraImage
        public ICommand CameraImageCommand => new Command(OnCameraImage);
        public async void OnCameraImage()
        {
            var result = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions()
            {
                Title = "צלם תמונה"
            });

            if (result != null)
            {
                this.imageFileResult = result;
                var stream = await result.OpenReadAsync();
                ImageSource imgSource = ImageSource.FromStream(() => stream);
                if (SetImageSourceEvent != null)
                    SetImageSourceEvent(imgSource);
            }
        }
        #endregion
        #endregion

        public SManagerProfileViewModel()
        {
            App app = (App)App.Current;
            if (app.CurrentUser != null && app.CurrentUser is SchoolManager)
            {
                SchoolManager sm = (SchoolManager)app.CurrentUser;
                Pass = sm.Pass;
                PhoneNumber = sm.PhoneNumber;
                ImageUrl = sm.PhotoURI;
            }
            ShowNumberError = false;
            ShowPassError = false;
            this.SaveDataCommand = new Command(() => SaveData());
        }

        public Command SaveDataCommand { protected set; get; }

        //פעולה הבודקת את תקינות הפרטים החדשים שהזין המורה
        private bool ValidateForm()
        {
            //Validate all fields first
            ValidatePass();
            ValidateNumber();


            //check if any validation failed
            if (ShowNumberError)
                return false;
            return true;
        }

        //פעולה המעדכנת את פרטי המנהל המחובר בהתאם לשדות ששינה
        private async void SaveData()
        {
            if (ValidateForm())
            {
                App theApp = (App)App.Current;
                SchoolManager newSManager = new SchoolManager
                {
                    Pass = this.Pass,
                    PhoneNumber = this.PhoneNumber,
                    SmanagerId = ((SchoolManager)theApp.CurrentUser).SmanagerId,
                    Smname = ((SchoolManager)theApp.CurrentUser).Smname,
                    Email = ((SchoolManager)theApp.CurrentUser).Email,
                    GenderId = ((SchoolManager)theApp.CurrentUser).GenderId,
                    Birthday = ((SchoolManager)theApp.CurrentUser).Birthday,
                    SchoolId = ((SchoolManager)theApp.CurrentUser).SchoolId,
                    RegistrationDate = ((SchoolManager)theApp.CurrentUser).RegistrationDate
                };

                LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                //עדכון פרטי המנהל במסד הנתונים
                SchoolManager sManager = await proxy.UpdateSManager(newSManager);

                //הצגת הודעה בהתאם להצלחת העדכון
                if (sManager == null)
                {
                    await App.Current.MainPage.DisplayAlert("שגיאה", "העדכון נכשל", "אישור", FlowDirection.RightToLeft);
                }
                else
                {
                    if (this.imageFileResult != null)
                    {
                        bool success = await proxy.UploadImage(new Models.FileInfo()
                        {
                            Name = this.imageFileResult.FullPath
                        }, $"SchoolManagers\\{sManager.SmanagerId}.jpg");
                    }

                    theApp.CurrentUser = sManager;
                    await App.Current.MainPage.DisplayAlert("", "העדכון בוצע בהצלחה", "אישור", FlowDirection.RightToLeft);
                    
                    ((App)App.Current).UIRefresh();
                }
            }
            else
                await App.Current.MainPage.DisplayAlert("שמירת נתונים", " יש בעיה עם הנתונים בדוק ונסה שוב", "אישור", FlowDirection.RightToLeft);
        }
    }
}
