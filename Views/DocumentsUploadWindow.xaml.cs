using AcademyManager.Models;
using AcademyManager.Viewmodels;
using System.Windows;

namespace AcademyManager.Views
{
    /// <summary>
    /// Interaction logic for DocumentsUploadWindow.xaml
    /// </summary>
    public partial class DocumentsUploadWindow : Window
    {
        public DocumentsUploadVM Viewmodel { get; set; }
        public DocumentsUploadWindow(Class data)
        {
            InitializeComponent();
            this.DataContext = Viewmodel = new DocumentsUploadVM(data, notification);
        }
    }
}
