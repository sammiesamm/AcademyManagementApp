using AcademyManager.Viewmodels;
using System.Windows.Controls;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for LectureMainScreenxaml.xaml
    /// </summary>
    public partial class LectureMainScreen : UserControl
    {
        public LectureHomeVM Viewmodel { get; set; }
        public LectureMainScreen(MainVM vm)
        {
            this.DataContext = Viewmodel = new LectureHomeVM(vm);
            InitializeComponent();
        }
    }
}
