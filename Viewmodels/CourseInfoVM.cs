using AcademyManager.Models;
using AcademyManager.UCViews;
using AcademyManager.Views;
using System.Windows.Controls;
using System.Windows.Input;

namespace AcademyManager.Viewmodels
{
    public class CourseInfoVM : BaseViewModel
    {
        #region Commands
        public ICommand ViewStudentListCommand { get; set; }
        public ICommand UpdateScoreCommand { get; set; }
        public ICommand AddDocumentCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand ReloadCommand { get; set; }
        #endregion
        #region Properties
        public Class Data { get; set; }
        private MainVM GGParentVM { get; set; }
        private StackPanel DocumentsPanel { get; set; }
        #endregion
        #region Methods
        public async void LoadDocuments()
        {
            DatabaseManager db = new DatabaseManager();
            Data = await db.GetClassAsync(Data.TermID, Data.CourseID, Data.ClassID);
            if (Data.Documents == null) return;
            DocumentsPanel.Children.Clear();
            foreach (var doc in Data.Documents)
            {
                SubjectContentUC item = new SubjectContentUC(doc.Key, doc.Value);
                DocumentsPanel.Children.Add(item);
            }
        }
        private void InitializeCommands()
        {
            ViewStudentListCommand = new RelayCommand<object>(p => Data.Students != null, p =>
            {
                StudentListWindow window = new StudentListWindow(Data);
                window.ShowDialog();
            });

            UpdateScoreCommand = new RelayCommand<object>(p => Data.Students != null, p =>
            {
                ScoreUpdateWindow window = new ScoreUpdateWindow(Data);
                window.ShowDialog();
            });

            AddDocumentCommand = new RelayCommand<object>(p => true, p =>
            {
                DocumentsUploadWindow window = new DocumentsUploadWindow(Data);
                window.ShowDialog();
            });

            BackCommand = new RelayCommand<object>(p => true, p =>
            {
                GGParentVM.CurrentView = GGParentVM.CourseListView;
            });

            ReloadCommand = new RelayCommand<object>(p => true, p =>
            {
                LoadDocuments();
            });
        }
        #endregion
        public CourseInfoVM(Class data, MainVM vm, StackPanel p)
        {
            Data = data;
            GGParentVM = vm;
            DocumentsPanel = p;
            LoadDocuments();
            InitializeCommands();
        }
    }
}
