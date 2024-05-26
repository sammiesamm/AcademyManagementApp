using AcademyManager.Models;
using AcademyManager.Viewmodels;
using Flattinger.Core.Theme;
using Flattinger.UI.ToastMessage;
using Flattinger.UI.ToastMessage.Controls;
using Microsoft.Win32;
using OfficeOpenXml;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AcademyManager.AdminViewmodels
{
    public class AddScheduleVM : BaseViewModel
    {
        #region Commands
        public ICommand UploadCommand { get; set; }
        public ICommand BrowseCommand { get; set; }
        public ICommand DownloadCommand { get; set; }
        #endregion

        #region Properties
        private AppTheme _theme;
        private ToastProvider _toastProvider;
        private bool _inProcess;
        private string _path;
        private Visibility _loading;
        public Visibility Loading
        {
            get { return _loading; }
            set { _loading = value; OnPropertyChanged(); }
        }
        public string Path
        {
            get { return _path; }
            set { _path = value; OnPropertyChanged(); }
        }
        #endregion

        #region Methods
        private DayOfWeek StringToDayOfWeek(string vietnameseDay)
        {
            var vietnameseToEnglishDayMapping = new Dictionary<string, string>
            {
                {"Thứ Hai", "Monday"},
                {"Thứ Ba", "Tuesday"},
                {"Thứ Tư", "Wednesday"},
                {"Thứ Năm", "Thursday"},
                {"Thứ Sáu", "Friday"},
                {"Thứ Bảy", "Saturday"},
                {"Chủ Nhật", "Sunday"}
            };
            if (vietnameseToEnglishDayMapping.TryGetValue(vietnameseDay, out string englishDay))
            {
                // Parse English day name to DayOfWeek enum
                if (Enum.TryParse<DayOfWeek>(englishDay, true, out DayOfWeek dayOfWeek))
                {
                    return dayOfWeek;
                }
            }
            throw new Exception("Cannot convert");
        }
        private TimeOnly StringToTimeOnly(string s)
        {
            if (TimeOnly.TryParseExact(s, "HH:mm", null, System.Globalization.DateTimeStyles.None, out TimeOnly time))
            {
                return time;
            }
            else
            {
                throw new Exception("Cannot convert");
            }
        }
        private DateOnly StringToDateOnly(string s)
        {
            if (DateOnly.TryParseExact(s, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateOnly date))
            {
                return date;
            }
            else
            {
                throw new Exception("Cannot convert");
            }
        }
        private bool NullOrEmpty(string s)
        {
            return s == null || s.Length == 0;
        }
        private bool InvalidClass(Class c1, Class c2)
        {
            if (c1.ClassID != c2.ClassID)
            {
                if (c1.InstructorID != c2.InstructorID) return false;
                else
                {
                    bool manage2class1time = (c1.Weekday == c2.Weekday) && ((c1.BeginTime >= c2.BeginTime && c1.EndTime <= c2.EndTime) ||
                        (c1.BeginTime <= c2.BeginTime && c1.EndTime >= c2.EndTime));
                    if (manage2class1time) return true;
                }
            }
            return false;
        }
        private List<Term>? GetDataFromExcel(out List<KeyValuePair<string, Class>>? list)
        {
            List<Term> data = new List<Term>();
            List<KeyValuePair<string, Class>> insSchedule = new List<KeyValuePair<string, Class>>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(_path)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;
                if (colCount != 16)
                {
                    list = null;
                    return null;
                }

                for (int row = 2; row <= rowCount; row++)
                {
                    // read data as string
                    string? termID = worksheet.Cells[row, 1].Value.ToString();
                    string? courseID = worksheet.Cells[row, 2].Value.ToString();
                    string? courseName = worksheet.Cells[row, 3].Value.ToString();
                    string? crd = worksheet.Cells[row, 4].Value.ToString();
                    string? classID = worksheet.Cells[row, 5].Value.ToString();
                    string? insID = worksheet.Cells[row, 6].Value.ToString();
                    string? insName = worksheet.Cells[row, 7].Value.ToString();
                    string? room = worksheet.Cells[row, 8].Value.ToString();
                    string? exr = worksheet.Cells[row, 15].Value.ToString();

                    // Check string input
                    if (NullOrEmpty(termID) || NullOrEmpty(courseID) || NullOrEmpty(courseName) || NullOrEmpty(classID)
                        || NullOrEmpty(insID) || NullOrEmpty(room) || NullOrEmpty(insName) || NullOrEmpty(exr))
                    {
                        list = null;
                        return null;
                    }

                    string? day = worksheet.Cells[row, 9].Value.ToString();
                    string? bgt = worksheet.Cells[row, 10].Value.ToString();
                    string? et = worksheet.Cells[row, 11].Value.ToString();
                    string? bgd = worksheet.Cells[row, 12].Value.ToString();
                    string? ed = worksheet.Cells[row, 13].Value.ToString();
                    string? exd = worksheet.Cells[row, 14].Value.ToString();
                    string? ext = worksheet.Cells[row, 16].Value.ToString();

                    // try to convert some data to correct type
                    DayOfWeek dayOfWeek;
                    TimeOnly beginTime;
                    TimeOnly endTime;
                    DateOnly beginDate;
                    DateOnly endDate;
                    DateOnly examDate;
                    TimeOnly examTime;
                    int credits;

                    try
                    {
                        dayOfWeek = StringToDayOfWeek(day);
                        beginTime = StringToTimeOnly(bgt);
                        endTime = StringToTimeOnly(et);
                        examTime = StringToTimeOnly(ext);
                        beginDate = StringToDateOnly(bgd);
                        endDate = StringToDateOnly(ed);
                        examDate = StringToDateOnly(exd);
                        credits = Convert.ToInt32(crd);
                    }
                    catch
                    {
                        list = null;
                        return null;
                    }

                    // Check date time
                    if (beginTime >= endTime || beginDate >= endDate)
                    {
                        list = null;
                        return null;
                    }

                    int Tidx = data.FindIndex(t => t.TermID == termID);
                    Class cls = null;
                    if (Tidx != -1) // Check if term is contained in list
                    {
                        // if contains
                        if (data[Tidx].Courses.ContainsKey(courseID)) // Check if course is contained in term
                        {
                            // if contains
                            if (!data[Tidx].Courses[courseID].Classes.ContainsKey(classID)) // Check if class is not contained in course
                            {
                                // if not contain
                                // check if there is at least one class in the same time that manage by the same instructor
                                cls = new Class(classID, insID, insName, termID, courseID, courseName, credits, dayOfWeek, beginTime, endTime, beginDate, endDate, room, examDate, exr, examTime);
                                foreach (Class c in data[Tidx].Courses[courseID].Classes.Values)
                                {
                                    if (InvalidClass(cls, c))
                                    {
                                        list = null;
                                        return null;
                                    }
                                }
                                data[Tidx].Courses[courseID].Classes[classID] = cls;
                            }
                            else
                            {
                                list = null;
                                return null;
                            }
                        }
                        else
                        {
                            data[Tidx].Courses[courseID] = new Course(courseID, courseName, credits);
                            cls = new Class(classID, insID, insName, termID, courseID, courseName, credits, dayOfWeek, beginTime, endTime, beginDate, endDate, room, examDate, exr, examTime);
                            data[Tidx].Courses[courseID].Classes[classID] = cls;
                        }
                    }
                    else
                    {
                        Term term = new Term(termID);
                        term.Courses[courseID] = new Course(courseID, courseName, credits);
                        cls = new Class(classID, insID, insName, termID, courseID, courseName, credits, dayOfWeek, beginTime, endTime, beginDate, endDate, room, examDate, exr, examTime);
                        term.Courses[courseID].Classes[classID] = cls;
                        data.Add(term);
                    }
                    insSchedule.Add(new KeyValuePair<string, Class>(insID, cls));
                }
            }
            list = insSchedule;
            return data;
        }
        private void SendNotification(ref InstructorUser us, Class data)
        {
            Random rand = new Random();
            int id = rand.Next(0, 1000);
            string title = $"{data.CourseName} ({data.CourseID} - {data.ClassID})";
            string message = $"Bạn vừa được phân công dạy lớp này!";
            Notification n = new Notification(id, title, message, DateTime.Now);
            us.Notifications.Add(id, n);
        }
        private async Task<bool> UploadInstructorSchedule(List<KeyValuePair<string, Class>> list)
        {
            if (list == null) return false;
            DatabaseManager db = new DatabaseManager();
            Random rand = new Random();
            int id = rand.Next(0, 1000);
            var batch = new List<Task<bool>>();
            foreach (var c in list)
            {
                Account acc = await db.GetAccountAsync(c.Key, 1);
                if (acc != null)
                {
                    InstructorUser user = await db.GetInstructorAsync(c.Key);
                    bool contain = user.StudyElements.Any(n => n.TermID == c.Value.TermID && n.ClassID == c.Value.ClassID && n.CourseID == c.Value.CourseID);
                    if (!contain)
                    {
                        user.StudyElements.Add(new ClassIdentifier(c.Value.TermID, c.Value.CourseID, c.Value.ClassID));
                        SendNotification(ref user, c.Value);
                    }
                    Task<bool> task = db.UpdateInstructorAsync(acc.UUID, user);
                    batch.Add(task);
                }
                else return false;
            }
            var res = await Task.WhenAll(batch);
            foreach (bool state in res)
            {
                if (!state) return false;
            }
            return true;
        }
        private void ShowNotification(bool success)
        {
            if (success)
                _toastProvider.NotificationService.AddNotification(Flattinger.Core.Enums.ToastType.SUCCESS, "Cập nhật thành công!", "Lịch trình học tập và giảng dạy đã được cập nhật.", 500);
            else
                _toastProvider.NotificationService.AddNotification(Flattinger.Core.Enums.ToastType.ERROR, "Cập nhật thất bại!", "Dữ liệu không hợp lệ.", 500);
        }
        private void InitializeCommands()
        {
            BrowseCommand = new RelayCommand<TextBox>(p => { return _inProcess == false; }, p =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Pick a file";
                openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";
                if (openFileDialog.ShowDialog() == true)
                {
                    Path = openFileDialog.FileName;
                }
            });

            UploadCommand = new RelayCommand<object>(p => { return _path != null && _path.Length > 0 && _inProcess == false; }, async p =>
            {
                _inProcess = true;
                Loading = Visibility.Visible;
                List<Term>? terms = GetDataFromExcel(out List<KeyValuePair<string, Class>>? list);
                bool canUpload = await UploadInstructorSchedule(list);
                bool success = true;
                if (terms != null && canUpload)
                {
                    DatabaseManager db = new DatabaseManager();
                    var batch = new List<Task<bool>>();
                    foreach (Term term in terms)
                    {
                        Task<bool> task = db.UpdateTermAsync(term);
                        batch.Add(task);
                    }
                    var res = await Task.WhenAll(batch);
                    batch.Clear();
                }
                ShowNotification(terms != null && canUpload && success);
                Loading = Visibility.Hidden;
                Path = String.Empty;
                _inProcess = false;
            });

            DownloadCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                string url = "https://firebasestorage.googleapis.com/v0/b/academymanager-5ea2b.appspot.com/o/excelformat%2FScheduleFileFormat.xlsx?alt=media&token=8d5ca0d1-6658-4634-8991-eaebe93b59f8";
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            });
        }
        #endregion
        public AddScheduleVM(NotificationContainer n)
        {
            Loading = Visibility.Hidden;
            _inProcess = false;
            _theme = new AppTheme();
            _theme.ChangeTheme(Flattinger.Core.Enums.Theme.LIGHT);
            _toastProvider = new ToastProvider(n);
            InitializeCommands();
        }
    }
}
