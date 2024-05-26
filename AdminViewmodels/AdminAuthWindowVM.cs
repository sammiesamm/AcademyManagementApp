using AcademyManager.AdminViews;
using AcademyManager.Viewmodels;
using System.Windows.Controls;
using System.Windows.Input;

namespace AcademyManager.AdminViewmodels
{
    public class AdminAuthWindowVM : BaseViewModel
    {
        #region Commands
        public ICommand CloseCommand { get; set; }
        public ICommand SigninTabCommand { get; set; }
        public ICommand LoginTabCommand { get; set; }
        #endregion

        #region Properties
        private UserControl _current;
        private AdminLoginUC _loginUC;
        private AdminSigninUC _signinUC;
        private bool _atLoginTab;
        private bool _atSigninTab;
        public bool AtLoginTab
        {
            get { return _atLoginTab; }
            set { _atLoginTab = value; OnPropertyChanged(); }
        }
        public bool AtSigninTab
        {
            get { return _atSigninTab; }
            set { _atSigninTab = value; OnPropertyChanged(); }
        }
        public UserControl CurrentView
        {
            get { return _current; }
            set { _current = value; OnPropertyChanged(); }
        }
        #endregion

        #region Methods
        private void InitializeCommands()
        {
            CloseCommand = new RelayCommand<AdminAuthWindow>(p => { return true; }, p =>
            {
                p.Close();
            });

            SigninTabCommand = new RelayCommand<object>(p => { return AtLoginTab; }, p =>
            {
                AtSigninTab = true;
                AtLoginTab = false;
                if (_signinUC != null) CurrentView = _signinUC;
                else
                {
                    _signinUC = new AdminSigninUC();
                    CurrentView = _signinUC;
                }
            });

            LoginTabCommand = new RelayCommand<object>(p => { return AtSigninTab; }, p =>
            {
                AtLoginTab = true;
                AtSigninTab = false;
                if (_loginUC != null) CurrentView = _loginUC;
                else
                {
                    _loginUC = new AdminLoginUC();
                    CurrentView = _loginUC;
                }
            });
        }
        #endregion
        public AdminAuthWindowVM()
        {
            InitializeCommands();
            AtLoginTab = true;
            AtSigninTab = false;
            if (_loginUC != null) CurrentView = _loginUC;
            else
            {
                _loginUC = new AdminLoginUC();
                CurrentView = _loginUC;
            }
        }
    }
}
