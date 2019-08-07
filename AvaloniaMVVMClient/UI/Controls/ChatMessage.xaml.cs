using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaMVVMClient.UI.Controls
{
    public class ChatMessage : UserControl
    {
        public string AvatarUrl { get; set; }
        public string Message { get; set; }
        public ChatMessage()
        {
            this.InitializeComponent();
            DataContext = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
