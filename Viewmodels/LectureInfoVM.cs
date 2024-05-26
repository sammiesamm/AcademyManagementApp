using AcademyManager.Models;
using AcademyManager.Views;
using System.Windows.Input;

namespace AcademyManager.Viewmodels
{
    public class LectureInfoVM : BaseViewModel
    {
        #region Commands
        public ICommand UpdateInfoCommand { get; set; }
        public ICommand BackCommand { get; set; }
        #endregion

        #region Properties
        private MainVM ParentVM { get; set; }
        private string _fullname;
        private string _id;
        private string _email;
        private string _faculty;
        private string _spec;
        private string _cert;
        private string _birthday;
        private string _avatar;
        public string Certificate
        {
            get { return _cert; }
            set { _cert = value; OnPropertyChanged(); }
        }
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
        public string Speciality
        {
            get { return _spec; }
            set { _spec = value; OnPropertyChanged(); }
        }
        #endregion

        #region Methods
        private void LoadInfo()
        {
            InstructorUser? user = MainVM.CurrentUser as InstructorUser;
            if (user != null)
            {
                Fullname = user.Fullname;
                ID = user.ID;
                Email = user.Email;
                Birthday = user.Birthday.ToString("dd/MM/yyyy");
                Faculty = user.Faculty;
                Speciality = user.Speciality;
                Certificate = user.Certificate;
                Avatar = user.AvatarBase64;
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
        public LectureInfoVM(MainVM vm)
        {
            ParentVM = vm;
            LoadInfo();
            InitializeCommand();
        }
    }
}
