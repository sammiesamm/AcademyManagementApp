using AcademyManager.Models;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace AcademyManager.Viewmodels
{
    public class ForgetPassVM : BaseViewModel
    {
        #region Commands
        public ICommand BackCommand { get; set; }
        public ICommand SendCodeCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand ConfirmPasswordChangedCommand { get; set; }
        public ICommand ConfirmCodeCommand { get; set; }
        public ICommand ResetPasswordCommand { get; set; }
        #endregion
        #region Properties
        private MainVM ParentVM { get; set; }
        private Account _tempAcc;
        private string _userid;
        private string _email;
        private string _password;
        private string _confirm;
        private string _noti;
        private string _authcode;
        private string _inputcode;
        private PasswordBox _passwordBox, _confirmBox;
        private bool _step1enable;
        private bool _step2enable;
        private bool _step3enable;
        private int _currstep;
        private int _expireT;
        private string _timelb;
        private DispatcherTimer _timer;
        private Visibility _load1, _load2;
        public Visibility Loading1
        {
            get { return _load1; }
            set { _load1 = value; OnPropertyChanged(); }
        }
        public Visibility Loading2
        {
            get { return _load2; }
            set { _load2 = value; OnPropertyChanged(); }
        }
        public string TimeLabel
        {
            get { return _timelb; }
            set { _timelb = value; OnPropertyChanged(); }
        }
        public string InputCode
        {
            get { return _inputcode; }
            set { _inputcode = value; OnPropertyChanged(); }
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
        public int CurrentStep
        {
            get { return _currstep; }
            set { _currstep = value; OnPropertyChanged(); }
        }
        public bool Step1Enable
        {
            get { return _step1enable; }
            set { _step1enable = value; OnPropertyChanged(); }
        }
        public bool Step2Enable
        {
            get { return _step2enable; }
            set { _step2enable = value; OnPropertyChanged(); }
        }
        public bool Step3Enable
        {
            get { return _step3enable; }
            set { _step3enable = value; OnPropertyChanged(); }
        }
        #endregion
        #region Methods
        private void ResetAll()
        {
            UserID = "";
            Email = "";
            _expireT = 300;
            Step1Enable = true;
            Step2Enable = false;
            Step3Enable = false;
            if (_passwordBox != null) _passwordBox.Clear();
            if (_confirmBox != null) _confirmBox.Clear();
        }
        private bool NullOrEmpty(string s)
        {
            return (s == null || s == String.Empty);
        }
        private void SetTime()
        {
            int min = _expireT / 60;
            int sec = _expireT % 60;
            TimeLabel = $"{min:D2}:{sec:D2}";
        }
        private async Task<int> SendCodeToEmail()
        {
            // generate code
            Random rd = new Random();
            int randomCode = rd.Next(100000, 999999);
            _authcode = randomCode.ToString();

            // Sender info
            string senderEmail = Authentication.Email;
            string senderPassword = "zaknmguipunfxnpd";

            // Recipient's email address check
            DatabaseManager db = new DatabaseManager();
            _tempAcc = await db.GetAccountAsync(_userid, MainVM.Type);
            if (_tempAcc != null)
            {
                if (_tempAcc.Email != _email)
                {
                    return -1;
                }
            }
            else return -1;

            if (!_tempAcc.IsActivated()) return -3;


            // Email configuration
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587; // Gmail SMTP port
            smtpClient.EnableSsl = true; // Use SSL
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

            // Email message prepare and check email format
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(senderEmail);
            try
            {
                mailMessage.To.Add(_email);
            }
            catch (FormatException)
            {
                return -2;
            }
            mailMessage.Subject = "Reset password";
            mailMessage.Body = $"This is security code to reset your password: {randomCode}\n" +
                               $"Please do not share it with anyone.";

            // Send email
            try
            {
                // Send the email
                await smtpClient.SendMailAsync(mailMessage);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private void InitializeCommands()
        {
            BackCommand = new RelayCommand<object>(p => true, p =>
            {
                ParentVM.CurrentView = ParentVM.WhoAreYouView;
                ResetAll();
            });

            SendCodeCommand = new RelayCommand<object>(p => { return !NullOrEmpty(_userid) && !NullOrEmpty(_email) && _expireT > 0; }, async p =>
            {
                Loading1 = Visibility.Visible;
                int result = await SendCodeToEmail();
                Loading1 = Visibility.Hidden;
                if (result == 0) // sending error
                {
                    Noti = "Không thể gửi mã xác nhận.";
                    await Task.Delay(1500);
                    Noti = "";
                }
                else if (result == -1)
                {
                    Noti = "Thông tin không hợp lệ.";
                    await Task.Delay(1500);
                    Noti = "";
                }
                else if (result == -2)
                {
                    Noti = "Email không hợp lệ.";
                    await Task.Delay(1500);
                    Noti = "";
                }
                else if (result == -3)
                {
                    Noti = "Tài khoản chưa được kích hoạt.";
                    await Task.Delay(1500);
                    Noti = "";
                }
                else
                {
                    Step2Enable = true;
                    CurrentStep = 1;
                    Step1Enable = false;
                    _expireT = 300;
                    _timer.Start();
                }
            });

            PasswordChangedCommand = new RelayCommand<PasswordBox>(p => true, p =>
            {
                _passwordBox = p;
                _password = p.Password;
            });

            ConfirmPasswordChangedCommand = new RelayCommand<PasswordBox>(p => true, p =>
            {
                _confirmBox = p;
                _confirm = p.Password;
            });

            ConfirmCodeCommand = new RelayCommand<object>(p => !NullOrEmpty(_inputcode), async p =>
            {
                if (InputCode == _authcode)
                {
                    Step3Enable = true;
                    CurrentStep = 2;
                    Step2Enable = false;
                }
                else
                {
                    Noti = "Mã xác nhận không hợp lệ.";
                    await Task.Delay(1500);
                    Noti = "";
                    InputCode = "";
                }
            });

            ResetPasswordCommand = new RelayCommand<object>(p => !NullOrEmpty(_password) && !NullOrEmpty(_confirm) && _password.Length >= 8, async p =>
            {
                Loading2 = Visibility.Visible;
                if (_password == _confirm)
                {
                    DatabaseManager db = new DatabaseManager();
                    _tempAcc.ChangePassword(_password);
                    await _tempAcc.SetPassword();
                    BackCommand.Execute(null);
                }
                else
                {
                    Loading2 = Visibility.Hidden;
                    Noti = "Mật khẩu không khớp.";
                    _confirmBox.Clear();
                    await Task.Delay(1500);
                    Noti = "";
                }
                Loading2 = Visibility.Hidden;
            });
        }
        #endregion
        public ForgetPassVM(MainVM vm)
        {
            ParentVM = vm;
            CurrentStep = 0;
            Step1Enable = true;
            Step2Enable = false;
            Step3Enable = false;
            Loading1 = Loading2 = Visibility.Hidden;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += _timer_Tick;
            _expireT = 300;
            InitializeCommands();
        }

        private void _timer_Tick(object? sender, EventArgs e)
        {
            _expireT--;
            SetTime();
            if (_expireT == 0)
            {
                _timer.Stop();
                _authcode = "";
            }
        }
    }
}
