using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AcademyManager.UCViews
{
    /// <summary>
    /// Interaction logic for SubjectContentUC.xaml
    /// </summary>
    public partial class SubjectContentUC : UserControl
    {
        public SubjectContentUC(string name, string url)
        {
            Title = name;
            URL = url;
            InitializeComponent();
        }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(SubjectContentUC));
        public string URL
        {
            get { return (string)GetValue(URLProperty); }
            set { SetValue(URLProperty, value); }
        }

        public static readonly DependencyProperty URLProperty = DependencyProperty.Register("URL", typeof(string), typeof(SubjectContentUC));

        private void content_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo(URL) { UseShellExecute = true });
        }
    }
}
