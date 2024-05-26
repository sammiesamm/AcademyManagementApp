using AcademyManager.Models;
using AcademyManager.UCViews;
using AcademyManager.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AcademyManager.Viewmodels
{
    public class SubjectListVM : BaseViewModel
    {
        #region Commands
        public ICommand BackCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        #endregion
        #region Properties
        private MainVM ParentVM { get; set; }
        private WrapPanel SubjList { get; set; }
        private string _noti;
        private Visibility _notiV;
        public string Noti
        {
            get { return _noti; }
            set { _noti = value; OnPropertyChanged(); }
        }
        public Visibility NotiV
        {
            get { return _notiV; }
            set { _notiV = value; OnPropertyChanged(); }
        }
        #endregion
        #region Methods
        public void LoadClasses()
        {
            SubjList.Children.Clear();
            List<Class> list = MainVM.UserClassList;
            if (list.Count == 0)
            {
                if (MainVM.CurrentAccount.Type == 1)
                {
                    Noti = "Bạn chưa được phân công giảng dạy.";
                    NotiV = Visibility.Visible;
                }
                else
                {
                    Noti = "Bạn chưa đăng ký khóa học nào.";
                    NotiV = Visibility.Visible;
                }
                return;
            }
            else NotiV = Visibility.Hidden;

            foreach (Class c in list)
            {
                SubjectUC item = new SubjectUC(ParentVM, this, c);
                SubjList.Children.Add(item);
            }
        }
        private void InitializeCommands()
        {
            BackCommand = new RelayCommand<object>(p => true, p =>
            {
                ParentVM.HomeNavigateCommand.Execute(null);
            });

            RegisterCommand = new RelayCommand<object>(p => true, p =>
            {
                ParentVM.CourseRegisterView = new SubjectRegister(ParentVM, this);
                ParentVM.CurrentView = ParentVM.CourseRegisterView;
            });
        }
        #endregion
        public SubjectListVM(MainVM vm, WrapPanel list)
        {
            ParentVM = vm;
            SubjList = list;
            NotiV = Visibility.Hidden;
            LoadClasses();
            InitializeCommands();
        }
    }
}
