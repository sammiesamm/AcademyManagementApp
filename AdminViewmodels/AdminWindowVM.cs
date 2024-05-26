using AcademyManager.AdminViews;
using AcademyManager.Viewmodels;
using System.Windows.Controls;
using System.Windows.Input;

namespace AcademyManager.AdminViewmodels
{
    public class AdminWindowVM : BaseViewModel
    {
        #region Commands
        public ICommand CloseCommand { get; set; }
        public ICommand AddCoursesCommand { get; set; }
        public ICommand AddUsersCommand { get; set; }
        public ICommand SearchUsersCommand { get; set; }
        #endregion
        #region Properties
        private AddScheduleUC _addtermsView;
        private AddUsersUC _addUsersView;
        private SearchUserUC _searchUsersView;
        private UserControl _currentView;
        private string _currentUrl;
        private bool _addcourses;
        private bool _addusers;
        private bool _searchusers;
        public UserControl CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        public bool AddCourses
        {
            get { return _addcourses; }
            set { _addcourses = value; OnPropertyChanged(); }
        }
        public bool AddUsers
        {
            get { return _addusers; }
            set { _addusers = value; OnPropertyChanged(); }
        }
        public bool SearchUsers
        {
            get { return _searchusers; }
            set { _searchusers = value; OnPropertyChanged(); }
        }
        public string CurrentUrl
        {
            get { return _currentUrl; }
            set { _currentUrl = value; OnPropertyChanged(); }
        }
        #endregion
        #region Methods
        private void InitializeCommands()
        {
            CloseCommand = new RelayCommand<AdminWindow>(p => { return true; }, p =>
            {
                p.Close();
            });

            AddCoursesCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                AddUsers = false;
                SearchUsers = false;
                CurrentUrl = "Quản trị viên" + " > Thêm lịch trình";
                CurrentView = _addtermsView;
            });

            AddUsersCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                AddCourses = false;
                SearchUsers = false;
                CurrentUrl = "Quản trị viên" + " > Thêm người dùng";
                if (_addUsersView == null)
                {
                    _addUsersView = new AddUsersUC();
                    CurrentView = _addUsersView;
                }
                else CurrentView = _addUsersView;

            });

            SearchUsersCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                AddUsers = false;
                AddCourses = false;
                CurrentUrl = "Quản trị viên" + " > Truy xuất người dùng";
                if (_searchUsersView == null)
                {
                    _searchUsersView = new SearchUserUC();
                    CurrentView = _searchUsersView;
                }
                else CurrentView = _searchUsersView;
            });
        }
        #endregion
        public AdminWindowVM()
        {
            if (_addtermsView == null)
            {
                _addtermsView = new AddScheduleUC();
                CurrentView = _addtermsView;
            }
            else
            {
                CurrentView = _addtermsView;
            }
            AddCourses = true;
            AddUsers = false;
            SearchUsers = false;
            CurrentUrl = "Quản trị viên" + " > Thêm lịch trình";
            InitializeCommands();
        }
    }
}
