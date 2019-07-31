using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaMVVMClient.Database;
using AvaloniaMVVMClient.UI.ViewModels;
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
            switch (model)
            {
                case ViewModelEnum.Splash:
                    Instance.Content = new SplashScreenView();
                    Instance.DataContext = null;
                    break;
                case ViewModelEnum.Login:
                    Instance.Content = new LoginView();
                    Instance.DataContext = new LoginViewModel();
                    break;
                case ViewModelEnum.Home:
                    Instance.Content = new HomeView();
                    Instance.DataContext = new HomeViewModel();
                    break;
            }
        }
    }
}
