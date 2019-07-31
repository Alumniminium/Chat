﻿using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaMVVMClient.UI.ViewModels;

namespace AvaloniaMVVMClient.UI.Views
{
    public class HomeView : UserControl
    {
        public HomeViewModel ViewModel { get; }

        public HomeView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

    }
}
