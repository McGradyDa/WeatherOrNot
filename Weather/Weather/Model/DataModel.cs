using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace Weather.Model
{
    #region CLASS

    public class MainData
    {
        public string cityname { get; set; }
        public string countrycode { get; set; }
        public string updatename { get; set; }
        public string updatetime { get; set; }
        public string windspeed { get; set; }
        public string humidity { get; set; }
        public string pressure { get; set; }
        public string weathericon { get; set; }
        public string describe { get; set; }
        public string tempmax { get; set; }
        public string tempmin { get; set; }
        public string dayofweek { get; set; }
        public string todaydate { get; set; }
        public string windicon { get; set; }
        public string winddegree { get; set; }
        public string sunrise { get; set; }
        public string sunset { get; set; }
        public string placeholdertext { get; set; }
    }

    public class BottomData
    {
        public string day { get; set; }
        public string temp { get; set; }
        public string weather { get; set; }
        public string describe { get; set; }
    }

    #endregion

    public class DataModel : DependencyObject, INotifyPropertyChanged
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

        #region DATA

        private ObservableCollection<BottomData> _bottomdata;
        public ObservableCollection<BottomData> bottomdata
        {
            get { return _bottomdata; }
            set
            {
                _bottomdata = value;
                NotifyPropertyChanged("bottomdata");
            }
        }

        private MainData _maindata;
        public MainData maindata
        {
            get { return _maindata; }
            set
            {
                _maindata = value;
                NotifyPropertyChanged("maindata");
            }
        }

        #endregion

        public DataModel(Yahoo.Channel channel)
        {
            this.maindata = ParseMainData(channel);
            this.bottomdata = parseBottomData(channel.item.forecast.Skip(1).Take(6).ToList());
        }

        private MainData ParseMainData(Yahoo.Channel con)
        {
            string alpha2 = ISO3166.FromName(con.location.country).Alpha2;
            var p = Math.Round(Convert.ToDouble(con.atmosphere.pressure) / 1000.0, 1);
            var w = (int)((Convert.ToInt32(con.wind.direction) + 22.5) % 360 / 45);

            MainData tempMaindata = new MainData()
            {
                cityname = con.location.city,
                countrycode = "." + alpha2,
                updatename = BasicData.LANG[9, MainPage.LanguageValue],
                //updatetime
                updatetime = con.lastBuildDate,
                windspeed = BasicData.LANG[6, MainPage.LanguageValue] + (int)Convert.ToDouble(con.wind.speed) + BasicData.WindUnit[MainPage.UnitsValue],
                humidity = BasicData.LANG[7, MainPage.LanguageValue] + con.atmosphere.humidity + "%",
                pressure = BasicData.LANG[8, MainPage.LanguageValue] + p.ToString() + "k" + con.units.pressure,
                weathericon = BasicData.IconDict[con.item.forecast[0].code],
                describe = BasicData.Describe[Convert.ToInt32(con.item.forecast[0].code), MainPage.LanguageValue],
                tempmax = con.item.forecast[0].high + BasicData.TempUnit[MainPage.UnitsValue],
                tempmin = "/" + con.item.forecast[0].low + BasicData.TempUnit[MainPage.UnitsValue],
                dayofweek = BasicData.DayOfWeek[BasicData.dayOfWeek[con.item.forecast[0].day], MainPage.LanguageValue],
                //todaydate
                //wind icon 
                windicon = BasicData.Wind[5, w],
                //N or 北风
                winddegree = BasicData.Wind[MainPage.LanguageValue, w],
                //sun rise time
                sunrise = con.astronomy.sunrise,
                //sun set time
                sunset = con.astronomy.sunset,
                //place holder text
                placeholdertext = BasicData.LANG[5, MainPage.LanguageValue],
            };
            return tempMaindata;
        }

        private ObservableCollection<BottomData> parseBottomData(System.Collections.Generic.List<Yahoo.Forecast> wea)
        {
            ObservableCollection<BottomData> tempBottomdata = new ObservableCollection<Model.BottomData>();
            for (int i = 0; i < 6; i++)
            {
                string _day, _temp, _weather, _describe;

                //day of week
                if (MainPage.LanguageValue == 0)
                    _day = wea[i].day;
                else
                {
                    _day = BasicData.DayOfWeek[BasicData.dayOfWeek[wea[i].day], MainPage.LanguageValue];
                }

                //day temperature
                _temp = wea[i].high + BasicData.TempUnit[MainPage.UnitsValue] + "/ " + wea[i].low + BasicData.TempUnit[MainPage.UnitsValue];
                //weather icon
                _weather = BasicData.IconDict[wea[i].code];
                //weather describe
                _describe = BasicData.Describe[Convert.ToInt32(wea[i].code), MainPage.LanguageValue];

                tempBottomdata.Add(new BottomData() { day = _day, temp = _temp, weather = _weather, describe = _describe });
            }
            return tempBottomdata;
        }

        #region
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
        #endregion
    }
}
