using System.Windows;
using System.Windows.Input;

namespace AcademyManager.UCViews
{
    /// <summary>
    /// Interaction logic for CustomedMessageBox.xaml
    /// </summary>
    public partial class CustomedMessageBox : Window
    {
        public CustomedMessageBox()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
            App.Current.Shutdown();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
