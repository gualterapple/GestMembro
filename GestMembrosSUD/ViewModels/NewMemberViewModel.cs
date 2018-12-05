using System;
using System.IO;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GestMembrosSUD.Helpers;
using GestMembrosSUD.Models;
using GestMembrosSUD.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SQLite;
using Xamarin.Forms;

namespace GestMembrosSUD.ViewModels
{
    public class NewMemberViewModel:BaseViewModel
    {
        #region Attributes

        private bool isRunning;
        private bool isEnabled;
        private ImageSource photo;
        private MediaFile file;

        private int id;
        private string name;
        private string capela;
        private string address;
        private int age;

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
        public NewMemberViewModel()
        {
            this.Photo = "photo1";
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

        public ICommand NewMemberCommand
        {
            get
            {
                return new RelayCommand(Save);
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
                    this.isRunning = true;

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
                    this.isRunning = true;
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
            this.isRunning = false;
        }

        public async void Save()
        {
            this.isRunning = true;

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            dbPath = Path.Combine(
                 Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                 "MembersDB.db3");

            db = new SQLiteConnection(dbPath);
            db.CreateTable<Member>();

            Member x = new Member();
                x.Name = Name;
                x.Address = Address;
                x.Capela = Capela;
                x.Age = age;
                x.Photo = file.Path;

            db.Insert(x);
            var table = db.Table<Member>();
            await Application.Current.MainPage.DisplayAlert(
                    "Done",
                    "Member "+ Name + " inserted!",
                    "Accept");

            this.isRunning = false;

            MainViewModel.GetInstance().Members = new MembersViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new MembersPage());
        }
        public async void Update()
        {


            dbPath = Path.Combine(
                 Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                 "MembersDB.db3");

            db = new SQLiteConnection(dbPath);
            db.CreateTable<Member>();

            Member x = new Member();
            x.Name = Name;
            x.Address = Address;
            x.Capela = Capela;
            x.Age = age;
            x.Photo = "photo1";

            db.Execute("Update [Member] set [Name] = 'Nox1' where [_Id] = 3");
            var table = db.Table<Member>();
            await Application.Current.MainPage.DisplayAlert(
                    "Done",
                    "Member " + Name + " updated!",
                    "Accept");
        }
        #endregion

    }
}
