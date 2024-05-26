using AcademyManager.Viewmodels;
using System.Windows.Controls;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for StudentInfor.xaml
    /// </summary>
    public partial class StudentInfor : UserControl
    {
        public StudentInfoVM Viewmodel { get; set; }
        public StudentInfor(MainVM vm)
        {
            this.DataContext = Viewmodel = new StudentInfoVM(vm);
            InitializeComponent();
        }
    }
}
