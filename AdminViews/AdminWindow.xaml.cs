using AcademyManager.AdminViewmodels;
using System.Windows;

namespace AcademyManager.AdminViews
{
    public partial class AdminWindow : Window
    {
        public AdminWindowVM Viewmodel { get; set; }
        public AdminWindow()
        {
            this.DataContext = Viewmodel = new AdminWindowVM();
            InitializeComponent();
        }
    }
}
