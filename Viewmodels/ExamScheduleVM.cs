using AcademyManager.Models;
using AcademyManager.UCViews;
using System.Windows.Controls;
using System.Windows.Input;

namespace AcademyManager.Viewmodels
{
    public class ExamScheduleVM : BaseViewModel
    {
        #region Commands
        public ICommand BackCommand { get; set; }
        #endregion
        #region Properties
        private MainVM ParentVM { get; set; }
        #endregion
        #region Methods
        private void LoadExamSchedule(StackPanel p)
        {
            List<Class> list = MainVM.UserClassList;
            foreach (Class cls in list)
            {
                ExamScheduleUC item = new ExamScheduleUC(cls.CourseName, cls.ExamDate, cls.ExamRoom, cls.ExamTime);
                p.Children.Add(item);
            }
        }
        private void InitializeCommand()
        {
            BackCommand = new RelayCommand<object>(p => true, p =>
            {
                ParentVM.HomeNavigateCommand.Execute(null);
            });
        }
        #endregion
        public ExamScheduleVM(MainVM vm, StackPanel panel)
        {
            ParentVM = vm;
            LoadExamSchedule(panel);
            InitializeCommand();
        }
    }
}
