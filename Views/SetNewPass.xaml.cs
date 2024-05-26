using AcademyManager.Viewmodels;
using System.Windows.Controls;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for SetNewPass.xaml
    /// </summary>
    public partial class SetNewPass : UserControl
    {
        public SetPasswordVM Viewmodel { get; set; }
        public SetNewPass(MainVM vm)
        {
            this.DataContext = Viewmodel = new SetPasswordVM(vm);
            InitializeComponent();
        }
    }
}
