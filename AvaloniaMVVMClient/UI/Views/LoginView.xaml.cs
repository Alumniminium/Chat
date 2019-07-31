using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using AvaloniaMVVMClient.UI.ViewModels;
using AvaloniaMVVMClient.UI.Windows;

namespace AvaloniaMVVMClient.UI.Views
{
    public class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        public void Login()
        {
            var viewModel = (LoginViewModel) MainWindow.Instance.DataContext;

            if (viewModel.RememberCheckbox)
            {
                Core.StateFile.Username = viewModel.Username;
                Core.StateFile.Password = viewModel.Password;
                Core.StateFile.RememberCredentials = true;
            }

            viewModel.ProgressbarVisible = true;
            viewModel.StatusLabel = "Connecting...";
            Core.Client.ConnectAsync(viewModel.Username, viewModel.Password);
            Core.Client.Socket.OnConnected += async () =>
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    viewModel.StatusLabel = "Connected! Logging in...";
                    viewModel.ProgressbarVisible = false;
                });
            };
            Core.Client.OnLoggedIn += async () =>
            {
                await Task.Delay(1000);
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    viewModel.StatusLabel = "Logged in!";
                    MainWindow.UpdateContent(ViewModelEnum.Home);
                });
            };
            Core.Client.Socket.OnDisconnect += async () =>
                {
                    await Dispatcher.UIThread.InvokeAsync(() => { viewModel.ProgressbarVisible = false; });
                };
        }
    }
}
