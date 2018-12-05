using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GestMembrosSUD.Models;
using GestMembrosSUD.Services;
using SQLite;
using Xamarin.Forms;

namespace GestMembrosSUD.ViewModels
{
    public class MembersViewModel : BaseViewModel
    {

        #region Services
        private ApiService apiService;
        #endregion

        #region atributes
        private string filter;
        private bool isRefreshing;
        private ObservableCollection<MemberItemViewModel> members;
        private List<Member> memberList;
        #endregion

        #region properties
        public string Filter
        {
            get { return this.filter; }
            set 
            { 
                SetValue(ref this.filter, value);
                this.Search();
            }
        }
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        public ObservableCollection<MemberItemViewModel> Members
        {
            get { return this.members; }
            set { SetValue(ref this.members, value); }
        }

        #endregion

        #region Constructors
        public MembersViewModel()
        {
            this.apiService = new ApiService();
            this.LoadMembers();
        }
        #endregion

        #region Methods

        private void LoadMembers()
        {

            this.IsRefreshing = true;

            /*var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert
                 ("Error",
                  connection.Message,
                 "Accept");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }*/
            string dbPath;
            SQLiteConnection db;

            dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Personal),
            "MembersDB.db3");

            db = new SQLiteConnection(dbPath);
            db.CreateTable<Member>();
            var table = db.Table<Member>();
            this.memberList = new List<Member>();
            foreach (var s in table)
            {
                this.memberList.Add(s);
                Console.WriteLine("Id: "+s.Id," Nome: " + s.Name);
            }
            this.Members = new ObservableCollection<MemberItemViewModel>(ToMemberItemViewModel());
            this.IsRefreshing = false;
        }

        #region Methods
        private async void NewUser()
        {
            MainViewModel.GetInstance().NewMember = new NewMemberViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new Views.NewMemberPage());
        }

        private IEnumerable<MemberItemViewModel>ToMemberItemViewModel()
        {
            return this.memberList.Select(m => new MemberItemViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Address = m.Address,
                Age = m.Age,
                Capela = m.Capela,
                Photo = m.Photo
            });
        }
        #endregion

        #endregion
        #region Commands
        public ICommand NewUserCommand
        {
            get
            {
                return new RelayCommand(NewUser);

            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadMembers);
            }
        }
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(Search);
            }
        }

        private async void Search()
        {
            if(string.IsNullOrEmpty(Filter))
            {
                this.Members = new ObservableCollection<MemberItemViewModel>(ToMemberItemViewModel());
            }
            else
            {
                this.Members = new ObservableCollection<MemberItemViewModel>
                    (this.ToMemberItemViewModel().Where(m=>m.Name.ToLower().Contains(this.Filter.ToLower()) || m.Capela.ToLower().Contains(this.Filter.ToLower())));
                /*await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    this.Filter,
                    "Accept");*/
            }

        }
        #endregion


    }
}
