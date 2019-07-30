using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaMVVMClient.ViewModels;
using AvaloniaMVVMClient.Views;

namespace AvaloniaMVVMClient.Windows
{
    public class MainWindow : Window
    {
        public static MainWindow Instance;
        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            HasSystemDecorations = false;
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Instance = this;
            Content = new SplashScreenView();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static void UpdateContent(ViewModelBase model, UserControl view)
        {
            Instance.DataContext = model;
            Instance.Content = view;
        }
    }
}
