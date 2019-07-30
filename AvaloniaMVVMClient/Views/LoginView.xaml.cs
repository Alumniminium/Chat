using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaMVVMClient.Views
{
    public class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
