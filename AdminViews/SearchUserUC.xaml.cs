using AcademyManager.AdminViewmodels;
using System.Windows.Controls;

namespace AcademyManager.AdminViews
{
    /// <summary>
    /// Interaction logic for SearchUserUC.xaml
    /// </summary>
    public partial class SearchUserUC : UserControl
    {
        public SearchUserVM Viewmodel { get; set; }
        public SearchUserUC()
        {
            this.DataContext = Viewmodel = new SearchUserVM();
            InitializeComponent();
        }
    }
}
