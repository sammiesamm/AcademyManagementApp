using AcademyManager.Models;
using AcademyManager.Viewmodels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AcademyManager.AdminViewmodels
{
    public class AdminSigninVM : BaseViewModel
    {
        #region Commands
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand PasswordConfirmCommand { get; set; }
        public ICommand SigninCommand { get; set; }
        #endregion

        #region Properties
        private string _password;
        private string _passwordConfirm;
        private string _uuid;
        private string _notification;
        private PasswordBox _passwordBox, _confirmBox;
        private Brush _foreground;
        private Visibility _notificationV;
        private Visibility _load;
        public Visibility Loading
        {
            get => _load;
            set
            {
                _load = value;
                OnPropertyChanged();
            }
        }
        public Brush Foreground
        {
            get { return _foreground; }
            set { _foreground = value; OnPropertyChanged(); }
        }
        public string Notification
        {
            get { return _notification; }
            set { _notification = value; OnPropertyChanged(); }
        }
        public Visibility NotificationV
        {
            get { return _notificationV; }
            set { _notificationV = value; OnPropertyChanged(); }
        }
        public string UUID
        {
            get { return _uuid; }
            set { _uuid = value; OnPropertyChanged(); }
        }
        #endregion

        #region Methods
        private bool NullOrEmpty(string s)
        {
            return s == null || s.Length == 0;
        }
        private void InitializeCommands()
        {
            PasswordChangedCommand = new RelayCommand<PasswordBox>(p => { return true; }, p =>
            {
                _password = p.Password;
                _passwordBox = p;
            });

            PasswordConfirmCommand = new RelayCommand<PasswordBox>(p => { return true; }, p =>
            {
                _passwordConfirm = p.Password;
                _confirmBox = p;
            });

            SigninCommand = new RelayCommand<object>(p => { return !NullOrEmpty(_password) && !NullOrEmpty(_uuid) && !NullOrEmpty(_passwordConfirm); }, async p =>
            {
                Loading = Visibility.Visible;
                DatabaseManager db = new DatabaseManager();
                Admin ad = await db.GetAdminAsync(_uuid);
                if (ad != null)
                {
                    if (ad.Activated())
                    {
                        Loading = Visibility.Hidden;
                        Notification = "Tài khoản này đã được kích hoạt.";
                        Foreground = Brushes.DeepPink;
                        NotificationV = Visibility.Visible;
                        await Task.Delay(1000);
                        NotificationV = Visibility.Hidden;
                    }
                    else if (_password.Length < 8)
                    {
                        Loading = Visibility.Hidden;
                        Notification = "Mật khẩu phải chứa ít nhất 8 kí tự.";
                        Foreground = Brushes.DeepPink;
                        NotificationV = Visibility.Visible;
                        await Task.Delay(1000);
                        NotificationV = Visibility.Hidden;
                    }
                    else if (_password != _passwordConfirm)
                    {
                        Loading = Visibility.Hidden;
                        Notification = "Xác nhận mật khẩu không trùng khớp.";
                        Foreground = Brushes.DeepPink;
                        NotificationV = Visibility.Visible;
                        await Task.Delay(1000);
                        NotificationV = Visibility.Hidden;
                    }
                    else
                    {
                        Admin newadmin = new Admin(_uuid, _password);
                        bool success = await db.UpdateAdminPassword(newadmin);
                        if (success)
                        {
                            Loading = Visibility.Hidden;
                            Notification = "Kích hoạt thành công.";
                            Foreground = Brushes.Green;
                            NotificationV = Visibility.Visible;
                            await Task.Delay(1000);
                            NotificationV = Visibility.Hidden;
                        }
                        else
                        {
                            Loading = Visibility.Hidden;
                            Notification = "Lỗi đường truyền.";
                            Foreground = Brushes.OrangeRed;
                            NotificationV = Visibility.Visible;
                            await Task.Delay(1000);
                            NotificationV = Visibility.Hidden;
                        }
                    }
                }
                else
                {
                    Loading = Visibility.Hidden;
                    NotificationV = Visibility.Hidden;
                    Notification = "Tài khoản chưa được cấp quyền.";
                    Foreground = Brushes.DeepPink;
                    NotificationV = Visibility.Visible;
                    await Task.Delay(1000);
                    NotificationV = Visibility.Hidden;
                }
                UUID = String.Empty;
                _passwordBox.Clear();
                _confirmBox.Clear();
            });
        }
        #endregion
        public AdminSigninVM()
        {
            InitializeCommands();
            NotificationV = Visibility.Hidden;
            Loading = Visibility.Hidden;
        }
    }
}
