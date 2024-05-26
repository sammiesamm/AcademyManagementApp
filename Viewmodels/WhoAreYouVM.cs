using AcademyManager.AdminViews;
using AcademyManager.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AcademyManager.Viewmodels
{
    public class WhoAreYouVM : BaseViewModel
    {
        #region Commands
        public ICommand InstructorCommand { get; set; }
        public ICommand StudentCommand { get; set; }
        public ICommand AdminCommand { get; set; }
        #endregion
        #region Properties
        private MainVM ParentVM { get; set; }
        #endregion
        #region Methods
        FrameworkElement GetWindowParent(UserControl p)
        {
            FrameworkElement parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }

            return parent;
        }
        private void InitializeCommand()
        {
            InstructorCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                MainVM.Type = 1;
                if (ParentVM.WelcomeView == null)
                {
                    ParentVM.WelcomeView = new WelcomeScreen(ParentVM);
                    ParentVM.CurrentView = ParentVM.WelcomeView;
                }
                else
                    ParentVM.CurrentView = ParentVM.WelcomeView;
            });

            StudentCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                MainVM.Type = 2;
                if (ParentVM.WelcomeView == null)
                {
                    ParentVM.WelcomeView = new WelcomeScreen(ParentVM);
                    ParentVM.CurrentView = ParentVM.WelcomeView;
                }
                else
                    ParentVM.CurrentView = ParentVM.WelcomeView;
            });

            AdminCommand = new RelayCommand<WhoAreYou>(p => { return true; }, p =>
            {
                var w = GetWindowParent(p);
                Window main = w as Window;
                if (main != null)
                {
                    AdminAuthWindow adm = new AdminAuthWindow();
                    main.Hide();
                    adm.ShowDialog();
                    main.Show();
                }
            });

        }
        #endregion
        public WhoAreYouVM(MainVM vm)
        {
            ParentVM = vm;
            InitializeCommand();
        }
    }
}
