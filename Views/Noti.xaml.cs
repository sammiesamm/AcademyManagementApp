using AcademyManager.Viewmodels;
using System.Windows.Controls;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for Noti.xaml
    /// </summary>
    public partial class Noti : UserControl
    {
        public NotiVM Viewmodel { get; set; }
        public Noti(MainVM vm)
        {
            InitializeComponent();
            this.DataContext = Viewmodel = new NotiVM(vm, notiPanel);
        }
    }
}
