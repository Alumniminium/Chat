using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GUI.Database;
using GUI.UI.ViewModels;
using GUI.UI.Views;

namespace GUI
{
    public class MainWindow : Window
    {
        public static MainWindow Instance;


        public MainWindow()
        {
            Instance = this;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            Content = new LoginView();
            DataContext = new LoginViewModel();
            Closing += (sender, args) => Db.SaveConfig();
            HasSystemDecorations = false;
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
