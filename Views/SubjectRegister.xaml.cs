using AcademyManager.Viewmodels;
using System.Windows.Controls;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for SubjectRegister.xaml
    /// </summary>
    public partial class SubjectRegister : UserControl
    {
        public SubjectRegisterVM Viewmodel { get; set; }
        public SubjectRegister(MainVM vm, SubjectListVM p)
        {
            InitializeComponent();
            this.DataContext = Viewmodel = new SubjectRegisterVM(vm, p, notification);
        }
    }
}
