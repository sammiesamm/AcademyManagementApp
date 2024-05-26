using System.Windows;
using System.Windows.Controls;

namespace AcademyManager.UCViews
{
    /// <summary>
    /// Interaction logic for ExamScheduleSubject.xaml
    /// </summary>
    public partial class ExamScheduleUC : UserControl
    {
        public ExamScheduleUC()
        {
            InitializeComponent();
        }
        public ExamScheduleUC(string courseName, DateOnly examDate, string examRoom, TimeOnly examTime)
        {
            InitializeComponent();
            Subject = courseName;
            Date = examDate.ToString("dd/MM/yyyy");
            Room = examRoom;
            Time = examTime.ToString("HH:mm");
        }
        public string Subject
        {
            get { return (string)GetValue(SubjectProperty); }
            set { SetValue(SubjectProperty, value); }
        }

        public static readonly DependencyProperty SubjectProperty = DependencyProperty.Register("Subject", typeof(string), typeof(ExamScheduleUC));
        public string Room
        {
            get { return (string)GetValue(RoomProperty); }
            set { SetValue(RoomProperty, value); }
        }

        public static readonly DependencyProperty RoomProperty = DependencyProperty.Register("Room", typeof(string), typeof(ExamScheduleUC));
        public string Date
        {
            get { return (string)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly DependencyProperty DateProperty = DependencyProperty.Register("Date", typeof(string), typeof(ExamScheduleUC));
        public string Time
        {
            get { return (string)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(string), typeof(ExamScheduleUC));
    }
}
