using AcademyManager.Models;
using AcademyManager.Viewmodels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace AcademyManager.AdminViewmodels
{
    public class SearchUserVM : BaseViewModel
    {
        #region Support Classes
        public class StudentDataRecord
        {
            public string CourseID { get; set; }
            public string CourseName { get; set; }
            public double Daily { get; set; }
            public double Project { get; set; }
            public double MidTerm { get; set; }
            public double Final { get; set; }
            public double GPA { get; set; }
            public StudentDataRecord(string cid, string cname, double d, double p, double midTerm, double final, double gpa)
            {
                CourseID = cid;
                CourseName = cname;
                Daily = d;
                Project = p;
                MidTerm = midTerm;
                Final = final;
                GPA = gpa;
            }
        }

        public class InsDataRecord
        {
            public string CourseID { get; set; }
            public string CourseName { get; set; }
            public string ClassID { get; set; }
            public DayOfWeek Workday { get; set; }
            public TimeOnly BeginTime { get; set; }
            public TimeOnly EndTime { get; set; }
            public InsDataRecord(string cid, string cname, string clid, DayOfWeek day, TimeOnly bgt, TimeOnly et)
            {
                CourseID = cid;
                CourseName = cname;
                ClassID = clid;
                Workday = day;
                BeginTime = bgt;
                EndTime = et;
            }
        }
        #endregion

        #region Commands
        public ICommand SearchCommand { get; set; }
        #endregion

        #region Properties
        private ObservableCollection<StudentDataRecord> _students;
        private ObservableCollection<InsDataRecord> _inss;
        private int _selectedIdx;
        private string _id;
        private string _userid;
        private string _fullname;
        private string _email;
        private string _birthday;
        private string _faculty;
        private string _avatar;
        private string _add1;
        private string _add2;
        private string _tabheader;
        private Visibility _add2V;
        private Visibility _loading;
        private Visibility _dataV;
        private Visibility _notfound;
        private Visibility _std;
        private Visibility _ins;
        public ObservableCollection<StudentDataRecord> Students
        {
            get { return _students; }
            set { _students = value; OnPropertyChanged(); }
        }
        public ObservableCollection<InsDataRecord> Instructors
        {
            get { return _inss; }
            set { _inss = value; OnPropertyChanged(); }
        }
        public Visibility StudentV
        {
            get => _std;
            set { _std = value; OnPropertyChanged(); }
        }
        public Visibility InsV
        {
            get => _ins;
            set { _ins = value; OnPropertyChanged(); }
        }
        public Visibility Addition2V
        {
            get { return _add2V; }
            set { _add2V = value; OnPropertyChanged(); }
        }
        public Visibility Loading
        {
            get { return _loading; }
            set { _loading = value; OnPropertyChanged(); }
        }
        public Visibility NotFound
        {
            get { return _notfound; }
            set { _notfound = value; OnPropertyChanged(); }
        }
        public Visibility DataV
        {
            get { return _dataV; }
            set { _dataV = value; OnPropertyChanged(); }
        }
        public string TabHeader
        {
            get { return _tabheader; }
            set { _tabheader = value; OnPropertyChanged(); }
        }
        public int SelectedIdx
        {
            get { return _selectedIdx; }
            set { _selectedIdx = value; OnPropertyChanged(); }
        }
        public string ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }
        public string UserID
        {
            get { return _userid; }
            set { _userid = value; OnPropertyChanged(); }
        }
        public string Fullname
        {
            get { return _fullname; }
            set { _fullname = value; OnPropertyChanged(); }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }
        public string Birthday
        {
            get { return _birthday; }
            set { _birthday = value; OnPropertyChanged(); }
        }
        public string Faculty
        {
            get { return _faculty; }
            set { _faculty = value; OnPropertyChanged(); }
        }
        public string Avatar
        {
            get { return _avatar; }
            set { _avatar = value; OnPropertyChanged(); }
        }
        public string Addition1
        {
            get { return _add1; }
            set { _add1 = value; OnPropertyChanged(); }
        }
        public string Addition2
        {
            get { return _add2; }
            set { _add2 = value; OnPropertyChanged(); }
        }
        #endregion 

        #region Methods
        private async Task<List<StudentDataRecord>> GetStudentResultData(StudentUser user)
        {
            List<StudentDataRecord> list = new List<StudentDataRecord>();
            DatabaseManager db = new DatabaseManager();
            foreach (ClassIdentifier cls in user.StudyElements)
            {
                Course c = await db.GetCourseAsync(cls.TermID, cls.CourseID);
                double daily = c.Classes[cls.ClassID].Students[ID].DailyTestScore;
                double prj = c.Classes[cls.ClassID].Students[ID].Project;
                double mid = c.Classes[cls.ClassID].Students[ID].Mid_Term;
                double final = c.Classes[cls.ClassID].Students[ID].Final;
                double gpa = c.Classes[cls.ClassID].Students[ID].GPA;
                list.Add(new StudentDataRecord(c.CourseID, c.CourseName, daily, prj, mid, final, gpa));
            }
            return list;
        }
        private async Task<List<InsDataRecord>> GetInsData(InstructorUser user)
        {
            List<InsDataRecord> list = new List<InsDataRecord>();
            DatabaseManager db = new DatabaseManager();
            foreach (ClassIdentifier cls in user.StudyElements)
            {
                Course c = await db.GetCourseAsync(cls.TermID, cls.CourseID);
                DayOfWeek day = c.Classes[cls.ClassID].Weekday;
                TimeOnly bgt = c.Classes[cls.ClassID].BeginTime;
                TimeOnly et = c.Classes[cls.ClassID].EndTime;
                list.Add(new InsDataRecord(c.CourseID, c.CourseName, cls.ClassID, day, bgt, et));
            }
            return list;
        }
        private async void LoadStudentDataGrid(StudentUser user)
        {
            StudentV = Visibility.Visible;
            InsV = Visibility.Hidden;
            var data = await GetStudentResultData(user);
            Students = [.. data];
        }
        private async void LoadInsDataGrid(InstructorUser user)
        {
            StudentV = Visibility.Hidden;
            InsV = Visibility.Visible;
            var data = await GetInsData(user);
            Instructors = [.. data];
        }
        private void LoadStudentPersonalInfo(StudentUser user)
        {
            Addition2V = Visibility.Hidden;
            Avatar = user.AvatarBase64;
            UserID = $"Mã số sinh viên: {user.ID}";
            Fullname = $"Họ và tên: {user.Fullname}";
            Email = $"Email: {user.Email}";
            string birthday = user.Birthday.ToString("dd/MM/yyyy");
            Birthday = $"Ngày sinh: {birthday}";
            Faculty = $"Khoa: {user.Faculty}";
            Addition1 = $"Ngành: {user.Major}";
        }
        private void LoadInsPersonalInfo(InstructorUser user)
        {
            Addition2V = Visibility.Visible;
            Avatar = user.AvatarBase64;
            UserID = $"Mã số giảng viên: {user.ID}";
            Fullname = $"Họ và tên: {user.Fullname}";
            Email = $"Email: {user.Email}";
            string birthday = user.Birthday.ToString("dd/MM/yyyy");
            Birthday = $"Ngày sinh: {birthday}";
            Faculty = $"Khoa: {user.Faculty}";
            Addition1 = $"Bằng cấp: {user.Certificate}";
            Addition2 = $"Chuyên môn: {user.Speciality}";
        }
        private void InitializeCommands()
        {
            SearchCommand = new RelayCommand<object>(p => { return true; }, async p =>
            {
                if (SelectedIdx == -1 || ID == null || ID == String.Empty) return;
                if (SelectedIdx == 0)
                {
                    DatabaseManager db = new DatabaseManager();
                    Loading = Visibility.Visible;
                    NotFound = Visibility.Hidden;
                    StudentUser user = await db.GetStudentAsync(ID);
                    if (user == null)
                    {
                        NotFound = Visibility.Visible;
                        DataV = Visibility.Hidden;
                        Loading = Visibility.Hidden;
                        return;
                    }
                    DataV = Visibility.Visible;
                    NotFound = Visibility.Hidden;
                    LoadStudentPersonalInfo(user);
                    LoadStudentDataGrid(user);
                    TabHeader = "Kết quả học tập";
                    Loading = Visibility.Hidden;

                }
                else if (SelectedIdx == 1)
                {
                    DatabaseManager db = new DatabaseManager();
                    Loading = Visibility.Visible;
                    NotFound = Visibility.Hidden;
                    InstructorUser user = await db.GetInstructorAsync(ID);
                    if (user == null)
                    {
                        NotFound = Visibility.Visible;
                        DataV = Visibility.Hidden;
                        Loading = Visibility.Hidden;
                        return;
                    }
                    DataV = Visibility.Visible;
                    NotFound = Visibility.Hidden;
                    LoadInsPersonalInfo(user);
                    LoadInsDataGrid(user);
                    TabHeader = "Các lớp giảng dạy";
                    Loading = Visibility.Hidden;
                }
            });
        }
        #endregion
        public SearchUserVM()
        {
            InitializeCommands();
            DataV = Visibility.Hidden;
            Loading = Visibility.Hidden;
            NotFound = Visibility.Hidden;
        }
    }
}