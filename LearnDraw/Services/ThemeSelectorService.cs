﻿using LearnDraw.Core.Models;
using LearnDraw.Helpers;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace LearnDraw.Services
{
    public static class ThemeSelectorService
    {
        private const string SettingsKey = SettingsContract.AppBackgroundRequestedTheme;

        public static ElementTheme Theme { get; set; } = ElementTheme.Default;

        public static async Task InitializeAsync()
        {
            Theme = await LoadThemeFromSettingsAsync();
        }

        public static async Task SetThemeAsync(ElementTheme theme)
        {
            Theme = theme;

            await SetRequestedThemeAsync();
            await SaveThemeInSettingsAsync(Theme);
        }

        public static async Task SetRequestedThemeAsync()
        {
            foreach (var view in CoreApplication.Views)
            {
                await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (Window.Current.Content is FrameworkElement frameworkElement)
                    {
                        frameworkElement.RequestedTheme = Theme;
                        var viewTitleBar = ApplicationView.GetForCurrentView().TitleBar;
                        viewTitleBar.ButtonForegroundColor = Theme == ElementTheme.Default ? (Color)App.Current.Resources["SystemBaseHighColor"]
                        : (Theme == ElementTheme.Light ? Colors.Black : Colors.White);
                    }
                });
            }
        }

        private static async Task<ElementTheme> LoadThemeFromSettingsAsync()
        {
            ElementTheme cacheTheme = ElementTheme.Default;
            string themeName = await ApplicationData.Current.LocalSettings.ReadAsync<string>(SettingsKey);

            if (!string.IsNullOrEmpty(themeName))
            {
                Enum.TryParse(themeName, out cacheTheme);
            }

            return cacheTheme;
        }

        private static async Task SaveThemeInSettingsAsync(ElementTheme theme)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(SettingsKey, theme.ToString());
        }
    }
}
