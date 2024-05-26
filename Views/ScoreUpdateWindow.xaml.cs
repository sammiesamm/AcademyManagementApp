using AcademyManager.Models;
using AcademyManager.Viewmodels;
using System.Windows;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for ScoreUpdateWindow.xaml
    /// </summary>
    public partial class ScoreUpdateWindow : Window
    {
        public ScoreUpdateVM Viewmodel { get; set; }
        public ScoreUpdateWindow(Class data)
        {
            InitializeComponent();
            this.DataContext = Viewmodel = new ScoreUpdateVM(data, noti);
        }
    }
}
