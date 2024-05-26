using AcademyManager.Models;
using AcademyManager.Views;
using OfficeOpenXml;
using Ookii.Dialogs.Wpf;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;
using System.Windows.Input;

namespace AcademyManager.Viewmodels
{
    public class StudentListVM : BaseViewModel
    {
        #region Commands
        public ICommand DownloadCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        #endregion
        #region Properties
        private ObservableCollection<StudentUser> StudentsList { get; set; }
        private ICollectionView _studentsview;
        private string _filter;
        public ICollectionView StudentsView
        {
            get => _studentsview;
            set { _studentsview = value; OnPropertyChanged(); }
        }
        public string Filter
        {
            get { return _filter; }
            set { _filter = value; StudentsView.Refresh(); OnPropertyChanged(); }
        }
        private Class ClassData { get; set; }
        #endregion
        #region Methods
        private void GenerateExcelFileAndSave(string path)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            string filename = $"Danh sách lớp {ClassData.ClassID} {ClassData.CourseName}.xlsx";
            string fullpath = Path.Combine(path, filename);
            var fi = new FileInfo(fullpath);
            if (fi.Exists)
            {
                fi.Delete();
            }
            using (var package = new ExcelPackage(fi))
            {
                var worksheet = package.Workbook.Worksheets.Add("Danh sách lớp");
                worksheet.Cells["A1"].Value = "Mã số sinh viên";
                worksheet.Cells["B1"].Value = "Họ và tên";
                worksheet.Cells["C1"].Value = "Email";
                worksheet.Cells["D1"].Value = "Ngày sinh";
                worksheet.Cells["E1"].Value = "Khoa";
                worksheet.Cells["F1"].Value = "Ngành";

                int row = 2;
                foreach (StudentUser s in StudentsList)
                {
                    worksheet.Cells[row, 1].Value = s.ID;
                    worksheet.Cells[row, 2].Value = s.Fullname;
                    worksheet.Cells[row, 3].Value = s.Email;
                    worksheet.Cells[row, 4].Value = s.Birthday.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 5].Value = s.Faculty;
                    worksheet.Cells[row, 6].Value = s.Major;
                    row++;
                }
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                package.Save();
            }
        }
        private async void LoadDataGrid()
        {
            var batch = new List<Task<StudentUser>>();
            var list = new List<StudentUser>();
            DatabaseManager db = new DatabaseManager();
            if (ClassData.Students == null) return;
            foreach (string id in ClassData.Students.Keys)
            {
                batch.Add(db.GetStudentAsync(id));
            }

            var result = await Task.WhenAll(batch);
            list.AddRange(result);
            StudentsList = [.. list];

            StudentsView = CollectionViewSource.GetDefaultView(StudentsList);
            StudentsView.Filter = FilterStudents;
        }
        private bool FilterStudents(object item)
        {
            if (item is StudentUser student)
            {
                return string.IsNullOrEmpty(Filter) || student.ID.Contains(Filter, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
        private void InitializeCommands()
        {
            DownloadCommand = new RelayCommand<object>(p => true, p =>
            {
                VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
                dialog.Description = "Chọn thư mục lưu trữ";
                dialog.UseDescriptionForTitle = true;
                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    GenerateExcelFileAndSave(dialog.SelectedPath);
                    string location = Path.Combine(dialog.SelectedPath, $"Danh sách lớp {ClassData.ClassID}.xlsx");
                }
            });

            CloseCommand = new RelayCommand<StudentListWindow>(p => true, p =>
            {
                p.Close();
            });
        }
        #endregion
        public StudentListVM(Class data)
        {
            ClassData = data;
            LoadDataGrid();
            InitializeCommands();
        }
    }
}
