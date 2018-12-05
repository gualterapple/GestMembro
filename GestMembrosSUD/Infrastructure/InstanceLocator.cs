using System;
namespace GestMembrosSUD.Infrastructure
{
    using ViewModels;
    public class InstanceLocator
    {
        #region MyRegion
        public MainViewModel Main
        {
            get;
            set;
        }
        #endregion

        #region MyRegion
        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        }
        #endregion
    }
}
