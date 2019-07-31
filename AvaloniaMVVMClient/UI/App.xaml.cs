using Avalonia;
using Avalonia.Markup.Xaml;

namespace AvaloniaMVVMClient.UI
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
