using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.Storage;

namespace Weather
{
    public class CityData
    {
        public static RootObject CitysOfCountry;

        public static async void readCityData()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///city3.json"));
            string contents = await FileIO.ReadTextAsync(file);

            var serializer = new DataContractJsonSerializer(typeof(RootObject));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(contents));
            CitysOfCountry = (RootObject)serializer.ReadObject(ms);
        }

        public Tuple<double, double> getCoordinatesFromCity(string cityName)
        {
            City temp = CitysOfCountry.city.Find(x => x.name.Contains(cityName));

            return Tuple.Create(temp.coord.lon, temp.coord.lat);
        }

        public int getIdFromCity(string cityName)
        {
            City temp = CitysOfCountry.city.Find(x => x.name.Contains(cityName));

            return temp._id;
        }

        [DataContract]
        public class Coord
        {
            [DataMember]
            public double lat { get; set; }
            [DataMember]
            public double lon { get; set; }
        }

        [DataContract]
        public class City
        {
            [DataMember]
            public Coord coord { get; set; }
            [DataMember]
            public string country { get; set; }
            [DataMember]
            public int _id { get; set; }
            [DataMember]
            public string name { get; set; }
            //for autosuggestbox
            public override string ToString()
            {
                return string.Format("{0} ({1})", name, country);
            }
        }

        [DataContract]
        public class RootObject
        {
            [DataMember]
            public List<City> city { get; set; }
        }
    }

}
