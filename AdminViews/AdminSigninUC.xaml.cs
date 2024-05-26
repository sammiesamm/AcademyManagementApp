using AcademyManager.AdminViewmodels;
using System.Windows.Controls;

namespace AcademyManager.AdminViews
{
    /// <summary>
    /// Interaction logic for AdminSigninUC.xaml
    /// </summary>
    public partial class AdminSigninUC : UserControl
    {
        public AdminSigninVM Viewmodel { get; set; }
        public AdminSigninUC()
        {
            this.DataContext = Viewmodel = new AdminSigninVM();
            InitializeComponent();
        }
    }
}
