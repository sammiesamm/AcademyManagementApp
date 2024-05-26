using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AcademyManager.UCViews
{
    /// <summary>
    /// Interaction logic for CalendarUC.xaml
    /// </summary>
    public partial class CalendarUC : UserControl
    {
        public CalendarUC()
        {
            InitializeComponent();
        }

        // DependencyProperty for SelectedDate
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(CalendarUC));

        // Property to get and set the SelectedDate
        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }
        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Calendar calendar = (Calendar)sender;
            Keyboard.Focus(null);
            this.SelectedDate = calendar.SelectedDate;
        }
    }
}
