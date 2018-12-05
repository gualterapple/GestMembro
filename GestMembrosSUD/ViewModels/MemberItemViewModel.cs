using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GestMembrosSUD.Models;
using GestMembrosSUD.Views;
using Xamarin.Forms;

namespace GestMembrosSUD.ViewModels
{
    public class MemberItemViewModel : Member
    {
        #region Commands

        public ICommand SelectMemberCommand
        {
            get
            {
                return new RelayCommand(SelectMember);
            
            }
}

        private async void SelectMember()
        {
            MainViewModel.GetInstance().Member = new MemberViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new MemberPage());
        }
        #endregion
    }
}
