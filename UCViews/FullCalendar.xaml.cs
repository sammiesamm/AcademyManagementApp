using AcademyManager.UCViewmodels;
using System.Windows.Controls;

namespace AcademyManager.UCViews
{
    /// <summary>
    /// Interaction logic for Calendar.xaml
    /// </summary>
    public partial class FullCalendar : UserControl
    {
        public FullCalendarVM Viewmodel { get; set; }
        public FullCalendar()
        {
            InitializeComponent();
            this.DataContext = Viewmodel = new FullCalendarVM();
        }
    }
}