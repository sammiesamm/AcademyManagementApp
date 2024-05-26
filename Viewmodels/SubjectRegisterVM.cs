using AcademyManager.Models;
using AcademyManager.UCViews;
using Flattinger.Core.Theme;
using Flattinger.UI.ToastMessage;
using Flattinger.UI.ToastMessage.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AcademyManager.Viewmodels
{
    public class SubjectRegisterVM : BaseViewModel
    {
        #region Commands
        public ICommand BackCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        #endregion
        #region Properties
        private MainVM GParentVM { get; set; }
        private SubjectListVM ParentVM { get; set; }
        private AppTheme _theme;
        private ToastProvider _toastProvider;
        private string _cid;
        private Visibility _unfound;
        private bool inProcess;
        public Visibility Unfound
        {
            get => _unfound;
            set
            {
                _unfound = value;
                OnPropertyChanged();
            }
        }
        public string CID
        {
            get { return _cid; }
            set { _cid = value; OnPropertyChanged(); }
        }
        #endregion
        #region Methods
        public void ShowRegisterNotification(bool success)
        {
            if (success) _toastProvider.NotificationService.AddNotification(Flattinger.Core.Enums.ToastType.SUCCESS, "Thành công!", "Đăng ký môn thành công.", 500);
            else _toastProvider.NotificationService.AddNotification(Flattinger.Core.Enums.ToastType.ERROR, "Thất bại!", "Đăng ký môn thất bại.", 500);
        }
        public void ShowCancelNotification(bool success)
        {
            if (success) _toastProvider.NotificationService.AddNotification(Flattinger.Core.Enums.ToastType.SUCCESS, "Thành công!", "Hủy đăng ký môn thành công.", 500);
            else _toastProvider.NotificationService.AddNotification(Flattinger.Core.Enums.ToastType.ERROR, "Thất bại!", "Hủy đăng ký môn thất bại.", 500);
        }
        public void ShowTimeConflictNotification()
        {
            _toastProvider.NotificationService.AddNotification(Flattinger.Core.Enums.ToastType.ERROR, "Thất bại!", "Đăng ký trùng lịch học.", 500);
        }
        private async Task<bool> LoadAvailableCourses(StackPanel panel)
        {
            if (panel != null) panel.Children.Clear();
            DatabaseManager db = new DatabaseManager();
            string termid = await db.GetCurrentTermAsync();
            Course course = await db.GetCourseAsync(termid, CID);
            if (course == null) return false;
            if (course.Classes.Count == 0) return false;
            foreach (Class c in course.Classes.Values)
            {
                bool contain = MainVM.CurrentUser.StudyElements.Any(e => e.TermID == c.TermID && e.CourseID == c.CourseID && e.ClassID == c.ClassID);
                SubjectRegisterUC item;
                if (!contain) item = new SubjectRegisterUC(c, MaterialDesignThemes.Wpf.PackIconKind.PencilBox, this);
                else item = new SubjectRegisterUC(c, MaterialDesignThemes.Wpf.PackIconKind.BoxCancel, this);
                panel.Children.Add(item);
            }
            return true;
        }
        private void InitializeCommands()
        {
            BackCommand = new RelayCommand<object>(p => true, p =>
            {
                GParentVM.CurrentView = GParentVM.CourseListView;
                ParentVM.LoadClasses();
            });

            SearchCommand = new RelayCommand<StackPanel>(p => _cid != null && _cid.Length > 0 && !inProcess, async p =>
            {
                inProcess = true;
                bool found = await LoadAvailableCourses(p);
                if (!found) Unfound = Visibility.Visible;
                else Unfound = Visibility.Hidden;
                inProcess = false;
            });
        }
        #endregion
        public SubjectRegisterVM(MainVM vm, SubjectListVM p, NotificationContainer container)
        {
            GParentVM = vm;
            ParentVM = p;
            inProcess = false;
            _theme = new AppTheme();
            _theme.ChangeTheme(Flattinger.Core.Enums.Theme.LIGHT);
            _toastProvider = new ToastProvider(container);
            Unfound = Visibility.Hidden;
            InitializeCommands();
        }
    }
}
