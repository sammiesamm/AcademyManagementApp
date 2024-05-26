using AcademyManager.Models;
using AcademyManager.Views;
using Flattinger.Core.Theme;
using Flattinger.UI.ToastMessage;
using Flattinger.UI.ToastMessage.Controls;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace AcademyManager.Viewmodels
{
    public class DocumentsUploadVM : BaseViewModel
    {
        #region Support class
        public class FileItem
        {
            public bool IsSelected { get; set; }
            public string Path { get; set; }
            public BitmapSource Icon { get; set; }
        }
        #endregion
        #region Commands
        public ICommand SelectFilesCommand { get; set; }
        public ICommand UploadFilesCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        #endregion
        #region Properties
        public ObservableCollection<FileItem> Files { get; set; }
        private Class ClassData { get; set; }
        private NotificationContainer _notification;
        private AppTheme _theme;
        private ToastProvider _toastProvider;
        private Visibility _load;
        public Visibility Loading
        {
            get => _load;
            set { _load = value; OnPropertyChanged(); }
        }
        #endregion
        #region Methods
        private async Task SendNotification()
        {
            Random random = new Random();
            DatabaseManager db = new DatabaseManager();
            int id = random.Next(0, 1000);
            string title = $"{ClassData.CourseName} ({ClassData.CourseID} - {ClassData.ClassID})";
            string message = "Tài liệu đã được cập nhật!";
            Notification noti = new Notification(id, title, message, DateTime.Now);
            if (ClassData.Students == null) return;
            foreach (string s in ClassData.Students.Keys)
            {
                var success = await db.SendNotificationAsync(s, 2, noti);
                do { success = await db.SendNotificationAsync(s, 2, noti); } while (!success);
            }
        }
        private void ShowNotification(bool success)
        {
            if (!success)
                _toastProvider.NotificationService.AddNotification(Flattinger.Core.Enums.ToastType.ERROR, "Thất bại!", "Không thể tải tệp tin!", 500);
            else
                _toastProvider.NotificationService.AddNotification(Flattinger.Core.Enums.ToastType.SUCCESS, "Thành công!", "Đã tải tệp tin lên!", 500);
        }
        private void InitializeCommands()
        {
            SelectFilesCommand = new RelayCommand<object>(p => true, p =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Multiselect = true,
                    Filter = "All files (*.*)|*.*"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filename in openFileDialog.FileNames)
                    {
                        if (String.IsNullOrEmpty(filename)) return;
                        Icon icon = Icon.ExtractAssociatedIcon(filename);
                        BitmapSource iconImage = Imaging.CreateBitmapSourceFromHIcon(
                            icon.Handle,
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());

                        Files.Add(new FileItem { IsSelected = false, Path = filename, Icon = iconImage });
                    }
                }
            });

            UploadFilesCommand = new RelayCommand<object>(p => Files.Count > 0, async p =>
            {
                Loading = Visibility.Visible;
                StorageManager storage = new StorageManager();
                bool error = false;
                string termid = ClassData.TermID,
                       courseid = ClassData.CourseID,
                       classid = ClassData.ClassID;

                var batch = new List<Task<bool>>();
                foreach (FileItem file in Files)
                {
                    string title = Path.GetFileNameWithoutExtension(file.Path);
                    Task<bool> task = storage.UploadFileToFirebaseStorage(file.Path, termid, courseid, classid, title);
                    batch.Add(task);
                }
                var res = await Task.WhenAll(batch);
                foreach (bool state in res)
                {
                    if (!state)
                    {
                        error = true;
                        break;
                    }
                }

                ShowNotification(!error);
                await SendNotification();
                Loading = Visibility.Hidden;
                Files.Clear();
            });

            RemoveCommand = new RelayCommand<object>(p => true, p =>
            {
                var selectedFiles = Files.Where(f => f.IsSelected).ToList();
                foreach (var file in selectedFiles)
                {
                    Files.Remove(file);
                }
            });

            CloseCommand = new RelayCommand<DocumentsUploadWindow>(p => true, p =>
            {
                p.Close();
            });
        }
        #endregion
        public DocumentsUploadVM(Class data, NotificationContainer noti)
        {
            ClassData = data;
            _notification = noti;
            _theme = new AppTheme();
            _theme.ChangeTheme(Flattinger.Core.Enums.Theme.LIGHT);
            _toastProvider = new ToastProvider(noti);
            Files = new ObservableCollection<FileItem>();
            Loading = Visibility.Hidden;
            InitializeCommands();
        }
    }
}
