using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace Weather
{
    public class Tile
    {
        public static void TileUpdate()
        {
            var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare310x310SmallImagesAndTextList03);

            var tileAttributes = tileXml.GetElementsByTagName("text");
            tileAttributes[0].AppendChild(tileXml.CreateTextNode("da"));
            var tileNotification = new TileNotification(tileXml);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }
    }
}
