using AcademyManager.AdminViewmodels;
using System.Windows.Controls;

namespace AcademyManager.AdminViews
{
    /// <summary>
    /// Interaction logic for AddUsersUC.xaml
    /// </summary>
    public partial class AddUsersUC : UserControl
    {
        public AddUsersVM Viewmodel { get; set; }
        public AddUsersUC()
        {
            InitializeComponent();
            this.DataContext = Viewmodel = new AddUsersVM(noti);
        }
    }
}
