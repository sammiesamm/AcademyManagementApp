using AcademyManager.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AcademyManager.Viewmodels
{
    public class SetPasswordVM : BaseViewModel
    {
        #region Commands
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand ConfirmPasswordChangedCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand BackCommand { get; set; }
        #endregion
        #region Properties
        private MainVM ParentVM { get; set; }
        private Account _tempAcc;
        private string _userid;
        private string _email;
        private string _password;
        private string _confirm;
        private string _noti;
        private Brush _labelcolor;
        private Visibility _load;
        private PasswordBox _passwordBox, _confirmBox;
        public Brush LabelColor
        {
            get => _labelcolor;
            set
            {
                _labelcolor = value;
                OnPropertyChanged();
            }
        }
        public Visibility Loading
        {
            get { return _load; }
            set { _load = value; OnPropertyChanged(); }
        }
        public string Noti
        {
            get { return _noti; }
            set { _noti = value; OnPropertyChanged(); }
        }
        public string UserID
        {
            get { return _userid; }
            set { _userid = value; OnPropertyChanged(); }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }
        #endregion
        #region Methods
        private bool NullOrEmpty(string s)
        {
            return (s == null || s == String.Empty);
        }
        private void ResetAll()
        {
            UserID = "";
            Email = "";
            if (_passwordBox != null) _passwordBox.Clear();
            if (_confirmBox != null) _confirmBox.Clear();
        }
        private void InitializeCommands()
        {
            PasswordChangedCommand = new RelayCommand<PasswordBox>(p => { return true; }, p =>
            {
                _passwordBox = p;
                _password = p.Password;
            });

            ConfirmPasswordChangedCommand = new RelayCommand<PasswordBox>(p => { return true; }, p =>
            {
                _confirmBox = p;
                _confirm = p.Password;
            });

            ConfirmCommand = new RelayCommand<object>(p =>
            {
                return !NullOrEmpty(UserID) && !NullOrEmpty(Email)
                                                            && !NullOrEmpty(_password) && !NullOrEmpty(_confirm)
                                                            && _password.Length >= 8;
            }, async p =>
            {
                Loading = Visibility.Visible;
                DatabaseManager db = new DatabaseManager();
                _tempAcc = await db.GetAccountAsync(UserID, MainVM.Type);
                if (_tempAcc != null)
                {
                    if (_tempAcc.Email == Email)
                    {
                        if (!_tempAcc.IsActivated())
                        {
                            if (_password == _confirm)
                            {
                                _tempAcc.ChangePassword(_password);
                                await _tempAcc.SetPassword();
                                Loading = Visibility.Hidden;
                                LabelColor = Brushes.ForestGreen;
                                Noti = "Đăng ký thành công.";
                                await Task.Delay(1500);
                                Noti = "";
                            }
                            else
                            {
                                Loading = Visibility.Hidden;
                                LabelColor = Brushes.OrangeRed;
                                Noti = "Mật khẩu không khớp.";
                                _confirmBox.Clear();
                                await Task.Delay(1500);
                                Noti = "";
                            }
                        }
                        else
                        {
                            Loading = Visibility.Hidden;
                            LabelColor = Brushes.OrangeRed;
                            Noti = "Tài khoản này đã được kích hoạt.";
                            await Task.Delay(1500);
                            Noti = "";
                        }
                    }
                    else
                    {
                        Loading = Visibility.Hidden;
                        LabelColor = Brushes.OrangeRed;
                        Noti = "Email không khớp với tài khoản được cấp quyền.";
                        await Task.Delay(1500);
                        Noti = "";
                    }
                }
                else
                {
                    Loading = Visibility.Hidden;
                    Noti = "Tài khoản không tồn tại.";
                    await Task.Delay(1500);
                    Noti = "";
                }
                Loading = Visibility.Hidden;
            });

            BackCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                ParentVM.CurrentView = ParentVM.WhoAreYouView;
                ResetAll();
            });
        }
        #endregion

        public SetPasswordVM(MainVM vm)
        {
            InitializeCommands();
            ParentVM = vm;
            Loading = Visibility.Hidden;
        }
    }
}
