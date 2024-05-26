using AcademyManager.Viewmodels;
using System.Windows.Controls;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for WelcomeScreen.xaml
    /// </summary>
    public partial class WelcomeScreen : UserControl
    {
        public WelcomeScreenVM Viewmodel { get; set; }
        public WelcomeScreen(MainVM vm)
        {
            this.DataContext = Viewmodel = new WelcomeScreenVM(vm);
            InitializeComponent();
        }
    }
}
