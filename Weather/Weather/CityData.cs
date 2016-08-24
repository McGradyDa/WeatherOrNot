using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.Storage;

namespace Weather
{
    public class CityData
    {
        public static RootObject cityContent;

        public async static void readCityData()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///city3.json"));
            string contents = await FileIO.ReadTextAsync(file);

            var serializer = new DataContractJsonSerializer(typeof(RootObject));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(contents));
            cityContent = (RootObject)serializer.ReadObject(ms);
        }

        public Tuple<double, double> getCoordinatesFromCity(string cityName)
        {
            City temp = cityContent.city.Find(x => x.name.Contains(cityName));

            return Tuple.Create(temp.coord.lon, temp.coord.lat);
        }

        public int getIdFromCity(string cityName)
        {
            City temp = cityContent.city.Find(x => x.name.Contains(cityName));

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
        }

        [DataContract]
        public class RootObject
        {
            [DataMember]
            public List<City> city { get; set; }
        }
    }
    public class CityTemp
    {
        public string cityN;
        public string countryN;
        public int id;

        public override string ToString()
        {
            return string.Format("{0} ({1})", cityN, countryN);
        }
    }
    public static class CityDataSource
    {

        //private static List<CityData.City> _city = CityData.cityContent.city.OrderBy(x => x.name).ToList();
        private static List<CityTemp> t = new List<CityTemp>();
        public static void convert()
        {
            List<CityTemp> c = new List<CityTemp>();
            for (int i = 0; i < CityData.cityContent.city.Count; i++)
            {
                c.Add(new CityTemp()
                    {
                        cityN = CityData.cityContent.city[i].name,
                        countryN = CityData.cityContent.city[i].country,
                        id = CityData.cityContent.city[i]._id
                    });

            }
           t = c.OrderBy(x => x.cityN).ToList();
        }

        public static List<CityTemp> City
        {
            get { return t; }
        }

        public static IEnumerable<CityTemp> GetMatching(String query)
        {
            return CityDataSource.City
                .Where(c => c.cityN.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1)
                .OrderByDescending(c => c.cityN.StartsWith(query, StringComparison.CurrentCultureIgnoreCase))
                .ThenByDescending(c => c.countryN.StartsWith(query, StringComparison.CurrentCultureIgnoreCase));
        }

    }
}
