using System;
using BCSTech.Services.Predefined;
using BCSTech.ViewModels;
using BCSTech.Views;
using Prism;
using Prism.Ioc;
using Prism.Plugin.Popups;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BCSTech
{
    public partial class App
    {
        #region device scale
        public static float ScreenHeight { get; set; }
        public static float ScreenWidth { get; set; }
        public static float DeviceScale { get; set; }
        public static double StatusBarHeight { get; set; }
        public static double NativeScale { get; set; }
        public static double AppScale { get; set; }
        public static double ScreenScale { get { return (ScreenHeight + ScreenHeight) / (320.0f + 568.0f); } }
        #endregion

        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();
            // Instantiate possible services (Session, Permissions, Socket, Analytics, Subscription Keys, etc)

            // Page Redirections
            NavigationService.NavigateAsync(Constants.SearchPage);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>(Constants.NAVIGATION_PAGE);
            containerRegistry.RegisterForNavigation<SearchPage, SearchPageViewModel>(Constants.SearchPage);
            containerRegistry.RegisterForNavigation<CustomerDetailPage, CustomerDetailPageViewModel>(Constants.CustomerDetailPage);
        }
    }
}
