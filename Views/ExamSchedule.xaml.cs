using AcademyManager.Viewmodels;
using System.Windows.Controls;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for ExamSchedule.xaml
    /// </summary>
    public partial class ExamSchedule : UserControl
    {
        public ExamScheduleVM Viewmodel { get; set; }
        public ExamSchedule(MainVM vm)
        {
            InitializeComponent();
            this.DataContext = Viewmodel = new ExamScheduleVM(vm, ExamList);
        }
    }
}
