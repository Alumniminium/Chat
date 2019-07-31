using System.Collections.ObjectModel;
using AvaloniaMVVMClient.Networking.Entities;

namespace AvaloniaMVVMClient.UI.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private ObservableCollection<VirtualServer> _servers = new ObservableCollection<VirtualServer>();

        public ObservableCollection<VirtualServer> Servers
        {
            get => _servers;
            set => _servers = value;
        }
    }
}