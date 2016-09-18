using System;
using System.IO;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Text;
using Windows.Storage;
using System.Runtime.Serialization.Json;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
namespace Weather
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public sealed partial class MainPage : Page ,INotifyPropertyChanged
    {
        #region INPC

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region global variable
        private Yahoo.RootObject Yahoo_Data;
        private CustomTitle customTitle = null;
        public static MainPage Current;

        public static int UnitsValue;
        public static int LanguageValue;
        public static double LatValue;
        public static double LongValue;
        private bool isHideUpdateTime;
        private bool isHideSunGraph;
        public static string City1;

        private Model.DataModel _Model;
        public Model.DataModel Model
        {
            get { return _Model; }
            set
            {
                _Model = value;
                NotifyPropertyChanged("Model");
            }
        }
        #endregion

        public MainPage()
        {
            //Transparent
            ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            formattableTitleBar.ButtonBackgroundColor = Colors.Transparent;
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            //White minisize
            formattableTitleBar.ButtonForegroundColor = Colors.White;
            //windows size
            ApplicationView.PreferredLaunchViewSize = new Size(1040, 660);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            //clearSettings();
            this.InitializeComponent();
            AddTitleBar();
            this.Loaded += OnLoaded;
            MapForWeather.MapRightTapped += MapForWeather_MapRightTapped;
            sunAnimatePath();
            //Read settings and return Weather cache
            string WeatherCache = readSettings();
            TheInitialization(WeatherCache);
            Current = this;
        }

        /*
         * Add custom title bar 
         * Make the main page's content a child of the title bar,
         * and make the title bar the new page content.
         */
        private void AddTitleBar()
        {
            customTitle = new CustomTitle();
            customTitle.EnableControlsInTitleBar();

            UIElement mainContent = this.Content;
            this.Content = null;
            customTitle.SetPageContent(mainContent);
            this.Content = customTitle;
        }
        /*
         * clear settings
         */
        private async void clearSettings()
        {
            await ApplicationData.Current.ClearAsync();
        }
        /*
         * ContainsKey to determine whether need to initialize settings or not 
         * Initialize the default settings, 
         * Language is the system language,
         * Unit "Metric" or "Imperial",
         * City1 ID default 2151330 (Beijing),
         * if DateNow - UpdateTime >1 day 
         *     update now
         * else 
         *     read WeatherData
        */
        private string readSettings()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            if (!localSettings.Values.ContainsKey("language"))
            {
                //Language
                if (Windows.Globalization.Language.CurrentInputMethodLanguageTag.Substring(0, 2) == "zh")
                    localSettings.Values["Language"] = 1;
                else
                    localSettings.Values["Language"] = 0;
                //Unit
                localSettings.Values["Unit"] = 0;
                //City1 ID
                localSettings.Values["City1"] = "2151330";
                //Update weather time               
                //31
                localSettings.Values["UpdateDay"] = DateTime.UtcNow.Day;
                //2016 / 8 / 31 20:07:20
                localSettings.Values["UpdateTime"] = DateTime.Now.ToString();
                //hide update time
                localSettings.Values["isHideUpdateTime"] = false;
                //hide sun graph
                localSettings.Values["isHideSunGraph"] = false;
                //Weather Data
                localSettings.Values["WeatherData"] = "";
            }
            UnitsValue = (int)localSettings.Values["Unit"];
            LanguageValue = (int)localSettings.Values["Language"];
            City1 = localSettings.Values["City1"].ToString();
            //UpdateTime.Text = localSettings.Values["UpdateTime"].ToString();
            isHideUpdateTime = (bool)localSettings.Values["isHideUpdateTime"];
            isHideSunGraph = (bool)localSettings.Values["isHideSunGraph"];

            if (DateTime.UtcNow.Day!= Convert.ToInt32(localSettings.Values["UpdateDay"].ToString()))
            {
                //update time > 1 day ,return null string to tell the method to update
                return "";
            }
            else
                return localSettings.Values["WeatherData"].ToString();
        }
        /*
         * Initialization 
         * parse cache to OpenWeatherMapAPI.RootObject or get data from API
         *    get Yahoo data
         */
        private void TheInitialization(string WeatherCache)
        {
            Initialize_MainPage();
            Yahoo.RootObject WeatherData = ParseYahooData(WeatherCache);
            UpdateMainWindowWithYahoo(WeatherData);
        }
        /*
         * Initialize setting page
         * Initialize the settings control with ObservableCollection
         * give the initialization value 
         */
        private void Initialize_SplitPage()
        {
            ObservableCollection<string> LanguageList = new ObservableCollection<string>() { "English", "中文", "日本語", "한국어"};
            ObservableCollection<string> UnitsList = new ObservableCollection<string>() { "Metric", "Imperial" };
            //Initialize the settings control
            languageBox.DataContext = LanguageList;
            UnitsBox.DataContext = UnitsList;

            pivotSetting.Header = BasicData.LANG[0, LanguageValue];
            pivotContact.Header = BasicData.LANG[1, LanguageValue];
            pivotAbout.Header = BasicData.LANG[2, LanguageValue];
            languageBox.Header = BasicData.LANG[3, LanguageValue];
            UnitsBox.Header = BasicData.LANG[4, LanguageValue];

            //selected Item will call languageBox_SelectionChanged function to select
            languageBox.SelectedItem = LanguageList[LanguageValue];
            UnitsBox.SelectedItem = UnitsList[UnitsValue];
            //toggle switch
            isHideSunToggle.IsOn = isHideSunGraph;
            isHideSunToggle.Header = BasicData.LANG[14,LanguageValue];
            //toggle switch
            isHideUpdateToggle.IsOn = isHideUpdateTime;
            isHideUpdateToggle.Header = BasicData.LANG[15,LanguageValue];


        }
        /*
         * Initialize the mainpage without weather data
         * Initialize searchbox PlaceholderText with languageValue
         * Initialize todaydate
         */
        private void Initialize_MainPage()
        {
            TodayDate.Text = DateTime.UtcNow.ToString().Split(' ')[0];
            //map control service token 
            MapForWeather.MapServiceToken = "zXyw9tFY6elmsJpCHtYY~qkcxn2idzgrLpurme3Au2Q~ArO1fMroeEhh65R5UPd9Dkjxp3K2oPZeyXqdOaFqvtNNiFrJ-RMYAJ9gWaDizuUu";
            MapForWeather.DesiredPitch = 54;
            // retrieve map
            MapForWeather.ZoomLevel = 12;
            if (isHideSunGraph)
            {
                sunGrid.Visibility = Visibility.Collapsed;
            }
            if (isHideUpdateTime)
            {
                UpdateName.Visibility = Visibility.Collapsed;
                UpdateTime.Visibility = Visibility.Collapsed;
            }
        }
        /*
         * parse Yahoo json data
         */
        private Yahoo.RootObject ParseYahooData(string cache)
        {
            if (cache == "")
            {
                int n;
                //identify if a string is a number
                bool isString = !int.TryParse(City1, out n);
                return Yahoo.YahooWeatherAPI.GetWeatherData(City1, UnitsValue, isString).Result;
            }
            else
            {
                //parse json data
                var serializer = new DataContractJsonSerializer(typeof(Yahoo.RootObject));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(cache));
                return (Yahoo.RootObject)serializer.ReadObject(ms);
            }
        }
        /*
         * Update main window 
         * Initialize control
         *  -> searchbox
         *  -> CityName
         *  -> CountryCode
         *  -> WindSpeed
         *  -> Humidity
         *  -> Pressure
         *  -> TodayDate
         *  -> WeatherIcon
         *  -> TempMax
         *  -> TempMin
         *  -> dayofweek
         */
        private void UpdateMainWindowWithYahoo(Yahoo.RootObject content)
        {
            //UTC time offset -> 28800 (China)
            //double UtcOffset = TimeZoneInfo.Local.BaseUtcOffset.TotalSeconds;
            Model.TileModel temp = new Model.TileModel(content.query.results.channel);
            var channel = content.query.results.channel;
            Yahoo_Data = content;
            // latitude and longitude
            LatValue = Convert.ToDouble(channel.item.lat);
            LongValue = Convert.ToDouble(channel.item.@long);

            Initialize_SplitPage();
            startAnimation(channel.astronomy.sunrise, channel.astronomy.sunset);

            Model = new Model.DataModel(channel);
        }
        /*
         * Initialize the sun graph
         * start sunrise animation
         */
        private void startAnimation(string riseTime, string setTime)
        {
            int _rise = Convert.ToInt32(riseTime.Split(':')[0]);
            int _set = Convert.ToInt32(setTime.Split(':')[0]) + 12;

            //DateTime.UtcNow.Hour -> 2 DateTime.Now.Hour ->  10
            if (DateTime.Now.Hour < _rise)
            {
                sunAnimation.To = 0;
            }
            else if (DateTime.Now.Hour > _set)
            {
                sunAnimation.To = 180;
            }
            else
            {
                //(DateTime.Now.Hour - _rise) / (_set - _rise) * 180.0 ------>5/13=0  0*180=0
                sunAnimation.To = (DateTime.Now.Hour - _rise) * 180.0 / (_set - _rise);
            }
            sunStoryboard.Begin();
        }
        /*
         * reload window for reloading settings
         */
        private void reloadWindow()
        {
            UpdateMainWindowWithYahoo(Yahoo_Data);
        }
    }
}
