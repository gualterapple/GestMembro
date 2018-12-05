using System;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GestMembrosSUD.Views;
using Xamarin.Forms;

namespace GestMembrosSUD.ViewModels
{
    public class LoginViewModel:BaseViewModel
    {
        #region Attributes
        private string email;
        private string password;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties
        public string Email
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }
        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }
        public bool IsRemembered
        {
            get;
            set;
        }
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            this.Email = "gualter@gmail.com";
            this.Password = "1234";
            this.IsRemembered = true;
            this.IsEnabled = true;

        }
        #endregion

        #region Commands

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);

            }
        }

        #endregion

        #region Methods

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter email",
                    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter your password",
                    "Accept");
                this.Password = string.Empty;
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            if (this.Email != "gualter@gmail.com" || this.Password != "1234")
            {
                await Application.Current.MainPage.DisplayAlert(
                "Error",
                "Your e-mail or password is incorrect",
                "Accept");

                this.IsRunning = false;
                this.IsEnabled = true;
                this.Password = string.Empty;

                return;
            }

            this.IsRunning = false;
            this.IsEnabled = true;

            MainViewModel.GetInstance().Members = new MembersViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new MembersPage());
        }
        #endregion



    }
}
