using System.Collections.Generic;

namespace Weather
{
    class BasicData
    {
        public static string[] BlueIcons = { "\xf019", "\xf010", "\xf006" };
        public static string[] WhiteIcons = { "\xf002", "\xf041", "\xf013", "\xf01b", "\xf003" };

        public static string[] BlueIcons2 =
            {"3","4","5","6","7","8","9","10","11","12","17","18","35","37","38","39","40","45","47"};
        public static string[] YellowIcons2 =
            { "31","32","33","34","36"};

        public static string[,] Wind ={
            { "N", "NE", "E", "SE", "S", "SW", "W", "NW" },
            { "北风", "东北风", "东风", "东南风", "南风", "西南风", "西风", "西北风" },
            { "\xf05c", "\xf05a", "\xf059", "\xf05d", "\xf060", "\xf05e", "\xf061", "\xf05b" }
        };
        public static string[,] DayOfWeek ={
            { "MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN" },
            { "周一", "周二", "周三", "周四", "周五", "周六", "周日" }
        };
        public static Dictionary<string, string> DayOfWeek2 = new Dictionary<string, string>()
        {
            {"Mon","周一" }, {"Tue","周二" },  {"Wed","周三" },  {"Thu","周四" },  {"Fri","周五" }, {"Sat","周六" },  {"Sun","周日" },
        };
        public static List<string> TempUnit = new List<string>() { "°C", "°F" };
        public static List<string> WindUnit = new List<string>() { "km/h", "mi/h" };

        public static string[] period = { "morning", "day", "sunset", "night" };
        public static string[] weatherType = { "Rain", "Snow", "Clouds", "Thundersnorm" };

        public static string[,] LANG =
        {
            {"Settings","设置" },
            {"Contact","联系" },
            {"About","关于"},
            {"Language","语言" },
            {"Unit","单位"},
            {"Search","搜索" },
            { "Wind ","风速 "},
            { "Hum ","湿度 "},
            { "Press ","气压 "},
            {"Update","更新时间" },
        };
        public static string[,] Describe =
        {
            {"Tornado" , "龙卷风" },
            {"Tropical storm" , "热带风暴" },
            {"Hurricane" , "飓风" },
            {"Severe TS" , "雷暴" },
            {"Thunderstorms" , "雷阵雨" },
            {"Mixed rain snow" , "混合雨雪" },
            {"Mixed rain sleet" , "混合雨和雨夹雪" },
            {"Mixed snow sleet" , "混合雪和雨夹雪" },
            {"Freezing drizzle" , "冻毛毛雨" },
            {"Drizzle" , "毛毛雨" },
            {"Freezing rain" , "冻雨" },
            {"Showers" , "阵雨" },
            {"Showers" , "阵雨" },
            {"Snow flurries" , "阵雪" },
            {"Light snow" , "小阵雪" },
            {"Blowing snow" , "吹雪" },
            {"Snow" , "雪" },
            {"Hail" , "冰雹" },
            {"Sleet" , "雨夹雪" },
            {"Dust" , "灰尘" },
            {"Foggy" , "多雾" },
            {"Haze" , "霾" },
            {"Smoky" , "烟" },
            {"Blustery" , "大风" },
            {"Windy" , "多风" },
            {"Cold" , "冷" },
            {"Cloudy" , "多云" },
            {"Mostly cloudy (n)" , "多云（夜间）" },
            {"Mostly cloudy" , "晴间多云" },
            {"Partly cloudy (n)" , "多云（夜间）" },
            {"Partly cloudy" , "晴间多云" },
            {"Clear (night)" , "晴（夜间）" },
            {"Sunny" , "阳光明媚" },
            {"Fair (night)" , "晴朗（夜间）" },
            {"Fair" , "晴朗" },
            {"Mixed rain hail" , "混合雨和冰雹" },
            {"Hot" , "热" },
            {"Isolated TS" , "局部雷暴" },
            {"Scattered TS" , "局部雷阵雨" },
            {"Scattered TS" , "局部雷阵雨" },
            {"Scattered showers" , "零星阵雨" },
            {"Heavy snow" , "大雪" },
            {"Scattered snow" , "局部阵雪" },
            {"Heavy snow" , "大雪" },
            {"Partly cloudy" , "晴间多云" },
            {"Thundershowers" , "雷阵雨" },
            {"Snow showers" , "阵雪" },
            {"Isolated TS" , "局部雷阵雨" },
            {"Not available" , "不可用" },
        };
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

        public static Dictionary<string, string> IconDict2 = new Dictionary<string, string>()
        {
            {"0","\xf056"},
            {"1","\xf00e"},
            {"2","\xf073"},
            {"3","\xf01e"},
            {"4","\xf01e"},
            {"5","\xf017"},
            {"6","\xf017"},
            {"7","\xf017"},
            {"8","\xf015"},
            {"9","\xf01a"},
            {"10","\xf015"},
            {"11","\xf01a"},
            {"12","\xf01a"},
            {"13","\xf01b"},
            {"14","\xf00a"},
            {"15","\xf064"},
            {"16","\xf01b"},
            {"17","\xf015"},
            {"18","\xf017"},
            {"19","\xf063"},
            {"20","\xf014"},
            {"21","\xf021"},
            {"22","\xf062"},
            {"23","\xf050"},
            {"24","\xf050"},
            {"25","\xf076"},
            {"26","\xf013"},
            {"27","\xf031"},
            {"28","\xf002"},
            {"29","\xf031"},
            {"30","\xf002"},
            {"31","\xf02e"},
            {"32","\xf00d"},
            {"33","\xf083"},
            {"34","\xf00c"},
            {"35","\xf017"},
            {"36","\xf072"},
            {"37","\xf00e"},
            {"38","\xf00e"},
            {"39","\xf00e"},
            {"40","\xf01a"},
            {"41","\xf064"},
            {"42","\xf01b"},
            {"43","\xf064"},
            {"44","\xf00c"},
            {"45","\xf00e"},
            {"46","\xf01b"},
            {"47","\xf00e"},
            {"3200","\xf077"},
        };

    }
}
