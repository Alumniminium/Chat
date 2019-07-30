using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaMVVMClient.ViewModels;
using AvaloniaMVVMClient.Windows;

namespace AvaloniaMVVMClient.Views
{
    public class SplashScreenView : UserControl
    {
        public SplashScreenView()
        {
            InitializeComponent();
        }
        private async void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            await Task.Delay(2000);
            MainWindow.UpdateContent(new LoginView(), new LoginViewModel());
        }
    }
}
