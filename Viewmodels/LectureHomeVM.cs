using AcademyManager.Views;
using System.Windows.Input;

namespace AcademyManager.Viewmodels
{
    public class LectureHomeVM : BaseViewModel
    {
        #region Commands
        public ICommand InfoCommand { get; set; }
        public ICommand CourseCommand { get; set; }
        public ICommand ScheduleCommand { get; set; }
        #endregion
        #region Properties
        private MainVM ParentVM { get; set; }
        #endregion
        #region Methods
        private void InitializeCommands()
        {
            InfoCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                ParentVM.InfoView = new LectureInfor(ParentVM);
                ParentVM.CurrentView = ParentVM.InfoView;
            });

            CourseCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                ParentVM.CourseListView = new LectureSubjectList(ParentVM);
                ParentVM.CurrentView = ParentVM.CourseListView;
            });

            ScheduleCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                ParentVM.DailyScheduleView = new StudySchedule(ParentVM);
                ParentVM.CurrentView = ParentVM.DailyScheduleView;
            });
        }
        #endregion
        public LectureHomeVM(MainVM vm)
        {
            InitializeCommands();
            ParentVM = vm;
        }
    }
}
