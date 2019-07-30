using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaMVVMClient.ViewModels;

namespace AvaloniaMVVMClient.Views
{
    public class LoginView : UserControl
    {
        public readonly LoginViewModel ViewModel;
        public LoginView()
        {
            InitializeComponent();
            DataContext = ViewModel = new LoginViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        public async void Login()
        {
            ViewModel.ProgressbarVisible = true;
            await Task.Delay(3000);
            var user = ViewModel.Username;
            var pass = ViewModel.Password;
            ViewModel.ProgressbarVisible = false;
        }
    }
}
