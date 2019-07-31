using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AvaloniaMVVMClient.UI.ViewModels;

namespace AvaloniaMVVMClient.UI
{
    public class ViewLocator : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public IControl Build(object data)
        {
            var fullName = data.GetType().FullName;

            if (fullName == null)
                throw new NullReferenceException("data.GetType().FullName == null");

            var name = fullName.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type);
            }
            else
            {
                return new TextBlock { Text = "Not Found: " + name };
            }
        }

        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}