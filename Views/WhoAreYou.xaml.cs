using AcademyManager.Viewmodels;
using System.Windows.Controls;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for WhoAreYou.xaml
    /// </summary>
    public partial class WhoAreYou : UserControl
    {
        public WhoAreYouVM Viewmodel { get; set; }
        public WhoAreYou(MainVM vm)
        {
            this.DataContext = Viewmodel = new WhoAreYouVM(vm);
            InitializeComponent();
        }
    }
}
