using System;
namespace GestMembrosSUD.ViewModels
{
    public class MainViewModel
    {
        #region ViewModels
        public LoginViewModel Login
        {
            get;
            set;
        }
        public MembersViewModel Members
        {
            get;
            set;
        }

        public MemberViewModel Member
        {
            get;
            set;
        }
        public NewMemberViewModel NewMember
        {
            get;
            set;
        }
        public UpdateMemberViewModel UpdateMember
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            this.Login = new LoginViewModel();
        }
        #endregion

        #region Singleton1
        private static MainViewModel instance;
        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        #endregion

    }
}
