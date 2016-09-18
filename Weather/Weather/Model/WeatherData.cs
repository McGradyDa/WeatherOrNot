using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Model
{
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
}
