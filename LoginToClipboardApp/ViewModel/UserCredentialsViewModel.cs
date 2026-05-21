using LoginClipboardApp.Model;
using LoginClipboardApp.TOTP;
using System.Windows;

namespace LoginClipboardApp.ViewModel
{
    public class UserCredentialsViewModel : BaseViewModel
    {
        private readonly Timer timer;
        private readonly Authenticator authenticator = new();
        public UserCredentialsViewModel(UserCredentialsProfile profile)
        {
            UserName = profile.email;
            Password = profile.password;
            Secret = profile.totpSecret;

            CopyUserNameCommand = new RelayCommand(CopyUserName);
            CopyPasswordCommand = new RelayCommand(CopyPassword);
            CopyTOTPCommand = new RelayCommand(CopyTOTP);
            CopyClipboardCommand = new RelayCommand(CopyClipboardContent);

            var currentSecond = DateTime.Now.Second;
            var next = (30 - (currentSecond % 30)) * 1000;
            timer = new Timer(_ => UpdateTOTP(), null, next, 30000);
            UpdateTOTP();
        }
    
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Secret { get; set; }
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
