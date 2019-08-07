using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using AvaloniaMVVMClient.UI.ViewModels;
using AvaloniaMVVMClient.UI.Windows;

namespace AvaloniaMVVMClient.UI.Views
{
    public class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        public async void ServerClick()
        {
            var viewModel = (HomeViewModel)MainWindow.Instance.DataContext;
            
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    viewModel.SelectedServer = viewModel.Servers.First();
                });
        }
    }
}
