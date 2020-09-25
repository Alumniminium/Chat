using System.Collections.ObjectModel;
using GUI.Networking.Entities;
using ReactiveUI;

namespace GUI.UI.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private ObservableCollection<VirtualServer> _servers = new ObservableCollection<VirtualServer>();
        private VirtualServer _selectedServer;

        public ObservableCollection<VirtualServer> Servers
        {
            get => _servers;
            set => _servers = value;
        }

        public VirtualServer SelectedServer
        {
            get => _selectedServer;
            set => this.RaiseAndSetIfChanged(ref _selectedServer, value);
        }
    }
}