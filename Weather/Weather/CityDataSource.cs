using System;
using System.Collections.Generic;
using System.Linq;

namespace Weather
{
    public static class CityDataSource
    {
        private static List<CityData.City> CitySource = CityData.CitysOfCountry.city.OrderBy(c => c.name).ToList();
        public static List<CityData.City> Citys
        {
            get { return CitySource; }
        }

        public static IEnumerable<CityData.City> GetMatching(string query)
        {
            return Citys
                .Where(c => c.name.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1)
                .OrderByDescending(c => c.name.StartsWith(query, StringComparison.CurrentCultureIgnoreCase))
                .ThenByDescending(c => c.country.StartsWith(query, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
