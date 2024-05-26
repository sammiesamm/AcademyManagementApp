using AcademyManager.Models;
using AcademyManager.Viewmodels;
using System.Windows;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for StudentListWindow.xaml
    /// </summary>
    public partial class StudentListWindow : Window
    {
        public StudentListVM Viewmodel { get; set; }
        public StudentListWindow(Class data)
        {
            this.DataContext = Viewmodel = new StudentListVM(data);
            InitializeComponent();
        }
    }
}
