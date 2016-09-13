using System;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Weather
{
    public sealed partial class MainPage:Page
    {
        #region Control Event

        /*
         * Ellipse pointer entered animation
         */
        private void Ellipse_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            esb1.Begin();
        }
        /*
         * Ellipse pointer exited animation
         */
        private void Ellipse_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            esb2.Begin();
        }
        /*
         * language box selection changed event
         * change the global variable
         * if no select on the combo box,it will return -1
         */
        private void languageBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            if (languageBox.SelectedIndex > -1)
            {
                if (languageBox.SelectedIndex != LanguageValue)
                {
                    LanguageValue = languageBox.SelectedIndex;
                    reloadWindow();
                }
                //modify settings
                localSettings.Values["Language"] = LanguageValue;

            }
        }
        /*
         * Units box selection changed event
         * change the global variable
         * if no select on the combo box,it will return -1
         */
        private void UnitsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            if (UnitsBox.SelectedIndex > -1)
            {
                if (UnitsBox.SelectedIndex != UnitsValue)
                {
                    UnitsValue = UnitsBox.SelectedIndex;
                    reloadWindow();
                }
                //modify settings
                localSettings.Values["Unit"] = UnitsValue;
            }
        }
        /*
         * resize the grid view
         */
        private void rightBottom_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var panel = (ItemsWrapGrid)rightBottom.ItemsPanelRoot;
            panel.ItemHeight = bottomPad.ActualHeight - 2;
            panel.ItemWidth = bottomPad.ActualWidth / 6.0;
        }

        private async void MapForWeather_MapRightTapped(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs args)
        {
            var GeoPosition = args.Location.Position;
            string status = BasicData.LANG[11, LanguageValue] + BasicData.LANG[12, LanguageValue] + GeoPosition.Latitude.ToString().Substring(0, 5) + "," + BasicData.LANG[13, LanguageValue] + GeoPosition.Longitude.ToString().Substring(0, 5);
            MessageDialog showDialog = new MessageDialog(status);

            showDialog.Commands.Add(new UICommand("Yes") { Id = 0 });
            showDialog.Commands.Add(new UICommand("No") { Id = 1 });
            showDialog.DefaultCommandIndex = 1;
            showDialog.CancelCommandIndex = 0;

            var result = await showDialog.ShowAsync();

            if ((int)result.Id == 0)
            {
                string address = string.Format("({0},{1})", GeoPosition.Latitude.ToString(), GeoPosition.Longitude.ToString());
                var data = Yahoo.YahooWeatherAPI.GetWeatherData(address, UnitsValue, true).Result;
                MapSplitView.IsPaneOpen = false;
                UpdateMainWindowWithYahoo(data);
            }

        }

        #endregion
    }
}
