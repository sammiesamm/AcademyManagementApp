using AcademyManager.Views;
using System.Windows.Input;

namespace AcademyManager.Viewmodels
{
    public class WelcomeScreenVM : BaseViewModel
    {
        #region Commands
        public ICommand LoginCommand { get; set; }
        public ICommand SigninCommand { get; set; }
        public ICommand BackCommand { get; set; }
        #endregion
        #region Properties
        private MainVM ParentVM { get; set; }
        #endregion
        #region Methods
        private void InitializeCommands()
        {
            LoginCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                if (ParentVM.LoginView == null)
                {
                    ParentVM.LoginView = new Login(ParentVM);
                    ParentVM.CurrentView = ParentVM.LoginView;
                }
                else
                    ParentVM.CurrentView = ParentVM.LoginView;
            });

            SigninCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                if (ParentVM.SigninView == null)
                {
                    ParentVM.SigninView = new SetNewPass(ParentVM);
                    ParentVM.CurrentView = ParentVM.SigninView;
                }
                else
                    ParentVM.CurrentView = ParentVM.SigninView;
            });

            BackCommand = new RelayCommand<object>(p => true, p =>
            {
                ParentVM.CurrentView = ParentVM.WhoAreYouView;
            });
        }
        #endregion
        public WelcomeScreenVM(MainVM parentVM)
        {
            InitializeCommands();
            ParentVM = parentVM;
        }
    }
}
