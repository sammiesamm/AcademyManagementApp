using AcademyManager.Models;
using AcademyManager.Views;
using System.Windows.Input;

namespace AcademyManager.Viewmodels
{
    public class StudentInfoVM : BaseViewModel
    {
        #region Commands
        public ICommand BackCommand { get; set; }
        #endregion
        #region Properties
        private MainVM ParentVM { get; set; }
        private string _fullname;
        private string _birthday;
        private string _id;
        private string _email;
        private string _faculty;
        private string _major;
        private string _avatar;
        public string Birthday
        {
            get { return _birthday; }
            set { _birthday = value; OnPropertyChanged(); }
        }
        public string Avatar
        {
            get { return _avatar; }
            set { _avatar = value; OnPropertyChanged(); }
        }
        public string Fullname
        {
            get { return _fullname; }
            set { _fullname = value; OnPropertyChanged(); }
        }
        public string ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }
        public string Faculty
        {
            get { return _faculty; }
            set { _faculty = value; OnPropertyChanged(); }
        }
        public string Major
        {
            get { return _major; }
            set { _major = value; OnPropertyChanged(); }
        }
        #endregion
        #region Methods
        private void LoadInfo()
        {
            StudentUser? user = MainVM.CurrentUser as StudentUser;
            if (user != null)
            {
                Fullname = user.Fullname;
                ID = user.ID;
                Email = user.Email;
                Faculty = user.Faculty;
                Major = user.Major;
                Avatar = user.AvatarBase64;
                Birthday = user.Birthday.ToString("dd/MM/yyyy");
            }
        }
        private void InitializeCommand()
        {
            BackCommand = new RelayCommand<MainWindow>(p => { return true; }, p =>
            {
                ParentVM.HomeNavigateCommand.Execute(null);
            });
        }
        #endregion
        public StudentInfoVM(MainVM vm)
        {
            ParentVM = vm;
            LoadInfo();
            InitializeCommand();
        }
    }
}
