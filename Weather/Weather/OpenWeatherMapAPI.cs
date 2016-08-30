using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using Windows.Storage;

namespace Weather
{
    public class OpenWeatherMapAPI
    {
        public async static Task<RootObject> GetWeatherData(int cityID, string APPKEY, string Unit)
        {
            return await Task.Run(() =>
            {
                var response = getResponse(cityID, APPKEY, Unit).Result;
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

        static void storeToCache(string result)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["WeatherData"] = result;
            //Update weather time
            localSettings.Values["UpdateDay"] = System.DateTime.UtcNow.Day;
            localSettings.Values["UpdateTime"] = System.DateTime.Now.ToString();

            ////initailization don't need the data,so needn't use .ConfigureAwait(continueOnCapturedContext: false)
            //Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            //Windows.Storage.StorageFile _file = await storageFolder.CreateFileAsync(BasicData.jsonFile, Windows.Storage.CreationCollisionOption.ReplaceExisting);
            //await Windows.Storage.FileIO.WriteTextAsync(_file, result);
        }

        async static Task<HttpResponseMessage> getResponse(int cityID, string APPKEY, string Unit)
        {
            return await Task.Run(() =>
            {
                var http = new HttpClient();
                string url = string.Format("http://api.openweathermap.org/data/2.5/forecast/daily?id={0}&APPID={1}&units={2}&cnt=7", cityID, APPKEY, Unit);
                var response = http.GetAsync(url);
                return response;
            }).ConfigureAwait(continueOnCapturedContext: false);
        }

        async static Task<string> responseToString(HttpResponseMessage c)
        {
            return await Task.Run(() =>
            {
                var v = c.Content.ReadAsStringAsync();  // return c.Content.ReadAsStringAsync(); still not work
                return v;
            }).ConfigureAwait(continueOnCapturedContext: false);
        }

        [DataContract]
        public class Coord
        {
            [DataMember]
            public double lon { get; set; }
            [DataMember]
            public double lat { get; set; }
        }

        [DataContract]
        public class City
        {
            [DataMember]
            public int id { get; set; }
            [DataMember]
            public string name { get; set; }
            [DataMember]
            public Coord coord { get; set; }
            [DataMember]
            public string country { get; set; }
            [DataMember]
            public int population { get; set; }
        }

        [DataContract]
        public class Temp
        {
            [DataMember]
            public double day { get; set; }
            [DataMember]
            public double min { get; set; }
            [DataMember]
            public double max { get; set; }
            [DataMember]
            public double night { get; set; }
            [DataMember]
            public double eve { get; set; }
            [DataMember]
            public double morn { get; set; }
        }

        [DataContract]
        public class Weather
        {
            [DataMember]
            public int id { get; set; }
            [DataMember]
            public string main { get; set; }
            [DataMember]
            public string description { get; set; }
            [DataMember]
            public string icon { get; set; }
        }

        [DataContract]
        public class List
        {
            [DataMember]
            public int dt { get; set; }
            [DataMember]
            public Temp temp { get; set; }
            [DataMember]
            public double pressure { get; set; }
            [DataMember]
            public int humidity { get; set; }
            [DataMember]
            public List<Weather> weather { get; set; }
            [DataMember]
            public double speed { get; set; }
            [DataMember]
            public int deg { get; set; }
            [DataMember]
            public int clouds { get; set; }
            [DataMember]
            public double? rain { get; set; }
            [DataMember]
            public double? snow { get; set; }
        }

        [DataContract]
        public class RootObject
        {
            [DataMember]
            public City city { get; set; }
            [DataMember]
            public string cod { get; set; }
            [DataMember]
            public double message { get; set; }
            [DataMember]
            public int cnt { get; set; }
            [DataMember]
            public List<List> list { get; set; }
        }

    }

}
