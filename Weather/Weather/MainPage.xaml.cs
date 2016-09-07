using System;
using System.IO;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Text;
using Windows.Storage;
using System.Runtime.Serialization.Json;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Globalization;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
namespace Weather
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public sealed partial class MainPage : Page,INotifyPropertyChanged
    {
        #region For Grid view
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ObservableCollection<Yahoo.Recording> tempF;

        private ObservableCollection<Yahoo.Recording> Forecas
        {
            get { return tempF; }
            set
            {
                tempF = value;
                NotifyPropertyChanged("Forecas");
            }
        }
        #endregion

        #region global variable
        private ObservableCollection<string> LanguageList = new ObservableCollection<string>() { "English", "Chinese" };
        private ObservableCollection<string> UnitsList = new ObservableCollection<string>() { "Metric", "Imperial" };

        private Yahoo.RootObject Yahoo_Data;

        private int UnitsValue;
        private int LanguageValue;
        private string City1;
        private CustomTitleBar customTitleBar = null;
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
            //add custom title bar
            this.AddTitleBar();
            //Read settings and return Weather cache
            string WeatherCache = readSettings();
            TheInitialization(WeatherCache);
            
        }

        /*
         * Add custom title bar 
         */
        public void AddTitleBar()
        {
            customTitleBar = new CustomTitleBar();

            UIElement mainContent = this.Content;
            this.Content = null;
            customTitleBar.SetPageContent(mainContent);
            this.Content = customTitleBar;
        }
        /*
         * clear settings
         */
        private async void clearSettings()
        {
            await ApplicationData.Current.ClearAsync();
        }
        /*
         * Initialization 
         * parse cache to OpenWeatherMapAPI.RootObject or get data from API
         * if api value = 1 
         *    get OMP data
         * else
         *    get Yahoo data
         */
        private void TheInitialization(string WeatherCache)
        {
            bgName.Source = chooseImage();
            Yahoo.RootObject WeatherData = ParseYahooData(WeatherCache);
            UpdateMainWindowWithYahoo(WeatherData);
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
                //Weather Data
                localSettings.Values["WeatherData"] = "";
            }
            UnitsValue = (int)localSettings.Values["Unit"];
            LanguageValue = (int)localSettings.Values["Language"];
            City1 = localSettings.Values["City1"].ToString();
            updateTime.Text = localSettings.Values["UpdateTime"].ToString();

            if (DateTime.UtcNow.Day!= Convert.ToInt32(localSettings.Values["UpdateDay"].ToString()))
            {
                //update time > 1 day ,return null string to tell the method to update
                return "";
            }
            else
                return localSettings.Values["WeatherData"].ToString();
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

        private void UpdateMainWindowWithYahoo(Yahoo.RootObject content)
        {
            Yahoo_Data = content;
            searchbox.PlaceholderText = BasicData.LANG[5, LanguageValue];
            //UTC time offset -> 28800 (China)
            double UtcOffset = TimeZoneInfo.Local.BaseUtcOffset.TotalSeconds;
            
            //Get city data 
            //CityData.readCityData();
            var con = content.query.results.channel;

            CityName.Text = con.location.city;
            string alpha2 = ISO3166.FromName(con.location.country).Alpha2;
            CountryCode.Text = ","+alpha2;
            
            WindSpeed.Text = BasicData.LANG[6, LanguageValue] + con.wind.speed+ BasicData.WindUnit[UnitsValue];
            Humidity.Text = BasicData.LANG[7, LanguageValue] + con.atmosphere.humidity + "%";
            var p = Math.Round(Convert.ToDouble(con.atmosphere.pressure) / 1000.0, 1);
            Pressure.Text = BasicData.LANG[8, LanguageValue] + p.ToString()+"k"+con.units.pressure;
            updateName.Text = BasicData.LANG[9, LanguageValue];

            Tile.TileUpdate();

            TodayDate.Text = DateTime.UtcNow.ToString().Split(' ')[0];
            WeatherIcon.Text= BasicData.IconDict2[con.item.forecast[0].code];
            TempMax.Text = con.item.forecast[0].high + BasicData.TempUnit[UnitsValue];
            TempMin.Text= "/"+con.item.forecast[0].low + BasicData.TempUnit[UnitsValue];
            //day of week
            if (LanguageValue != 0)
                dayofweek.Text = BasicData.DayOfWeek2[con.item.forecast[0].day];
            else
                dayofweek.Text = con.item.forecast[0].day;
            Describe.Text= BasicData.Describe[Convert.ToInt32(con.item.forecast[0].code), LanguageValue];

            var w = (int)((Convert.ToInt32(con.wind.direction) + 22.5) % 360 / 45);
            //wind icon      
            WindIcon.Text = BasicData.Wind[2, w];
            //N or 北风
            WindDegree.Text = BasicData.Wind[LanguageValue, w];

            var c=content.query.results.channel.astronomy.sunrise;
            var s=content.query.results.channel.astronomy.sunset;
            var wea = con.item.forecast.Skip(1).Take(6).ToList();
            string[] CustomColor = { "#00f39f", "#ff91cf", "#1ee7e9", "#9a37c3", "#0693fb", "#fba068" };
            ObservableCollection<Yahoo.Recording> _tempF=new ObservableCollection<Yahoo.Recording>();
            for (int i = 0; i < 6; i++)
            {
                string _day, _temp, _weather, _describe;
                //day of week
                if (LanguageValue != 0)
                    _day = BasicData.DayOfWeek2[wea[i].day];
                else
                    _day = wea[i].day;
                //day temperature
                _temp = wea[i].high + BasicData.TempUnit[UnitsValue] + "/ " + wea[i].low + BasicData.TempUnit[UnitsValue];
                //weather icon
                _weather = BasicData.IconDict2[wea[i].code];
                //weather describe
                _describe = BasicData.Describe[Convert.ToInt32(wea[i].code), LanguageValue];

                _tempF.Add(new Yahoo.Recording() { day = _day, temp = _temp, weather = _weather, describe = _describe,customColor=CustomColor[i] });
            }
            Forecas = _tempF;
        }

        /*
         * Reacquire data and update window
         */
        private void RefreshButton(object sender, RoutedEventArgs e)
        {
            TheInitialization("");
        }
        /*
         * reload window for reloading settings
         */
        private void reloadWindow()
        {
            UpdateMainWindowWithYahoo(Yahoo_Data);
        }

        private BitmapImage chooseImage()
        {
            Random rnd = new Random();
            //if (BasicData.weatherType.Contains(weatherName))
            //    return new BitmapImage(new Uri("ms-appx://Weather/Assets/Weather/" + weatherName + "0.jpg"));
            //else
            return new BitmapImage(new Uri("ms-appx://Weather/Assets/sun" + rnd.Next(0, 4) + ".jpg"));
        }

        #region Suggest Box
        private void SelectCity(CityData.Place city)
        {
            if (city != null)
            {
                CityName.Text = city.name;
                CountryCode.Text = city.country.content;
                City1 = city.woeid;
                TheInitialization("");
            }
        }
        /*
         * Do not use global variable anymore [CitysOfCountry]
         * It will delay and wait the other thread
         * Pass variable and return sorted list
         * Display sender source
         */
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var useless = CityData.getCityData(sender.Text).Result;
                if (useless.query.count != 0)
                {
                    var matchingContacts = CityDataSource.GetMatching(useless.query.results.place.OrderBy(c => c.name).ToList(), sender.Text);
                    //to list override
                    sender.ItemsSource = matchingContacts.ToList();
                }
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                SelectCity(args.ChosenSuggestion as CityData.Place);
            }
            else
            {
                 NoResults.Visibility = Visibility.Visible;
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var c = args.SelectedItem as CityData.Place;

            sender.Text = string.Format("{0} {1}", c.name, c.country.content);
        }
        #endregion

        #region Control Event
        /*
         * Option button click 
         * Initialize the settings control with ObservableCollection
         */
        private void Option_Click(object sender, RoutedEventArgs e)
        {
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

            SettingSplitView.IsPaneOpen = !SettingSplitView.IsPaneOpen;
        }
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
        private void weatherV_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var panel = (ItemsWrapGrid)weatherV.ItemsPanelRoot;
            panel.ItemHeight =bottomPad.ActualHeight-2;
            panel.ItemWidth = bottomPad.ActualWidth/6.0;
        }
        #endregion

        /* OMP 

        private OMP.RootObject ParseOMPData(string cache)
        {
            if (cache == "")
            {
                return OMP.OpenWeatherMapAPI.GetWeatherData(City1, UnitsList[UnitsValue]).Result;
            }
            else
            {
                //parse json data
                var serializer = new DataContractJsonSerializer(typeof(OMP.RootObject));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(cache));
                return (OMP.RootObject)serializer.ReadObject(ms);
            }
        }

        private void UpdateMainWindowWithOMP(OMP.RootObject content)
        {
            OMP_Data = content;
            searchbox.PlaceholderText = BasicData.LANG[5, LanguageValue];
            //Get city data and lat lon
            //CityData.readCityData();

            CityName.Text = content.city.name;
            RegionInfo CountryEN = new RegionInfo(content.city.country);
            CountryName.Text = CountryEN.EnglishName;
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            TodayDate.Text = DateTime.UtcNow.ToString().Split(' ')[0];

            WindSpeed.Text = BasicData.LANG[6, LanguageValue] + content.list[0].speed + BasicData.WindUnit[UnitsValue];
            Humidity.Text = BasicData.LANG[7, LanguageValue] + content.list[0].humidity + "%";
            Pressure.Text = content.list[0].pressure+"hPa";

            var w = (int)((content.list[0].deg + 22.5) % 360 / 45);
            //wind icon      
            WindIcon.Text = BasicData.Wind[2, w];
            //N or 北风
            WindDegree.Text = BasicData.Wind[LanguageValue, w];
            //calculate one time ,in for loop add 1
            var IntDayOfWeek = (int)dtDateTime.AddSeconds(content.list[0].dt).ToLocalTime().DayOfWeek;

            for (int i = 0; i < 6; i++)
            {
                string _day, _temp, _weather, _describe;
                //day of week unix timestamp to day of week

                _day = BasicData.DayOfWeek[LanguageValue, IntDayOfWeek+i];
                //day temperature
                _temp = content.list[i].temp.max + BasicData.TempUnit[UnitsValue] + "/ " + content.list[i].temp.min + BasicData.TempUnit[UnitsValue];
                //weather icon
                _weather = BasicData.IconDict[content.list[i].weather[0].icon];
                //weather describe
                _describe = "";

            }
        }

        */
    }

}
