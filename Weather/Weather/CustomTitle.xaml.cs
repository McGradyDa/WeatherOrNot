using System;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Weather
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomTitle : Page
    {
        public CustomTitle()
        {
            this.InitializeComponent();
            bgName.Source = chooseImage();
        }
        /* 
         * Clicks on the BackgroundElement will be treated as clicks on the title bar.
         */
        public void EnableControlsInTitleBar()
        {
            Window.Current.SetTitleBar(BackgroundElement);
        }
        /*
         * Set page content
         */
        UIElement pageContent = null;
        public UIElement SetPageContent(UIElement newContent)
        {
            UIElement oldContent = pageContent;
            if (oldContent != null)
            {
                pageContent = null;
                RootGrid.Children.Remove(oldContent);
            }
            pageContent = newContent;
            if (newContent != null)
            {
                RootGrid.Children.Add(newContent);
                // The page content is row 1 in our grid. (See diagram above.)
                Grid.SetRow((FrameworkElement)pageContent, 1);
            }
            return oldContent;
        }

        private BitmapImage chooseImage()
        {
            Random rnd = new Random();
            //if (BasicData.weatherType.Contains(weatherName))
            //    return new BitmapImage(new Uri("ms-appx://Weather/Assets/Weather/" + weatherName + "0.jpg"));
            //else
            return new BitmapImage(new Uri("ms-appx://Weather/Assets/sun" + rnd.Next(0, 4) + ".jpg"));
        }

        private void FreshButton_Click(object sender, RoutedEventArgs e)
        {

        }
        RandomAccessStreamReference mapIconStreamReference;
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
            mapIconStreamReference = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/locate.png"));
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
    }
}
