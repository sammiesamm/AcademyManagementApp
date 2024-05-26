using AcademyManager.Viewmodels;
using System.Windows;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for MainScreen.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainVM Viewmodel { get; set; }
        public MainWindow()
        {
            this.DataContext = Viewmodel = new MainVM();
            InitializeComponent();
        }
    }
}
