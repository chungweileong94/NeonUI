using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using NeonUI.Extendsions;

namespace NeonUI.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            initilizeTitleBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ContentFrame.Navigated += (s, args) =>
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ContentFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;

                refreshNavRadioState();
            };

            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;

            ContentFrame.Navigate(typeof(HomePage));
            refreshNavRadioState();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
        }

        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (ContentFrame.CanGoBack)
            {
                e.Handled = true;
                ContentFrame.GoBack();
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void NavRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var s = sender as RadioButton;

            switch (s.Content)
            {
                case "Home":
                    ContentFrame.Navigate(typeof(HomePage));
                    break;
                case "Favorite":
                    ContentFrame.Navigate(typeof(FavoritePage));
                    break;
            }
        }

        #region Helpers
        private void initilizeTitleBar()
        {
            var titleBar = CoreApplication.GetCurrentView().TitleBar;
            titleBar.ExtendViewIntoTitleBar = true;

            titleBar.LayoutMetricsChanged += (s, e) =>
            {
                TitleBarGrid.Height = s.Height;
                TitleBarContentGrid.Margin = new Thickness(s.SystemOverlayLeftInset + 12, 0, s.SystemOverlayRightInset - 12, 0);
            };

            var view = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            TitleBarTextBlock.Text = Package.Current.DisplayName;

            view.TitleBar.BackgroundColor = Colors.Transparent;
            view.TitleBar.InactiveBackgroundColor = Colors.Transparent;
            view.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            view.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }

        private void refreshNavRadioState()
        {
            foreach (var rb in this.FindVisualChildList<RadioButton>())
            {
                if (ContentFrame.SourcePageType.Name == $"{rb.Content}Page")
                {
                    rb.IsChecked = true;
                    return;
                }
            }
        }
        #endregion
    }
}
