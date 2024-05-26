using AcademyManager.AdminViewmodels;
using System.Windows.Controls;

namespace AcademyManager.AdminViews
{
    /// <summary>
    /// Interaction logic for AddCoursesUC.xaml
    /// </summary>
    public partial class AddScheduleUC : UserControl
    {
        public AddScheduleVM Viewmodel { get; set; }
        public AddScheduleUC()
        {
            InitializeComponent();
            this.DataContext = Viewmodel = new AddScheduleVM(noti);
        }
    }
}
