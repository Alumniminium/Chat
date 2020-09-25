using System.Collections.Generic;
using Avalonia.Controls;
using GUI.Networking;
using GUI.Networking.Entities;
using GUI.UI.ViewModels;
using GUI.UI.Views;

namespace GUI
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