using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Weather
{
    public class GetCityData
    {
        public async static Task<string> getCityData(string inputString)
        {
            return await Task.Run(() =>
            {
                var response = getResponse(inputString).Result;
                return responseToString(response).Result;

            }).ConfigureAwait(continueOnCapturedContext: false);
        }

        public static T parseJson<T>(string result)
        {
            //parse json
            var serializer = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            return (T)serializer.ReadObject(ms);
        }

        #region get http data
        async static Task<HttpResponseMessage> getResponse(string input)
        {
            string baseUrl = "http://query.yahooapis.com/v1/public/yql?";
            string queryUrl = string.Format("q=select%20*%20from%20geo.places%20where%20text=%27{0}%27&format=json", input);
            System.Diagnostics.Debug.WriteLine("--"+input);
            return await Task.Run(() =>
            {
                var http = new HttpClient();
                var response = http.GetAsync(baseUrl + queryUrl);
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
        #endregion

        #region city data
        [DataContract]
        public class PlaceTypeName
        {
            [DataMember]
            public string code { get; set; }
            [DataMember]
            public string content { get; set; }
        }

        [DataContract]
        public class Country
        {
            [DataMember]
            public string code { get; set; }
            [DataMember]
            public string type { get; set; }
            [DataMember]
            public string woeid { get; set; }
            [DataMember]
            public string content { get; set; }
        }

        [DataContract]
        public class Admin1
        {
            [DataMember]
            public string code { get; set; }
            [DataMember]
            public string type { get; set; }
            [DataMember]
            public string woeid { get; set; }
            [DataMember]
            public string content { get; set; }
        }

        [DataContract]
        public class Admin2
        {
            [DataMember]
            public string code { get; set; }
            [DataMember]
            public string type { get; set; }
            [DataMember]
            public string woeid { get; set; }
            [DataMember]
            public string content { get; set; }
        }

        [DataContract]
        public class Admin3
        {
            [DataMember]
            public string code { get; set; }
            [DataMember]
            public string type { get; set; }
            [DataMember]
            public string woeid { get; set; }
            [DataMember]
            public string content { get; set; }
        }

        [DataContract]
        public class Locality1
        {
            [DataMember]
            public string type { get; set; }
            [DataMember]
            public string woeid { get; set; }
            [DataMember]
            public string content { get; set; }
        }

        [DataContract]
        public class Postal
        {
            [DataMember]
            public string type { get; set; }
            [DataMember]
            public string woeid { get; set; }
            [DataMember]
            public string content { get; set; }
        }

        [DataContract]
        public class Centroid
        {
            [DataMember]
            public string latitude { get; set; }
            [DataMember]
            public string longitude { get; set; }
        }

        [DataContract]
        public class SouthWest
        {
            [DataMember]
            public string latitude { get; set; }
            [DataMember]
            public string longitude { get; set; }
        }

        [DataContract]
        public class NorthEast
        {
            [DataMember]
            public string latitude { get; set; }
            [DataMember]
            public string longitude { get; set; }
        }

        [DataContract]
        public class BoundingBox
        {
            [DataMember]
            public SouthWest southWest { get; set; }
            [DataMember]
            public NorthEast northEast { get; set; }
        }

        [DataContract]
        public class Timezone
        {
            [DataMember]
            public string type { get; set; }
            [DataMember]
            public string woeid { get; set; }
            [DataMember]
            public string content { get; set; }
        }

        [DataContract]
        public class Place
        {
            [DataMember]
            public string lang { get; set; }
            [DataMember]
            public string xmlns { get; set; }
            [DataMember]
            public string yahoo { get; set; }
            [DataMember]
            public string uri { get; set; }
            [DataMember]
            public string woeid { get; set; }
            [DataMember]
            public PlaceTypeName placeTypeName { get; set; }
            [DataMember]
            public string name { get; set; }
            [DataMember]
            public Country country { get; set; }
            [DataMember]
            public Admin1 admin1 { get; set; }
            [DataMember]
            public Admin2 admin2 { get; set; }
            [DataMember]
            public Admin3 admin3 { get; set; }
            [DataMember]
            public Locality1 locality1 { get; set; }
            [DataMember]
            public object locality2 { get; set; }
            [DataMember]
            public Postal postal { get; set; }
            [DataMember]
            public Centroid centroid { get; set; }
            [DataMember]
            public BoundingBox boundingBox { get; set; }
            [DataMember]
            public string areaRank { get; set; }
            [DataMember]
            public string popRank { get; set; }
            [DataMember]
            public Timezone timezone { get; set; }
            //for autosuggestbox
            public override string ToString()
            {
                //fix bug admin==null
                string ad1 = "", ad2 = "";
                if (admin1 != null)
                {
                    ad1 = admin1.content;
                }
                if (admin2 != null)
                {
                    if (admin2.content != ad1 && admin2.content != name)
                    {
                        ad2 = admin2.content;
                    }
                }
                return string.Format("{0} {1} {2} {3}", name, ad1, ad2, country.code);
            }
        }

        [DataContract]
        public class Results
        {
            [DataMember]
            public List<Place> place { get; set; }

        }

        [DataContract]
        public class Results2
        {
            [DataMember]
            public Place place { get; set; }
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
        public class Query2
        {
            [DataMember]
            public int count { get; set; }
            [DataMember]
            public string created { get; set; }
            [DataMember]
            public string lang { get; set; }
            [DataMember]
            public Results2 results { get; set; }
        }

        [DataContract]
        public class RootObject
        {
            [DataMember]
            public Query query { get; set; }
        }

        [DataContract]
        public class RootObject2
        {
            [DataMember]
            public Query2 query { get; set; }
        }
        #endregion

        /*
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
        */

    }

}
