using AcademyManager.Viewmodels;
using System.Windows.Controls;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for StudentSubjectList.xaml
    /// </summary>
    public partial class StudentSubjectList : UserControl
    {
        public SubjectListVM Viewmodel { get; set; }
        public StudentSubjectList(MainVM vm)
        {
            InitializeComponent();
            this.DataContext = Viewmodel = new SubjectListVM(vm, SubjectsList);
        }
    }
}
