using System;
using NotificationsExtensions.Tiles;
using Windows.UI.Notifications;
using System.Collections.Generic;
using NotificationsExtensions;

namespace Weather
{
    public class Tile
    {
        private List<string> rain = new List<string> { "3", "4", "8", "9", "10", "11", "12", "35", "37", "38", "39", "40", "45", "47" };
        private List<string> snow = new List<string> { "5", "6", "7", "13", "14", "15", "16", "18", "41", "42", "43", "46" };
        private List<string> cloud = new List<string> { "20", "22", "26", "27", "28", "29", "30" };
        //private List<string> fair = new List<string> { "31", "32", "33", "34" };

        public Tile(Yahoo.Channel channel)
        {
            UpdateMedium(channel);
        }

        private string chooseBackground(string code)
        {
            string img, baseFolder = "Assets/Tile/";
            if (rain.Contains(code))
            {
                img = "Rain0.jpg";
            }
            else if (snow.Contains(code))
            {
                img = "Snow0.jpg";
            }
            else if (cloud.Contains(code))
            {
                img = "Clouds0.jpg";
            }
            else
            {
                if (DateTime.Now.Hour > 6 && DateTime.Now.Hour < 8)
                    img = "morning0.jpg";
                else if (DateTime.Now.Hour > 6 && DateTime.Now.Hour < 15)
                    img = "day0.jpg";
                else if (DateTime.Now.Hour > 15 && DateTime.Now.Hour < 18)
                    img = "sunset0.jpg";
                else
                    img = "night0.jpg";
            }
            return baseFolder + img;
        }

        private void UpdateMedium(Yahoo.Channel channel)
        {
            List<Yahoo.Forecast> weather = channel.item.forecast;
            string bg = chooseBackground(weather[0].code);
            TileContent content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    #region medium tile 

                    TileMedium = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            BackgroundImage = new TileBackgroundImage()
                            {
                                Source = bg,
                                HintOverlay = 50
                            },
                            Children =
                            {
                                 new AdaptiveGroup()
                                 {
                                    Children =
                                    {
                                        new AdaptiveSubgroup()
                                        {
                                            HintWeight = 2,
                                            Children=
                                            {
                                                new AdaptiveText()
                                                {
                                                    Text = channel.item.condition.temp+"°",
                                                    HintStyle = AdaptiveTextStyle.Title,
                                                    HintAlign=AdaptiveTextAlign.Left
                                                },
                                                new AdaptiveText()
                                                {
                                                    Text=channel.location.city,
                                                    HintStyle=AdaptiveTextStyle.Caption,
                                                    HintAlign=AdaptiveTextAlign.Right
                                                },
                                                new AdaptiveText()
                                                {
                                                    Text=weather[0].high+"°/"+weather[0].low,
                                                    HintStyle=AdaptiveTextStyle.Caption,
                                                    HintAlign=AdaptiveTextAlign.Right
                                                },
                                                new AdaptiveText()
                                                {
                                                    Text=BasicData.Describe[Convert.ToInt32(channel.item.forecast[0].code), MainPage.LanguageValue],
                                                    HintAlign=AdaptiveTextAlign.Left
                                                }

                                            }
                                        },
                                    }
                                 }
                            }
                        }
                    },

                    #endregion

                    #region wide tile

                    TileWide = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            BackgroundImage = new TileBackgroundImage()
                            {
                                Source = bg,
                                HintOverlay = 50
                            },
                            Children =
                            {
                                new AdaptiveGroup()
                                {
                                    Children =
                                    {
                                        new AdaptiveSubgroup()
                                        {
                                            HintWeight = 2,
                                            Children=
                                            {
                                                new AdaptiveText()
                                                {
                                                    Text = channel.item.condition.temp+"°",
                                                    HintStyle = AdaptiveTextStyle.Subheader,
                                                    HintAlign=AdaptiveTextAlign.Left
                                                },
                                                new AdaptiveText()
                                                {
                                                    Text=BasicData.Describe[Convert.ToInt32(channel.item.forecast[0].code), MainPage.LanguageValue],
                                                    HintAlign=AdaptiveTextAlign.Right
                                                }
                                           
                                            }
                                        },
                                        CreateSubgroup2(weather[0].day, weather[0].code, weather[0].high, weather[0].low),
                                        CreateSubgroup2(weather[1].day, weather[1].code, weather[1].high, weather[1].low),
                                        CreateSubgroup2(weather[2].day, weather[2].code, weather[2].high, weather[2].low),
                                    }
                                },
                                new AdaptiveText()
                                {
                                    Text = channel.wind.speed + BasicData.WindUnit[MainPage.UnitsValue],
                                }
                            }
                        }
                    },

                    #endregion

                    #region large tile

                    TileLarge = new TileBinding()
                    {
                        DisplayName= BasicData.Describe[Convert.ToInt32(channel.item.forecast[0].code), MainPage.LanguageValue],
                        Content = new TileBindingContentAdaptive()
                        {
                            BackgroundImage = new TileBackgroundImage()
                            {
                                Source = bg,
                                HintOverlay = 50
                            },
                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text=channel.location.city+"."+ISO3166.FromName(channel.location.country).Alpha2,
                                    HintStyle=AdaptiveTextStyle.Title

                                },
                                new AdaptiveText()
                                {
                                    Text=channel.item.condition.temp+"°",
                                    HintAlign=AdaptiveTextAlign.Right,
                                    HintStyle=AdaptiveTextStyle.Header
                                },
                                new AdaptiveGroup()
                                {
                                    Children =
                                    {
                                        CreateSubgroup(weather[0].day, weather[0].code, weather[0].high, weather[0].low),
                                        CreateSubgroup(weather[1].day, weather[1].code, weather[1].high, weather[1].low),
                                        CreateSubgroup(weather[2].day, weather[2].code, weather[2].high, weather[2].low),
                                        CreateSubgroup(weather[3].day, weather[3].code, weather[3].high, weather[3].low),
                                        CreateSubgroup(weather[4].day, weather[4].code, weather[4].high, weather[4].low)
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                }
            };

            // And send the notification
            TileUpdateManager.CreateTileUpdaterForApplication().Update(new TileNotification(content.GetXml()));
        }

        private AdaptiveSubgroup CreateSubgroup(string day, string code, string highTemp, string lowTemp)
        {
            return new AdaptiveSubgroup()
            {
                HintWeight = 1,
                Children =
                {
                    new AdaptiveText()
                    {
                        Text = BasicData.DayOfWeek[BasicData.dayOfWeek[day],MainPage.LanguageValue],
                        HintAlign = AdaptiveTextAlign.Center
                    },

                    new AdaptiveImage()
                    {
                        Source = string.Format("Assets/ICON/{0}.png",code),
                        HintRemoveMargin = true
                    },

                    new AdaptiveText()
                    {
                        Text = highTemp + "°",
                        HintAlign = AdaptiveTextAlign.Center
                    },

                    new AdaptiveText()
                    {
                        Text = lowTemp + "°",
                        HintAlign = AdaptiveTextAlign.Center,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    }
                }
            };
        }

        private AdaptiveSubgroup CreateSubgroup2(string day, string code, string highTemp, string lowTemp)
        {
            return new AdaptiveSubgroup()
            {
                HintWeight = 1,
                Children =
                {
                    new AdaptiveText()
                    {
                        Text = BasicData.DayOfWeek[BasicData.dayOfWeek[day],MainPage.LanguageValue],
                        HintAlign = AdaptiveTextAlign.Center
                    },

                    new AdaptiveImage()
                    {
                        Source = string.Format("Assets/ICON/{0}.png",code),
                        HintRemoveMargin = true
                    },

                    new AdaptiveText()
                    {
                        Text = (int)((Convert.ToInt32(highTemp) + Convert.ToInt32(lowTemp)) / 2.0) + "°",
                        HintAlign = AdaptiveTextAlign.Center
                    },

                }
            };
        }
    }
}
