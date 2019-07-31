using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaMVVMClient.UI.Windows;

namespace AvaloniaMVVMClient.UI.Views
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
            await Task.Delay(1000);
            MainWindow.UpdateContent(ViewModelEnum.Login);
        }
    }
}
