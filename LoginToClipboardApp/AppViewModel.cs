using LoginClipboardApp.TOTP;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Threading;

namespace LoginClipboardApp
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
    public class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool>? canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action execute)
        {
            this.execute = execute;
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object? parameter)
        {
            return this.canExecute?.Invoke() != false;
        }

        public void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                this.execute();
            }
        }
    }


    public class AppViewModel : BaseViewModel
    {
        private readonly Timer timer;
        private readonly Authenticator authenticator = new();
        public AppViewModel()
        {
            UserName = LoginClipboardApp.Properties.Settings.Default.User;
            Password = LoginClipboardApp.Properties.Settings.Default.Password;
            Secret = LoginClipboardApp.Properties.Settings.Default.TotpSecret;
            CopyUserNameCommand = new RelayCommand(CopyUserName);
            CopyPasswordCommand = new RelayCommand(CopyPassword);
            CopyTOTPCommand = new RelayCommand(CopyTOTP);
            CopyClipboardCommand = new RelayCommand(CopyClipboardContent);

            var currentSecond = DateTime.Now.Second;
            var next = (30-(currentSecond % 30))*1000;
            timer = new Timer(_ => UpdateTOTP(), null, next, 30000);
            UpdateTOTP();
        }

        public string UserName { get; set; } 
        public string Password { get; set; }
        private string Secret { get; set; }
        public string ClipboardContent { get; set; } = string.Empty;
        public string TOTP { get; set; } = string.Empty;
        

        public RelayCommand CopyUserNameCommand { get; set; }
        public void CopyUserName()
        {
            this.ClipboardContent = Clipboard.ContainsText() ? Clipboard.GetText() : string.Empty;
            OnPropertyChanged(nameof(ClipboardContent));
            SetClipboard(UserName);
        }

        public RelayCommand CopyPasswordCommand { get; set; }
        public void CopyPassword()
        {
            SetClipboard(Password);
        }

        public RelayCommand CopyTOTPCommand { get; set; }
        public void CopyTOTP()
        {
            SetClipboard(TOTP);
        }

        public RelayCommand CopyClipboardCommand { get; set; }

        public void CopyClipboardContent()
        {
            SetClipboard(ClipboardContent);
            this.ClipboardContent = string.Empty;
            OnPropertyChanged(nameof(ClipboardContent));
        }   

        private void UpdateTOTP()
        {
            TOTP = authenticator.GenerateTimeBasedPassword(Secret);
            OnPropertyChanged(nameof(TOTP));
        }

        private static void SetClipboard(string clipboard)
        {
            Clipboard.Clear();
            Clipboard.SetText(clipboard);
            Clipboard.Flush();
        }
    }
}
