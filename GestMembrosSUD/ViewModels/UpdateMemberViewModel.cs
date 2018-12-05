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
    public class UpdateMemberViewModel: BaseViewModel
    {
        #region Attributes
        private int id;
        private string name;
        private string capela;
        private string address;
        private int age;

        private Member member;

        string dbPath;
        SQLiteConnection db;

        #endregion

        #region Properties
        public Member Member
        {
            get { return this.member; }
            set { SetValue(ref this.member, value); }
        }
        public int Id
        {
            get { return this.id; }
            set { SetValue(ref this.id, value); }
        }
        public string Name
        {
            get { return this.name; }
            set { SetValue(ref this.name, value); }
        }
        public string Capela
        {
            get { return this.capela; }
            set { SetValue(ref this.capela, value); }
        }
        public string Address
        {
            get { return this.address; }
            set { SetValue(ref this.address, value); }
        }
        public int Age
        {
            get { return this.age; }
            set { SetValue(ref this.age, value); }
        }

        #endregion

        #region Constructor
        public UpdateMemberViewModel()
        {

        }
        public UpdateMemberViewModel(Member m)
        {
            this.Member = m;
            this.Id = m.Id;
            this.Name = m.Name;
            this.Address = m.Address;
            this.Capela = m.Capela;
            this.Age = m.Age;            
        }

        #endregion

        #region Commands
        public ICommand UpdateMemberCommand
        {
            get
            {
                return new RelayCommand(Update);
            }
        }
        #endregion

        #region Methods
        public async void Update()
        {


            dbPath = Path.Combine(
                 Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                 "MembersDB.db3");

            db = new SQLiteConnection(dbPath);
            //db.CreateTable<Member>();

            db.Execute("Update [Member] set [Name] = '"+ Name + "', [Capela] = '" + Capela + "'," +
                       " [Address] = '"+ Address +"', [Age] = '" + Age + "'   where [_Id]= '" + Id + "'");
            var table = db.Table<Member>();
            await Application.Current.MainPage.DisplayAlert(
                    "Done",
                    "Member updated!",
                    "Accept");

            MainViewModel.GetInstance().Members = new MembersViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new MembersPage());

        }
        #endregion
    }
}
