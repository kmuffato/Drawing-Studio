﻿using HHChaosToolkit.UWP.Services.Navigation;
using LearnDraw.Helpers;
using LearnDraw.MLHelpers;
using LearnDraw.ViewModels;
using System;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace LearnDraw.Views
{
    public sealed partial class ShellPage : Page
    {
        private ShellViewModel ViewModel
        {
            get { return ViewModelLocator.Current.ShellViewModel; }
        }

        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            this.Loaded += ShellPage_Loaded;
        }

        private async void ShellPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigationServiceList.Instance.RegisterOrUpdateFrame(ShellViewModel.ContentNavigationServiceKey, shellFrame);
            NavigationServiceList.Instance[ShellViewModel.ContentNavigationServiceKey].Navigate(typeof(MainViewModel).FullName);
            try
            {
                MLHelper.Instance.Init();
                var assetsListFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appdata:///local/AnimAssets/assetList.json"));
                await AnimAssetsHelper.Instance.Init(assetsListFile);
            }
            catch (Exception)
            {
                //Ignored, the resource may not have been decompressed for the first time
            }
        }
    }
}
