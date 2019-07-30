using AvaloniaMVVMClient.Views;
using AvaloniaMVVMClient.Windows;
using ReactiveUI;

namespace AvaloniaMVVMClient.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            MainWindow.UpdateContent(new SplashScreenView(), null);
        }
        public async void Login()
        {

        }
    }
}
