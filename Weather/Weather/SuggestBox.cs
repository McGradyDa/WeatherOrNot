using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Weather
{
    public sealed partial class MainPage:Page
    {
        #region Suggest Box
        private void SelectCity(CityData.Place city)
        {
            if (city != null)
            {
                CityName.Text = city.name;
                CountryCode.Text = city.country.content;
                City1 = city.woeid;
                TheInitialization("");
            }
        }
        /*
         * Do not use global variable anymore [CitysOfCountry]
         * It will delay and wait the other thread
         * Pass variable and return sorted list
         * Display sender source
         */
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput && !string.IsNullOrWhiteSpace(sender.Text))
            {
                var useless = CityData.getCityData(sender.Text).Result;
                // RootObject mostly and then RootObject2
                var dataContract = CityData.parseJson<CityData.RootObject>(useless);
                if (dataContract.query.count != 0)
                {
                    if (dataContract.query.count != 1)
                    {
                        var matchingContacts = CityDataSource.GetMatching(dataContract.query.results.place.OrderBy(c => c.name).ToList(), sender.Text);
                        //to list override
                        sender.ItemsSource = matchingContacts.ToList();
                    }
                    else
                    {
                        var dataContract2 = CityData.parseJson<CityData.RootObject2>(useless);
                        if (dataContract.query.count != 0)
                        {
                            System.Collections.Generic.List<CityData.Place> c = new System.Collections.Generic.List<CityData.Place>();
                            c.Add(dataContract2.query.results.place);
                            sender.ItemsSource = c;
                        }
                    }
                }


            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                SelectCity(args.ChosenSuggestion as CityData.Place);
            }
            else
            {
                NoResults.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }
        /*
         * remove this function on xaml
         */
        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var c = args.SelectedItem as CityData.Place;

            sender.Text = string.Format("{0} {1}", c.name, c.country.content);
        }
        #endregion
    }
}
