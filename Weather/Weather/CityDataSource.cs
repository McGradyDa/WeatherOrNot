using System;
using System.Collections.Generic;
using System.Linq;

namespace Weather
{
    public static class CityDataSource
    {
        private static List<CityData.Place> CitySource = CityData.CitysOfCountry.query.results.place.OrderBy(c => c.name).ToList();
        public static List<CityData.Place> Citys
        {
            get { return CitySource; }
        }

        public static IEnumerable<CityData.Place> GetMatching(string query)
        {
            return Citys
                .Where(c => c.name.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1)
                .OrderByDescending(c => c.name.StartsWith(query, StringComparison.CurrentCultureIgnoreCase))
                .ThenByDescending(c => c.admin3.content.StartsWith(query, StringComparison.CurrentCultureIgnoreCase))
                .ThenByDescending(c => c.admin2.content.StartsWith(query, StringComparison.CurrentCultureIgnoreCase))
                .ThenByDescending(c => c.admin1.content.StartsWith(query, StringComparison.CurrentCultureIgnoreCase))
                .ThenByDescending(c => c.country.content.StartsWith(query, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
