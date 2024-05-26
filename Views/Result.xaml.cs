using AcademyManager.Viewmodels;
using System.Windows.Controls;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for Result.xaml
    /// </summary>
    public partial class Result : UserControl
    {
        public StudentResultVM Viewmodel { get; set; }
        public Result(MainVM vm)
        {
            this.DataContext = Viewmodel = new StudentResultVM(vm);
            InitializeComponent();
        }
    }
}
