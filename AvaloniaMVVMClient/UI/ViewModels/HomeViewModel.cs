using ReactiveUI;

namespace AvaloniaMVVMClient.UI.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private string _usernameLabel = "Username";
        private string _passwordLabel = "Username";
        private string _rememberCheckboxContent = "Remember credentials?";
        private string _loginButtonContent = "Login";
        private string _password;
        private string _username;
        private bool _rememberCheckbox;
        private bool _progressbarVisible;

        public string UsernameLabel
        {
            get => _usernameLabel;
            set => this.RaiseAndSetIfChanged(ref _usernameLabel, value);
        }
        public string PasswordLabel
        {
            get => _passwordLabel;
            set => this.RaiseAndSetIfChanged(ref _passwordLabel, value);
        }
        public string RememberCheckboxContent
        {
            get => _rememberCheckboxContent;
            set => this.RaiseAndSetIfChanged(ref _usernameLabel, value);
        }

        public string LoginButtonContent
        {
            get => _loginButtonContent;
            set => this.RaiseAndSetIfChanged(ref _loginButtonContent, value);
        }

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public string Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }

        public bool RememberCheckbox
        {
            get => _rememberCheckbox;
            set => this.RaiseAndSetIfChanged(ref _rememberCheckbox, value);
        }

        public bool ProgressbarVisible
        {
            get => _progressbarVisible;
            set => this.RaiseAndSetIfChanged(ref _progressbarVisible, value);
        }
    }
}