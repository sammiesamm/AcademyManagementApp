using AcademyManager.AdminViewmodels;
using System.Windows.Controls;

namespace AcademyManager.AdminViews
{
    /// <summary>
    /// Interaction logic for AdminLoginUC.xaml
    /// </summary>
    public partial class AdminLoginUC : UserControl
    {
        public AdminLoginVM Viewmodel { get; set; }
        public AdminLoginUC()
        {
            this.DataContext = Viewmodel = new AdminLoginVM();
            InitializeComponent();
        }
    }
}
