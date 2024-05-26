using AcademyManager.Models;
using AcademyManager.UCViews;
using AcademyManager.Viewmodels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace AcademyManager.UCViewmodels
{
    public class FullCalendarVM : BaseViewModel
    {
        #region Commands
        public ICommand SelectedDateChangedCommand { get; set; }
        #endregion
        #region Properties
        private DateTime? _date;
        private string _datelb;
        private Visibility _v;
        public Visibility NoTask
        {
            get => _v;
            set
            {
                _v = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Item> _items;
        public ObservableCollection<Item> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }
        public DateTime? SelectedDate
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged();
                SelectedDateChangedCommand.Execute(null);
            }
        }
        public string DateLabel
        {
            get { return _datelb; }
            set { _datelb = value; OnPropertyChanged(); }
        }
        #endregion
        #region Methods
        private void LoadTodayTasks(DateOnly selectedDate, List<Class> list)
        {
            Items.Clear();  
            if (list.Count == 0 || list == null)
            {
                NoTask = Visibility.Visible;
                return;
            } else NoTask = Visibility.Hidden;
            TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            list.Sort((c1, c2) => c1.BeginTime.CompareTo(c2.BeginTime));
            
            foreach (Class c in list)
            {
                Item item;
                string bg = c.BeginTime.ToString("HH:mm");
                string e = c.EndTime.ToString("HH:mm");
                string time = $"{bg} - {e}";

                if (selectedDate > today) item = new Item(c.CourseName, time, c.Room, FontAwesome.WPF.FontAwesomeIcon.CircleOutline);
                else if (selectedDate < today) item = new Item(c.CourseName, time, c.Room, FontAwesome.WPF.FontAwesomeIcon.CheckCircle);
                else
                {
                    if (now > c.EndTime) item = new Item(c.CourseName, time, c.Room, FontAwesome.WPF.FontAwesomeIcon.CheckCircle);
                    else item = new Item(c.CourseName, time, c.Room, FontAwesome.WPF.FontAwesomeIcon.CircleOutline);
                }
                Items.Add(item);
            }
        }
        private void InitializeCommands()
        {
            SelectedDateChangedCommand = new RelayCommand<object>(p => true, p =>
            {
                DateLabel = SelectedDate.Value.ToString("MMMM d, yyyy");
                DateOnly input = DateOnly.FromDateTime((DateTime)SelectedDate);
                List<Class> todaytasks = MainVM.CurrentUser.GetSchedule(input, MainVM.UserClassList);
                LoadTodayTasks(input, todaytasks);
            });
        }
        #endregion
        public FullCalendarVM()
        {
            Items = new ObservableCollection<Item>();
            DateLabel = DateTime.Now.ToString("MMMM d, yyyy");
            DateOnly input = DateOnly.FromDateTime(DateTime.Now);
            List<Class> todaytasks = MainVM.CurrentUser.GetSchedule(input, MainVM.UserClassList);
            LoadTodayTasks(input, todaytasks);
            InitializeCommands();
        }
    }
}
