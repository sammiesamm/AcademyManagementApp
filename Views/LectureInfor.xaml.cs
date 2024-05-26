using AcademyManager.Viewmodels;
using System.Windows.Controls;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for LectureInfor.xaml
    /// </summary>
    public partial class LectureInfor : UserControl
    {
        public LectureInfoVM Viewmodel { get; set; }
        public LectureInfor(MainVM vm)
        {
            this.DataContext = Viewmodel = new LectureInfoVM(vm);
            InitializeComponent();
        }
    }
}
