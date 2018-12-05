using System;
using System.IO;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GestMembrosSUD.Models;
using GestMembrosSUD.Views;
using SQLite;
using Xamarin.Forms;

namespace GestMembrosSUD.ViewModels
{
    public class MemberViewModel
    {
        //private MemberItemViewModel memberItemViewModel;
        #region Properties
        string dbPath;
        SQLiteConnection db;
        public Member Member
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public MemberViewModel()
        {
        }

        public MemberViewModel(Member member)
        {
            this.Member = member;
        }
        #endregion

        #region Commands
        public ICommand EditMemberCommand
        {
            get
            {
                return new RelayCommand(Update);
            }
        }

        private async void Update()
        {
            MainViewModel.GetInstance().UpdateMember = new UpdateMemberViewModel(this.Member);
            await Application.Current.MainPage.Navigation.PushAsync(new UpdateMemberPage());
        }

        public ICommand DeleteUserCommand
        {
            get
            {
                return new RelayCommand(Delete);
            }
        }
        #endregion

        #region methods
        private async void Delete()
        {
            var source = await Application.Current.MainPage.DisplayActionSheet(
                    "Delete",
                "Are you sure, to delete "+this.Member.Name+"?",
                    null,
                    "Yes",
                    "No");
            if (source == "No")
            {
                return;
            }
            if(source == "Yes")
            {
                dbPath = Path.Combine(
                 Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                 "MembersDB.db3");

                db = new SQLiteConnection(dbPath);

                if (db.Table<Member>().Count() == 0)
                {
                    db.CreateTable<Member>();
                }

                db.Delete<Member>(this.Member.Id);

                await Application.Current.MainPage.DisplayAlert(
                        "Done",
                    "Member " + this.Member.Id + " Deleted!",
                        "Accept");

                MainViewModel.GetInstance().Members = new MembersViewModel();
                await Application.Current.MainPage.Navigation.PushAsync(new MembersPage());
            }

        }
        #endregion
    }
}
