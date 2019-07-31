using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaMVVMClient.Database;
using AvaloniaMVVMClient.UI.Views;

namespace AvaloniaMVVMClient.UI.Windows
{
    public class MainWindow : Window
    {
        public static MainWindow Instance;


        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            HasSystemDecorations = false;
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Instance = this;
            Content = new SplashScreenView();
            Closing += (sender, args) => Db.SaveConfig();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static void UpdateContent(ViewModelEnum model)
        {
            if (!Core.Views.TryGetValue(model, out var pair))
                return;
            Instance.Content = pair.Item1;
            Instance.DataContext = pair.Item2;
        }
    }
}
