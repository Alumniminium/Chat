using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaMVVMClient.ViewModels;

namespace AvaloniaMVVMClient.Views
{
    public class HomeView : UserControl
    {
        public readonly HomeViewModel ViewModel;
        public HomeView()
        {
            InitializeComponent();
            DataContext = ViewModel = new HomeViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

    }
}
