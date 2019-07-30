namespace AvaloniaMVVMClient.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public string UsernameLabel => "Username";
        public string PasswordLabel => "Password";
        public string RememberCheckboxContent => "Remember credentials?";
        public string LoginButtonContent => "Login";

        public string Password { get; set; }
        public string Username { get; set; }
        public bool RememberCheckbox { get; set; }
    }
}