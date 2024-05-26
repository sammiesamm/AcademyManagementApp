using AcademyManager.Viewmodels;
using System.Windows.Controls;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for StudentMainScreen.xaml
    /// </summary>
    public partial class StudentMainScreen : UserControl
    {
        public StudentHomeVM Viewmodel { get; set; }
        public StudentMainScreen(MainVM vm)
        {
            this.DataContext = Viewmodel = new StudentHomeVM(vm);
            InitializeComponent();
        }
    }
}
