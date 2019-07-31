using System.Collections.Generic;
using Avalonia.Controls;
using AvaloniaMVVMClient.Networking.Entities;
using AvaloniaMVVMClient.UI.ViewModels;
using AvaloniaMVVMClient.UI.Views;

namespace AvaloniaMVVMClient
{
    public static class Core
    {
        public static readonly Networking.Client Client = new Networking.Client();
        public static VirtualServer SelectedServer = null;
        public static Channel SelectedChannel =null;
        public static User MyUser = new User();
        public const string SERVER_IP = "127.0.0.1";
        public const ushort SERVER_PORT = 65535;

        public static Dictionary<ViewModelEnum, (UserControl,ViewModelBase)> Views = new Dictionary<ViewModelEnum, (UserControl, ViewModelBase)>
        {
            [ViewModelEnum.Splash] = ( new SplashScreenView(), null),
            [ViewModelEnum.Login] = (new LoginView(),new LoginViewModel()),
            [ViewModelEnum.Home] = (new HomeView(), new HomeViewModel()),
        };
    }
}