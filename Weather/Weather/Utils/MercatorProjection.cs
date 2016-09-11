using System;

namespace Weather
{
    class MercatorProjection
    {
        public static int ACT_WIDTH = 933;
        public static int ACT_HEIGHT = 272;

        public static Tuple<double, double> calculateCoordinate(double lon, double lat)
        {
            int IMG_WIDTH = 1023;
            int IMG_HEIGHT = 1025;
            double PI = 3.14159265359;

            //int x = (int)((IMG_WIDTH / 360.0) * (180 + lon));
            //int y = (int)((IMG_HEIGHT / 180.0) * (90 - lat));  //270

            double x = (lon + 180) * (IMG_WIDTH / 360.0);

            double latRad = lat * PI / 180.0;
            double mercN = Math.Log(Math.Tan((PI / 4.0) + (latRad / 2.0)));
            double y = (IMG_HEIGHT / 2.0) - (IMG_WIDTH * mercN / (2.0 * PI)) - 263;  //623-452=171  364-272=92

            return Tuple.Create(x, y);
        }

    }
}
