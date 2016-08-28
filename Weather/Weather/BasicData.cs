using System.Collections.Generic;

namespace Weather
{
    class BasicData
    {
        public static string[] OpenWeatherIcons = { "01d", "01n", "02d", "02n", "03d", "03n", "04d", "04n", "09d", "09n", "10d", "10n", "11d", "11n", "13d", "13n", "50d", "50n" };
        public static string[] WeatherFontIcons = { "\xf00d", "\xf00d", "\xf002", "\xf002", "\xf041", "\xf041", "\xf013", "\xf013", "\xf006", "\xf006", "\xf019", "\xf019", "\xf010", "\xf010", "\xf01b", "\xf01b", "\xf003", "\xf003", };

        public static Dictionary<string, string> IconDict = new Dictionary<string, string>()
        {
            { "01d","\xf00d"},
            { "01n","\xf00d"},
            { "02d","\xf002"},
            { "02n","\xf002"},
            { "03d","\xf041"},
            { "03n","\xf041"},
            { "04d","\xf013"},
            { "04n","\xf013"},
            { "09d","\xf006"},
            { "09n","\xf006"},
            { "10d","\xf019"},
            { "10n","\xf019"},
            { "11d","\xf010"},
            { "11n","\xf010"},
            { "13d","\xf01b"},
            { "13n","\xf01b"},
            { "50d","\xf003"},
            { "50n","\xf003"},
        };

        public static string[] BlueIcons = { "\xf019", "\xf010", "\xf006" };
        public static string[] WhiteIcons = { "\xf002", "\xf041", "\xf013", "\xf01b", "\xf003" };

        public static string[,] Wind ={
            { "N", "NE", "E", "SE", "S", "SW", "W", "NW" },
            { "北风", "东北风", "东风", "东南风", "南风", "西南风", "西风", "西北风" },
            { "\xf05c", "\xf05a", "\xf059", "\xf05d", "\xf060", "\xf05e", "\xf061", "\xf05b" }
        };
        public static string[,] DayOfWeek ={
            { "MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN" },
            { "周一", "周二", "周三", "周四", "周五", "周六", "周日" }
        };

        public static List<string> TempUnit = new List<string>() { "°C", "°F" };
        public static List<string> WindUnit = new List<string>() { "m/s", "m/h" };

        public static string[] period = { "morning", "day", "sunset", "night" };
        public static string[] weatherType = { "Rain", "Snow", "Clouds", "Thundersnorm" };

        public static string jsonFile = "result.json";

    }
}
