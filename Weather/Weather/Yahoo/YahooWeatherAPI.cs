using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Weather.Yahoo
{
    public class Recording
    {
        public string day { get; set; }
        public string temp { get; set; }
        public string weather { get; set; }
        public string describe { get; set; }
    }

    public class YahooWeatherAPI
    {
        public async static Task<RootObject> GetWeatherData(string idOrCity, int unit, bool IsCityName)
        {
            return await Task.Run(() =>
            {
                var response = getResponse(idOrCity, unit, IsCityName).Result;
                var result = responseToString(response).Result;
                
                //parse json
                var serializer = new DataContractJsonSerializer(typeof(RootObject));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                var data = (RootObject)serializer.ReadObject(ms);
                //store 
                storeToCache(result);
                return data;
            }).ConfigureAwait(continueOnCapturedContext: false);
        }
        /*
         * store cache to settings
         */
        static void storeToCache(string result)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["WeatherData"] = result;
            //Update weather time
            localSettings.Values["UpdateDay"] = DateTime.UtcNow.Day;
            localSettings.Values["UpdateTime"] = DateTime.Now.ToString();
        }
        /*
         * get http response from yql
         */
        async static Task<HttpResponseMessage> getResponse(string idOrCity, int unit, bool IsCityName)
        {
            string baseUrl = string.Format("https://query.yahooapis.com/v1/public/yql?");
            string unitString = "";
            if (unit == 0)
            {
                unitString = "+and+u%3D%27c%27";
            }
            string mid = "%3D";
            if (IsCityName)
                mid = string.Format("%20in%20(select%20woeid%20from%20geo.places(1)%20where%20text%3D%22{0}%22)", idOrCity);
            else
                mid += idOrCity;
            string yql = string.Format("q=select+*+from+weather.forecast+where+woeid{0}{1}&format=json", mid, unitString);

            return await Task.Run(() =>
            {
                var http = new HttpClient();
                var response = http.GetAsync(baseUrl + yql);
                return response;
            }).ConfigureAwait(continueOnCapturedContext: false);
        }
        /*
         * read string from response 
         */
        async static Task<string> responseToString(HttpResponseMessage c)
        {
            return await Task.Run(() =>
            {
                var v = c.Content.ReadAsStringAsync();  // return c.Content.ReadAsStringAsync(); still not work
                
                return v;
            }).ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    #region

    /*
    "distance": "km",
    "pressure": "mb",
    "speed": "km/h",
    "temperature": "C"
    */

    [DataContract]
    public class Units
    {
        [DataMember]
        public string distance { get; set; }
        [DataMember]
        public string pressure { get; set; }
        [DataMember]
        public string speed { get; set; }
        [DataMember]
        public string temperature { get; set; }
    }

    [DataContract]
    public class Location
    {
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string country { get; set; }
        [DataMember]
        public string region { get; set; }
    }

    [DataContract]
    public class Wind
    {
        [DataMember]
        public string chill { get; set; }
        [DataMember]
        public string direction { get; set; }
        [DataMember]
        public string speed { get; set; }
    }

    [DataContract]
    public class Atmosphere
    {
        [DataMember]
        public string humidity { get; set; }
        [DataMember]
        public string pressure { get; set; }
        [DataMember]
        public string rising { get; set; }
        [DataMember]
        public string visibility { get; set; }
    }

    [DataContract]
    public class Astronomy
    {
        [DataMember]
        public string sunrise { get; set; }
        [DataMember]
        public string sunset { get; set; }
    }

    [DataContract]
    public class Image
    {
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string width { get; set; }
        [DataMember]
        public string height { get; set; }
        [DataMember]
        public string link { get; set; }
        [DataMember]
        public string url { get; set; }
    }

    [DataContract]
    public class Condition
    {
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string date { get; set; }
        [DataMember]
        public string temp { get; set; }
        [DataMember]
        public string text { get; set; }
    }

    [DataContract]
    public class Forecast
    {
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string date { get; set; }
        [DataMember]
        public string day { get; set; }
        [DataMember]
        public string high { get; set; }
        [DataMember]
        public string low { get; set; }
        [DataMember]
        public string text { get; set; }
    }

    [DataContract]
    public class Guid
    {
        [DataMember]
        public string isPermaLink { get; set; }
    }

    [DataContract]
    public class Item
    {
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string lat { get; set; }
        [DataMember]
        public string @long { get; set; }
        [DataMember]
        public string link { get; set; }
        [DataMember]
        public string pubDate { get; set; }
        [DataMember]
        public Condition condition { get; set; }
        [DataMember]
        public List<Forecast> forecast { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public Guid guid { get; set; }
    }

    [DataContract]
    public class Channel
    {
        [DataMember]
        public Units units { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string link { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string language { get; set; }
        [DataMember]
        public string lastBuildDate { get; set; }
        [DataMember]
        public string ttl { get; set; }
        [DataMember]
        public Location location { get; set; }
        [DataMember]
        public Wind wind { get; set; }
        [DataMember]
        public Atmosphere atmosphere { get; set; }
        [DataMember]
        public Astronomy astronomy { get; set; }
        [DataMember]
        public Image image { get; set; }
        [DataMember]
        public Item item { get; set; }
    }

    [DataContract]
    public class Results
    {
        [DataMember]
        public Channel channel { get; set; }
    }

    [DataContract]
    public class Query
    {
        [DataMember]
        public int count { get; set; }
        [DataMember]
        public string created { get; set; }
        [DataMember]
        public string lang { get; set; }
        [DataMember]
        public Results results { get; set; }
    }

    [DataContract]
    public class RootObject
    {
        [DataMember]
        public Query query { get; set; }
    }

    #endregion
}
