using AcademyManager.Views;
using System.Windows.Input;

namespace AcademyManager.Viewmodels
{
    public class StudentHomeVM : BaseViewModel
    {
        #region Command
        public ICommand DailyScheduleCommand { get; set; }
        public ICommand ExamScheduleCommand { get; set; }
        public ICommand SubjectCommand { get; set; }
        public ICommand InfoCommand { get; set; }
        public ICommand ResultCommand { get; set; }
        #endregion

        #region Properties
        private MainVM ParentVM { get; set; }
        #endregion

        #region Methods
        private void InitializeCommand()
        {
            DailyScheduleCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                ParentVM.DailyScheduleView = new StudySchedule(ParentVM);
                ParentVM.CurrentView = ParentVM.DailyScheduleView;
            });

            SubjectCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                ParentVM.CourseListView = new StudentSubjectList(ParentVM);
                ParentVM.CurrentView = ParentVM.CourseListView;
            });

            InfoCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                ParentVM.InfoView = new StudentInfor(ParentVM);
                ParentVM.CurrentView = ParentVM.InfoView;
            });

            ResultCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                ParentVM.ResultView = new Result(ParentVM);
                ParentVM.CurrentView = ParentVM.ResultView;
            });

            ExamScheduleCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                ParentVM.ExamScheduleView = new ExamSchedule(ParentVM);
                ParentVM.CurrentView = ParentVM.ExamScheduleView;
            });
        }
        #endregion
        public StudentHomeVM(MainVM parentVM)
        {
            InitializeCommand();
            ParentVM = parentVM;
        }
    }
}
