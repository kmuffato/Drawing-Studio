﻿using CommonServiceLocator;
using HHChaosToolkit.UWP.Mvvm;
using HHChaosToolkit.UWP.Services;
using HHChaosToolkit.UWP.Services.Navigation;
using LearnDraw.Core.Models;
using LearnDraw.ViewModels.PickerViewModels;
using LearnDraw.Views;
using LearnDraw.Views.Pickers;
using Unity;
using Unity.Lifetime;
using Unity.ServiceLocation;

namespace LearnDraw.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        private static ViewModelLocator _current;

        public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());

        private ViewModelLocator()
        {
            if (!ServiceLocator.IsLocationProviderSet)
            {
                InitViewModelLocator();
            }
        }
        private IUnityContainer _container;
        public void InitViewModelLocator()
        {
            _container = new UnityContainer();
            var _serviceLocator = new UnityServiceLocator(_container);
            ServiceLocator.SetLocatorProvider(() => _serviceLocator);

            _container.RegisterType<ObjectPickerService>(new ContainerControlledLifetimeManager())
                .RegisterType<SubWindowsService>(new ContainerControlledLifetimeManager())
                .RegisterType<ShellViewModel>(new ContainerControlledLifetimeManager());

            RegisterNavigationService<MainViewModel, MainPage>(ShellViewModel.ContentNavigationServiceKey);
            RegisterNavigationService<AnimDrawingViewModel, AnimDrawingPage>(ShellViewModel.ContentNavigationServiceKey);

            RegisterObjectPicker<bool, UnpackResViewModel, UnpackResPage>();
            RegisterObjectPicker<bool, SettingsViewModel, SettingsPage>();
            RegisterObjectPicker<bool, AboutViewModel, AboutPage>();
            RegisterObjectPicker<bool, StartScreenViewModel, StartScreenPage>();
            RegisterObjectPicker<bool, GuideVideoViewModel, GuideVideoPage>();

            RegisterObjectPicker<AnimConfig, AnimSettingsViewModel, AnimSettingsPage>();
            RegisterObjectPicker<ArtDrawing, MyFavoriteDrawingsViewModel, MyFavoriteDrawingsPage>();
        }

        public ObjectPickerService ObjectPickerService => ServiceLocator.Current.GetInstance<ObjectPickerService>();
        public SubWindowsService SubWindowsService => ServiceLocator.Current.GetInstance<SubWindowsService>();

        public ShellViewModel ShellViewModel => ServiceLocator.Current.GetInstance<ShellViewModel>();
        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
        public AnimDrawingViewModel AnimDrawingViewModel => ServiceLocator.Current.GetInstance<AnimDrawingViewModel>();
        public SettingsViewModel SettingsViewModel => ServiceLocator.Current.GetInstance<SettingsViewModel>();
        public AboutViewModel AboutViewModel => ServiceLocator.Current.GetInstance<AboutViewModel>();
        public StartScreenViewModel StartScreenViewModel => ServiceLocator.Current.GetInstance<StartScreenViewModel>();
        public GuideVideoViewModel GuideVideoViewModel => ServiceLocator.Current.GetInstance<GuideVideoViewModel>();
        public AnimSettingsViewModel AnimSettingsViewModel => ServiceLocator.Current.GetInstance<AnimSettingsViewModel>();
        public MyFavoriteDrawingsViewModel MyFavoriteDrawingsViewModel => ServiceLocator.Current.GetInstance<MyFavoriteDrawingsViewModel>();
        public UnpackResViewModel FirstRunViewModel => ServiceLocator.Current.GetInstance<UnpackResViewModel>();

        public void RegisterNavigationService<VM, V>(string nsKey)
            where VM : ViewModelBase
        {
            _container.RegisterType<VM>(new ContainerControlledLifetimeManager());
            if (!NavigationServiceList.Instance.IsRegistered(nsKey))
                NavigationServiceList.Instance.Register(nsKey, new NavigationService());
            var contentService = NavigationServiceList.Instance[nsKey];
            contentService.Configure(typeof(VM).FullName, typeof(V));
        }
        public void RegisterObjectPicker<T, VM, V>()
            where VM : ObjectPickerBase<T>
        {
            _container.RegisterType<VM>(new ContainerControlledLifetimeManager());
            ObjectPickerService.Configure(typeof(T).FullName, typeof(VM).FullName, typeof(V));
        }
        public void RegisterSubWindow<VM, V>()
            where VM : SubWindowBase
        {
            _container.RegisterType<VM>();
            SubWindowsService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
