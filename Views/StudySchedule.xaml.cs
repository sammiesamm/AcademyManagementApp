using AcademyManager.Viewmodels;
using System.Windows.Controls;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for StudySchedual.xaml
    /// </summary>
    public partial class StudySchedule : UserControl
    {
        public StudyScheduleVM Viewmodel { get; set; }
        public StudySchedule(MainVM vm)
        {
            InitializeComponent();
            this.DataContext = Viewmodel = new StudyScheduleVM(vm);
        }
    }
}
