using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaMVVMClient.ViewModels;

namespace AvaloniaMVVMClient.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            HasSystemDecorations = false;
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private async void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            await Task.Delay(2000);
            Content = new LoginView();
            DataContext = new LoginViewModel();
        }
    }
}
