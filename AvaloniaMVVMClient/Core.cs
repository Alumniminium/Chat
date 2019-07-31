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
        public static Config Config = new Config();

        public static readonly Dictionary<ViewModelEnum, (UserControl, ViewModelBase)> Views =
            new Dictionary<ViewModelEnum, (UserControl, ViewModelBase)>
            {
                [ViewModelEnum.Splash] = (new SplashScreenView(), null),
                [ViewModelEnum.Login] = (new LoginView(), new LoginViewModel()),
                [ViewModelEnum.Home] = (new HomeView(), new HomeViewModel()),
            };
    }
}