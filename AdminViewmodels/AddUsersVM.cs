using AcademyManager.Models;
using AcademyManager.Viewmodels;
using Flattinger.Core.Theme;
using Flattinger.UI.ToastMessage;
using Flattinger.UI.ToastMessage.Controls;
using Microsoft.Win32;
using OfficeOpenXml;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AcademyManager.AdminViewmodels
{
    public class AddUsersVM : BaseViewModel
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
        private void ShowNotification(bool success)
        {
            if (success) _toastProvider.NotificationService.AddNotification(Flattinger.Core.Enums.ToastType.SUCCESS, "Cập nhật thành công!", "Dữ liệu người dùng đã được tải lên.", 500);
            else _toastProvider.NotificationService.AddNotification(Flattinger.Core.Enums.ToastType.ERROR, "Cập nhật thất bại!", "Dữ liệu không hợp lệ.", 500);
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
        private List<StudentUser>? GetStudentsDataFromExcel()
        {
            List<StudentUser> data = new List<StudentUser>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(_path)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;
                if (colCount != 7) return null;

                for (int row = 2; row <= rowCount; row++)
                {
                    // read data as string
                    string? studentID = worksheet.Cells[row, 1].Value.ToString();
                    string? fullname = worksheet.Cells[row, 2].Value.ToString();
                    string? bd = worksheet.Cells[row, 3].Value.ToString();
                    string? email = worksheet.Cells[row, 4].Value.ToString();
                    string? faculty = worksheet.Cells[row, 5].Value.ToString();
                    string? major = worksheet.Cells[row, 6].Value.ToString();
                    string? imgpath = worksheet.Cells[row, 7].Value.ToString();

                    // Check string input
                    if (NullOrEmpty(studentID) || NullOrEmpty(fullname) || NullOrEmpty(email) || NullOrEmpty(faculty)
                        || NullOrEmpty(major) || NullOrEmpty(imgpath)) return null;

                    // Image to Base64

                    // try to convert some data to correct type
                    DateOnly birthday;
                    string avtbase64;
                    try
                    {
                        birthday = StringToDateOnly(bd);
                        byte[] img = File.ReadAllBytes(imgpath);
                        avtbase64 = Convert.ToBase64String(img);
                    }
                    catch
                    {
                        return null;
                    }
                    if (data.Any(student => student.ID == studentID)) return null;
                    else data.Add(new StudentUser(studentID, fullname, email, birthday, faculty, avtbase64, major));
                }
            }
            return data;
        }
        private List<InstructorUser>? GetInstructorsDataFromExcel()
        {
            List<InstructorUser> data = new List<InstructorUser>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(_path)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;
                if (colCount != 8) return null;

                for (int row = 2; row <= rowCount; row++)
                {
                    // read data as string
                    string? insID = worksheet.Cells[row, 1].Value.ToString();
                    string? fullname = worksheet.Cells[row, 2].Value.ToString();
                    string? bd = worksheet.Cells[row, 3].Value.ToString();
                    string? email = worksheet.Cells[row, 4].Value.ToString();
                    string? faculty = worksheet.Cells[row, 5].Value.ToString();
                    string? certificate = worksheet.Cells[row, 6].Value.ToString();
                    string? speciality = worksheet.Cells[row, 7].Value.ToString();
                    string? imgpath = worksheet.Cells[row, 8].Value.ToString();

                    // Check string input
                    if (NullOrEmpty(insID) || NullOrEmpty(fullname) || NullOrEmpty(email) || NullOrEmpty(certificate)
                        || NullOrEmpty(speciality) || NullOrEmpty(faculty)) return null;

                    // try to convert some data to correct type
                    DateOnly birthday;
                    string avtbase64;
                    try
                    {
                        birthday = StringToDateOnly(bd);
                        byte[] img = File.ReadAllBytes(imgpath);
                        avtbase64 = Convert.ToBase64String(img);
                    }
                    catch
                    {
                        return null;
                    }
                    if (data.Any(ins => ins.ID == insID)) return null;
                    else data.Add(new InstructorUser(insID, fullname, email, birthday, faculty, avtbase64, certificate, speciality));
                }
            }
            return data;
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

            UploadCommand = new RelayCommand<ComboBox>(p => { return _path != null && _path.Length > 0 && p.SelectedIndex != -1 && _inProcess == false; }, async p =>
            {
                _inProcess = true;
                Loading = Visibility.Visible;
                bool error = false;
                if (p.SelectedIndex == 0)
                {
                    List<StudentUser>? students = GetStudentsDataFromExcel();
                    if (students != null)
                    {
                        var accbatch = new List<Task<bool>>();
                        var userbatch = new List<Task<bool>>();
                        DatabaseManager db = new DatabaseManager();
                        foreach (StudentUser std in students)
                        {
                            Account newacc = new Account(std.ID, std.Email, null, 2);
                            Account existacc = await db.GetAccountAsync(std.ID, 2);
                            if (existacc != null)
                            {
                                StudentUser s = await db.GetStudentByUUIDAsync(existacc.UUID);
                                s.UpdateInfo(std);
                                Task<bool> usertask = db.UpdateStudentAsync(existacc.UUID, s);
                                userbatch.Add(usertask);
                            }
                            else
                            {
                                Task<bool> acctask = db.UpdateAccountAsync(newacc, 2);
                                accbatch.Add(acctask);
                                Task<bool> usertask = db.UpdateStudentAsync(newacc.UUID, std);
                                userbatch.Add(usertask);
                            }
                        }
                        var accres = await Task.WhenAll(accbatch);
                        foreach (bool state in accres)
                        {
                            if (!state)
                            {
                                error = true;
                                break;
                            }
                        }
                        var userres = await Task.WhenAll(userbatch);
                        foreach (bool state in userres)
                        {
                            if (!state)
                            {
                                error = true;
                                break;
                            }
                        }
                        accbatch.Clear();
                        userbatch.Clear();
                        ShowNotification(!error);
                    } else ShowNotification(false);
                }
                else if (p.SelectedIndex == 1)
                {
                    List<InstructorUser>? instructors = GetInstructorsDataFromExcel();
                    if (instructors != null)
                    {
                        var accbatch = new List<Task<bool>>();
                        var userbatch = new List<Task<bool>>();
                        DatabaseManager db = new DatabaseManager();
                        foreach (InstructorUser ins in instructors)
                        {
                            Account newacc = new Account(ins.ID, ins.Email, null, 1);
                            Account existacc = await db.GetAccountAsync(ins.ID, 1);
                            if (existacc != null)
                            {
                                InstructorUser i = await db.GetInstructorByUUIDAsync(existacc.UUID);
                                i.UpdateInfo(ins);
                                Task<bool> usertask = db.UpdateInstructorAsync(existacc.UUID, i);
                                userbatch.Add(usertask);
                            }
                            else
                            {
                                Task<bool> acctask = db.UpdateAccountAsync(newacc, 1);
                                accbatch.Add(acctask);
                                Task<bool> usertask = db.UpdateInstructorAsync(newacc.UUID, ins);
                                userbatch.Add(usertask);
                            }
                        }
                        var accres = await Task.WhenAll(accbatch);
                        foreach (bool state in accres)
                        {
                            if (!state)
                            {
                                error = true;
                                break;
                            }
                        }
                        var userres = await Task.WhenAll(userbatch);
                        foreach (bool state in userres)
                        {
                            if (!state)
                            {
                                error = true;
                                break;
                            }
                        }
                        accbatch.Clear();
                        userbatch.Clear();
                        ShowNotification(!error);
                    }
                    else ShowNotification(false);
                }
                Loading = Visibility.Hidden;
                Path = String.Empty;
                _inProcess = false;
            });

            DownloadCommand = new RelayCommand<ComboBox>(p => { return p.SelectedIndex != -1; }, p =>
            {
                string url1 = "https://firebasestorage.googleapis.com/v0/b/academymanager-5ea2b.appspot.com/o/excelformat%2FStudentFileFormat.xlsx?alt=media&token=31b0bddc-9fa7-424a-a714-1fbad7a7d9e1",
                       url2 = "https://firebasestorage.googleapis.com/v0/b/academymanager-5ea2b.appspot.com/o/excelformat%2FInstructorFileFormat.xlsx?alt=media&token=b748facf-bdb0-4889-ada6-edb279fa2b8e";
                if (p.SelectedIndex == 0)
                {
                    Process.Start(new ProcessStartInfo(url1) { UseShellExecute = true });
                }
                else if (p.SelectedIndex == 1)
                {
                    Process.Start(new ProcessStartInfo(url2) { UseShellExecute = true });
                }
            });
        }
        #endregion
        public AddUsersVM(NotificationContainer n)
        {
            InitializeCommands();
            _toastProvider = new ToastProvider(n);
            _theme = new AppTheme();
            _theme.ChangeTheme(Flattinger.Core.Enums.Theme.LIGHT);
            _inProcess = false;
            Loading = Visibility.Hidden;
        }
    }
}
