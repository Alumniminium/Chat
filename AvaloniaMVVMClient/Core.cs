using System.Collections.Generic;
using Avalonia.Controls;
using AvaloniaMVVMClient.Networking;
using AvaloniaMVVMClient.Networking.Entities;
using AvaloniaMVVMClient.UI.ViewModels;
using AvaloniaMVVMClient.UI.Views;

namespace AvaloniaMVVMClient
{
    public static class Core
    {
        public static readonly Client Client = new Client();
        public static VirtualServer SelectedServer = null;
        public static Channel SelectedChannel = null;
        public static User MyUser = new User();
        public static StateFile StateFile = new StateFile();
    }
}