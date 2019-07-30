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
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static void UpdateContent(UserControl view, ViewModelBase model)
        {
            Instance.DataContext = model;
            Instance.Content = view;
        }
    }
}
