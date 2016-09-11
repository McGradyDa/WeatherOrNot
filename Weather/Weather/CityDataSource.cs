using System;
using System.Collections.Generic;
using System.Linq;

namespace Weather
{
    public static class CityDataSource
    {
        /*
        private static List<CityData.Place> CitySource = CityData.CitysOfCountry.query.results.place.OrderBy(c => c.name).ToList();
        public static List<CityData.Place> Citys
        {
            get { return CitySource; }
        }
        */

        public static IEnumerable<CityData.Place> GetMatching(List<CityData.Place> Citys, string query)
        {
            if (Citys.Count < 5)
            {
                return Citys;
            }
            else
            {
                return Citys
                    .Where(c => c.name.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1)
                    .OrderByDescending(c => c.name.StartsWith(query, StringComparison.CurrentCultureIgnoreCase))
                    .ThenByDescending(c => c.country.content.StartsWith(query, StringComparison.CurrentCultureIgnoreCase));
            }

        }
    }
}
