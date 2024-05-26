using AcademyManager.AdminViewmodels;
using System.Windows;

namespace AcademyManager.AdminViews
{
    /// <summary>
    /// Interaction logic for AdminAuthWindow.xaml
    /// </summary>
    public partial class AdminAuthWindow : Window
    {
        public AdminAuthWindowVM Viewmodel { get; set; }
        public AdminAuthWindow()
        {
            this.DataContext = Viewmodel = new AdminAuthWindowVM();
            InitializeComponent();
        }
    }
}
