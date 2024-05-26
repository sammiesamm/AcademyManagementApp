using AcademyManager.Viewmodels;
using System.Windows;
using System.Windows.Controls;

namespace AcademyManager.UCViews
{
    /// <summary>
    /// Interaction logic for SpecificNoti.xaml
    /// </summary>
    public partial class SpecificNoti : UserControl
    {
        public SpecificNoti()
        {
            InitializeComponent();
        }
        public SpecificNoti(int id, string title, string notiContent, DateTime updateTime, NotiVM p)
        {
            NotiParent = p;
            ID = id;
            Title = title;
            NotiContent = notiContent;
            UpdateTime = updateTime.ToString();
            InitializeComponent();
        }
        public NotiVM NotiParent { get; set; }
        public int ID { get; set; }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(SpecificNoti));
        public string NotiContent
        {
            get { return (string)GetValue(NotiContentProperty); }
            set { SetValue(NotiContentProperty, value); }
        }

        public static readonly DependencyProperty NotiContentProperty = DependencyProperty.Register("NotiContent", typeof(string), typeof(SpecificNoti));
        public string UpdateTime
        {
            get { return (string)GetValue(UpdateTimeProperty); }
            set { SetValue(UpdateTimeProperty, value); }
        }

        public static readonly DependencyProperty UpdateTimeProperty = DependencyProperty.Register("UpdateTime", typeof(string), typeof(SpecificNoti));

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            NotiParent.DeletedNoti = this;
            NotiParent.DeleteNoti();
        }
    }
}
