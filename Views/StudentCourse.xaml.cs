using AcademyManager.Models;
using AcademyManager.Viewmodels;
using System.Windows.Controls;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for StudentCourse.xaml
    /// </summary>
    public partial class StudentCourse : UserControl
    {
        public CourseInfoVM Viewmodel { get; set; }
        public StudentCourse(Class data, MainVM vm, SubjectListVM p)
        {
            InitializeComponent();
            this.DataContext = Viewmodel = new CourseInfoVM(data, vm, DocumentsPanel);
        }
    }
}
