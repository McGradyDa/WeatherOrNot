using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;

namespace Weather
{
    class DataFromAPI
    {
        public async static Task<RootObject> GetWeatherData(int cityID, string APPKEY, string Unit)
        {
            var http = new HttpClient();
            string url = string.Format("http://api.openweathermap.org/data/2.5/forecast/daily?id={0}&APPID={1}&units={2}&cnt=7", cityID, APPKEY, Unit);
            var response = await http.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            //write to file
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile _file = await storageFolder.CreateFileAsync(DataLib.jsonFile, Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(_file, result);

            var serializer = new DataContractJsonSerializer(typeof(RootObject));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObject)serializer.ReadObject(ms);

            return data;
        }
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
