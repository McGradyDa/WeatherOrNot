using System.Collections.Generic;

namespace Weather
{
    class BasicData
    {
        public static string[] period = { "morning", "day", "sunset", "night" };
        public static string[] weatherType = { "Rain", "Snow", "Clouds", "Thundersnorm" };

        public static List<string> TempUnit = new List<string>() { "°C", "°F" };
        public static List<string> WindUnit = new List<string>() { "km/h", "mi/h" };
        public static Dictionary<string, int> dayOfWeek = new Dictionary<string, int>()
        {
            {"Mon",0 },
            {"Tue",1 },
            {"Wed",2 },
            {"Thu",3 },
            {"Fri",4 },
            {"Sat",5 },
            {"Sun",6 },
        };

        public static string[,] Wind ={
            { "N", "NE", "E", "SE", "S", "SW", "W", "NW" },
            { "北风", "东北风", "东风", "东南风", "南风", "西南风", "西风", "西北风" },
            { "北風", "東北風", "東風", "東南風", "南風", "西南風", "西風", "西北風" },
            { "북풍", "동북풍", "동풍", "동남풍", "남풍", "서남풍", "서풍", "서북풍" },
            { "Norte", "Nordeste", "Oriente", "Sudeste", "Sur", "Suroeste", "Occidental", "Noroeste" },
            { "\xf05c", "\xf05a", "\xf059", "\xf05d", "\xf060", "\xf05e", "\xf061", "\xf05b" }
        };

        public static string[,] DayOfWeek =
        {
            { "Mon","周一" ,"月曜日","월요일","Lunes"},
            { "Tue","周二" ,"火曜日","화요일","Martes"},
            { "Wed","周三" ,"水曜日","수요일","Miércoles"},
            { "Thu","周四" ,"木曜日","목요일","Jueves"},
            { "Fri","周五" ,"金曜日","금요일","Viernes"},
            { "Sat","周六" ,"土曜日","토요일","Sábado"},
            { "Sun","周日" ,"日曜日","일요일","Domingo"},
        };

        public static string[,] LANG =
        {
            { "Settings","设置" ,"設定" ,"설정" ,"Configuración" },
            { "Contact","联系" ,"お問い合わせ" ,"연락" ,"Contacto" },
            { "About","关于","について" ,"소개" ,"Acerca de" },
            { "Language","语言" ,"言語" ,"언어" ,"Idioma" },
            { "Unit","单位","単位" ,"단위" ,"Unidad" },
            { "Search","搜索" ,"検索" ,"검색" ,"Buscar" },
            { "Wind ","风速 ","風速" ,"바람" ,"Viento" },
            { "Hum ","湿度 ","湿度" ,"습도" ,"Humedad" },
            { "Press ","气压 ","圧力" ,"압력" ,"Presión" },
            { "Update","更新时间" ,"更新" ,"업데이트" ,"Update" },
            { "Choose","选择","選ぶ" ,"선택" ,"Escoger" },
            { "You choose:","你选择的位置是:","あなたが選択した場所" ,"위치 당신이 선택" ,"Ubicación que elija" },
            { "Latitude","纬度","緯度" ,"위도" ,"Latitud" },
            { "Longitude","经度","経度" ,"경도" ,"Longitud" },
        };

        public static string[,] Describe =
        {
            {"Tornado" , "龙卷风","竜巻" ,"폭풍" ,"Tornado" },
            {"Tropical storm" , "热带风暴","熱帯暴風雨" ,"열대폭풍" ,"Tormenta tropical" },
            {"Hurricane" , "飓风","ハリケーン" ,"허리케인" ,"Huracán" },
            {"Severe TS" , "雷暴","重度の雷雨" ,"심한뇌우" ,"Tormentas eléctricas severas" },
            {"Thunderstorms" , "雷阵雨","雷雨" ,"천둥번개" ,"tormentas eléctricas" },
            {"Mixed rain snow" , "混合雨雪","混合雨の雪" ,"혼합비가눈" ,"lluvia nieve mezclada" },
            {"Mixed rain sleet" , "混合雨和雨夹雪","混合雨みぞれ" ,"진눈깨비" ,"aguanieve lluvia mezclada" },
            {"Mixed snow sleet" , "混合雪和雨夹雪","ミックス雪みぞれ" ,"진눈깨비" ,"aguanieve nieve mezclada" },
            {"Freezing drizzle" , "冻毛毛雨","凍結霧雨" ,"냉동이슬비" ,"Llovizna helada" },
            {"Drizzle" , "毛毛雨","霧雨" ,"이슬비" ,"Llovizna" },
            {"Freezing rain" , "冻雨","冷たい雨" ,"냉동비" ,"Lluvia helada" },
            {"Showers" , "阵雨","にわか雨" ,"소나기" ,"duchas" },
            {"Showers" , "阵雨","にわか雨" ,"소나기" ,"duchas" },
            {"Snow flurries" , "阵雪","雪の突風" ,"눈돌풍" ,"Copos de nieve" },
            {"Light snow" , "小阵雪","小雪" ,"빛눈" ,"Nieve ligera" },
            {"Blowing snow" , "吹雪","吹いてる雪" ,"날리는눈" ,"La nieve que sopla" },
            {"Snow" , "雪","雪" ,"눈" ,"Nieve" },
            {"Hail" , "冰雹","雹" ,"빗발" ,"Granizo" },
            {"Sleet" , "雨夹雪","みぞれ" ,"진눈깨비" ,"Aguanieve" },
            {"Dust" , "灰尘","ほこり" ,"먼지" ,"Polvo" },
            {"Foggy" , "多雾","フォギー" ,"흐린" ,"Brumoso" },
            {"Haze" , "霾","ヘイズ" ,"안개" ,"Calina" },
            {"Smoky" , "烟","スモーキー" ,"침침한" ,"Ahumado" },
            {"Blustery" , "大风","大風" ,"강풍" ,"Tempestuoso" },
            {"Windy" , "多风","強風" ,"깜짝 놀란" ,"Ventoso" },
            {"Cold" , "冷","コールド" ,"감기" ,"Frío" },
            {"Cloudy" , "多云","曇りました" ,"흐린" ,"Nublado" },
            {"Mostly cloudy (n)" , "多云（夜间）","ほとんど曇り" ,"구름많음" ,"Mayormente nublado" },
            {"Mostly cloudy" , "晴间多云","ほとんど曇り" ,"구름많음" ,"Mayormente nublado" },
            {"Partly cloudy (n)" , "多云（夜间）","曇りました" ,"흐린" ,"Parcialmente nublado" },
            {"Partly cloudy" , "晴间多云","曇りました" ,"흐린" ,"Parcialmente nublado" },
            {"Clear (night)" , "晴（夜间）","クリア" ,"명확한" ,"Claro" },
            {"Sunny" , "阳光明媚","晴れました" ,"맑은" ,"Soleado" },
            {"Fair (night)" , "晴朗（夜间）","フェア" ,"공정한" ,"Justa" },
            {"Fair" , "晴朗","フェア" ,"공정한" ,"Justa" },
            {"Mixed rain hail" , "混合雨和冰雹","混合雨雹" ,"혼합비우박" ,"granizo lluvia mezclada" },
            {"Hot" , "热","ホット" ,"뜨거운" ,"Caliente" },
            {"Isolated TS" , "局部雷暴","孤立した雷雨" ,"맑음" ,"Tormentas aisladas" },
            {"Scattered TS" , "局部雷阵雨","散在雷雨" ,"구름조금" ,"Tormentas eléctricas dispersas" },
            {"Scattered TS" , "局部雷阵雨","散在雷雨" ,"구름조금" ,"Tormentas eléctricas dispersas" },
            {"Scattered showers" , "零星阵雨","散在シャワー" ,"구름조금" ,"Chubascos dispersos" },
            {"Heavy snow" , "大雪","大雪" ,"폭설" ,"Fuertes nevadas" },
            {"Scattered snow" , "局部阵雪","散在雪" ,"산발적눈" ,"nieve dispersos" },
            {"Heavy snow" , "大雪","大雪" ,"폭설" ,"Fuertes nevadas" },
            {"Partly cloudy" , "晴间多云","曇りました" ,"흐린" ,"Parcialmente nublado" },
            {"Thundershowers" , "雷阵雨","雷雨" ,"구름조금" ,"tormentosos" },
            {"Snow showers" , "阵雪","吹雪" ,"눈소나기" ,"Duchas de nieve" },
            {"Isolated TS" , "局部雷阵雨","孤立した雷雨" ,"맑음" ,"Tormentas aisladas" },
            {"Not available" , "不可用","利用不可" ,"사용불가" ,"No disponible" },
        };

        public static Dictionary<string, string> IconDict = new Dictionary<string, string>()
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

        /*
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
        */
    }
}
