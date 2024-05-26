using AcademyManager.Viewmodels;
using System.Windows.Controls;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for ForgetPass.xaml
    /// </summary>
    public partial class ForgetPass : UserControl
    {
        public ForgetPassVM Viewmodel { get; set; }
        public ForgetPass(MainVM vm)
        {
            this.DataContext = Viewmodel = new ForgetPassVM(vm);
            InitializeComponent();
        }
    }
}
