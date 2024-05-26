using AcademyManager.Viewmodels;
using System.Windows.Controls;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public LoginVM Viewmodel { get; set; }
        public Login(MainVM vm)
        {
            this.DataContext = Viewmodel = new LoginVM(vm);
            InitializeComponent();
        }
    }
}
