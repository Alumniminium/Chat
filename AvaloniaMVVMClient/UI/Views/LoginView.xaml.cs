using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaMVVMClient.UI.ViewModels;

namespace AvaloniaMVVMClient.UI.Views
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
        public void Login()
        {
            ViewModel.ProgressbarVisible = true;
            Core.Client.ConnectAsync(ViewModel.Username, ViewModel.Password);
            Core.Client.Socket.OnConnected += () => { ViewModel.ProgressbarVisible = false; };
        }
    }
}
