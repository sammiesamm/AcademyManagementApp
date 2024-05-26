using AcademyManager.Viewmodels;
using System.Windows.Controls;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for LectureSubjectList.xaml
    /// </summary>
    public partial class LectureSubjectList : UserControl
    {
        public SubjectListVM Viewmodel { get; set; }
        public LectureSubjectList(MainVM vm)
        {
            InitializeComponent();
            this.DataContext = Viewmodel = new SubjectListVM(vm, SubjectsList);
        }
    }
}
