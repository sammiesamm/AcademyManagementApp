using AcademyManager.Models;
using AcademyManager.Viewmodels;
using System.Windows.Controls;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for LectureCourexaml.xaml
    /// </summary>
    public partial class LectureCourse : UserControl
    {
        public CourseInfoVM Viewmodel { get; set; }
        public LectureCourse(Class data, MainVM vm, SubjectListVM p)
        {
            InitializeComponent();
            this.DataContext = Viewmodel = new CourseInfoVM(data, vm, DocumentsPanel);
        }
    }
}
