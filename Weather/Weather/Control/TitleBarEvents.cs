using System;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Weather
{
    public sealed partial class CustomTitle : Page
    {
        private void Mapbutton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.MapSplitView.IsPaneOpen = !MainPage.Current.MapSplitView.IsPaneOpen;
            MainPage.Current.MapSplitView.OpenPaneLength = MainPage.Current.ActualWidth / 18.2 * 12;
            MainPage.Current.MapForWeather.Center =
                new Windows.Devices.Geolocation.Geopoint(new Windows.Devices.Geolocation.BasicGeoposition()
                {
                    Latitude = MainPage.LatValue,
                    Longitude = MainPage.LongValue

                });
            putLocateIcon();
        }

        RandomAccessStreamReference mapIconStreamReference;
        private void putLocateIcon()
        {
            mapIconStreamReference = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/locate2.png"));
            Windows.UI.Xaml.Controls.Maps.MapIcon mapIcon1 = new Windows.UI.Xaml.Controls.Maps.MapIcon();
            mapIcon1.Location = MainPage.Current.MapForWeather.Center;
            mapIcon1.NormalizedAnchorPoint = new Windows.Foundation.Point(0.5, 1.0);
            mapIcon1.Image = mapIconStreamReference;
            mapIcon1.ZIndex = 0;
            MainPage.Current.MapForWeather.MapElements.Add(mapIcon1);
        }

        private void Option_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.SettingSplitView.IsPaneOpen = !MainPage.Current.SettingSplitView.IsPaneOpen;
            MainPage.Current.SettingSplitView.OpenPaneLength = MainPage.Current.ActualWidth / 18.2 * 6.2;

        }

        private void SwitchBg_Click(object sender, RoutedEventArgs e)
        {
            bgName.Source = chooseImage();
        }

        private BitmapImage chooseImage()
        {
            Random rnd = new Random();
            //if (BasicData.weatherType.Contains(weatherName))
            //    return new BitmapImage(new Uri("ms-appx://Weather/Assets/Weather/" + weatherName + "0.jpg"));
            //else
            return new BitmapImage(new Uri("ms-appx://Weather/Assets/Background/sun" + rnd.Next(0, 4) + ".jpg"));
        }
    }
}
