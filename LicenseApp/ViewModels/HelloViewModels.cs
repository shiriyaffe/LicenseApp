using LicenseApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Essentials;

namespace LicenseApp.ViewModels
{
    class HelloViewModels : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string text;

        public string Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }

        public HelloViewModels()
        {
            Text = "";
        }

        public ICommand ShowLbl => new Command(ShowLabel);
        private void ShowLabel()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            Task<string> t = proxy.SayHello();
            t.Wait();
            Text = t.Result;
        }
    }
}
