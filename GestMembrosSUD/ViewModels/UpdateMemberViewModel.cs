using System;
using System.IO;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GestMembrosSUD.Models;
using GestMembrosSUD.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
        private ImageSource photo;
        private MediaFile file;
        private bool isRunning;

        private Member member;

        string dbPath;
        SQLiteConnection db;

        #endregion

        #region Properties
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        } 

        public ImageSource Photo
        {
            get { return this.photo; }
            set { SetValue(ref this.photo, value); }
        } 
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
        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }

        public ICommand UpdateMemberCommand
        {
            get
            {
                return new RelayCommand(Update);
            }
        }
        #endregion

        #region Methods
        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsCameraAvailable &&
                CrossMedia.Current.IsTakePhotoSupported)
            {
                var source = await Application.Current.MainPage.DisplayActionSheet(
                    "Do you want to import from camera ou gallery",
                    "Cancel",
                    null,
                    "gallery",
                    "camera");

                if (source == "Cancel")
                {
                    this.file = null;
                    return;
                }

                if (source == "camera")
                {
                    this.file = await CrossMedia.Current.TakePhotoAsync(
                        new StoreCameraMediaOptions
                        {
                            Directory = "Sample",
                            Name = "test.jpg",
                            PhotoSize = PhotoSize.Small,
                        }
                    );
                }
                else
                {
                    this.file = await CrossMedia.Current.PickPhotoAsync();
                }
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.Photo = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
            Console.WriteLine("Caminho da foto: " + file.Path);
            Console.WriteLine("ImageSource.FromStream da foto: " + this.Photo);
            Console.WriteLine("file.GetStream() da foto: " + file.GetStream());
        }
        public async void Update()
        {


            dbPath = Path.Combine(
                 Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                 "MembersDB.db3");

            db = new SQLiteConnection(dbPath);
            //db.CreateTable<Member>();

            db.Execute("Update [Member] set [Name] = '"+ Name + "', [Capela] = '" + Capela + "'," +
                       " [Address] = '"+ Address +"', [Age] = '" + Age + "', [Photo] = '" + file.Path +"'   where [_Id]= '" + Id + "'");
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
