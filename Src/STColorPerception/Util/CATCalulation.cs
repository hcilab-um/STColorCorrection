using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STColorPerception.Util
{
    class CATCalulation
    {
        public static PerceptionLib.CIEXYZ bradford(PerceptionLib.CIEXYZ ColorIntended)
        {
            double a1 = 1.167956107031635, a2 = -0.031264500636418, a3 = 0.074388489007966,
                   b1 = -0.039681841508073, b2 = 1.244711477209885, b3 = 0.025079590194737,
                   c1 = 0.014012126634923, c2 = -0.023035447192908, c3 = 1.607562008000194;

            PerceptionLib.CIEXYZ colorObject = new PerceptionLib.CIEXYZ(0,0,0);
            colorObject.X = (a1 * ColorIntended.X) + (a2 * ColorIntended.Y) + (a3 * ColorIntended.Z);
            colorObject.Y = (b1 * ColorIntended.X) + (b2 * ColorIntended.Y) + (b3 * ColorIntended.Z);
            colorObject.Z = (c1 * ColorIntended.X) + (c2 * ColorIntended.Y) + (c3 * ColorIntended.Z);
            return colorObject;
        }

        public static PerceptionLib.CIEXYZ VonKries(PerceptionLib.CIEXYZ ColorIntended)
        {
            double a1 = 1.206614453867343, a2 = -0.072440269712756, a3 = 0.078685502523226,
                   b1 = -0.007957049834924, b2 = 1.233521284718770, b3 = 0.001604519150054,
                   c1 = 0, c2 = 0, c3 = 1.596314269377941;

            PerceptionLib.CIEXYZ colorObject = new PerceptionLib.CIEXYZ(0, 0, 0);
            colorObject.X = (a1 * ColorIntended.X) + (a2 * ColorIntended.Y) + (a3 * ColorIntended.Z);
            colorObject.Y = (b1 * ColorIntended.X) + (b2 * ColorIntended.Y) + (b3 * ColorIntended.Z);
            colorObject.Z = (c1 * ColorIntended.X) + (c2 * ColorIntended.Y) + (c3 * ColorIntended.Z);
            return colorObject;
        }

        public static PerceptionLib.CIEXYZ XYZScaling(PerceptionLib.CIEXYZ ColorIntended)
        {
            double a1 = 1.199845990708948, a2 = 0, a3 = 0,
                   b1 = 0, b2 = 1.227129376250138, b3 = 0,
                   c1 = 0, c2 = 0, c3 = 1.596314269377941;

            PerceptionLib.CIEXYZ colorObject = new PerceptionLib.CIEXYZ(0, 0, 0);
            colorObject.X = (a1 * ColorIntended.X) + (a2 * ColorIntended.Y) + (a3 * ColorIntended.Z);
            colorObject.Y = (b1 * ColorIntended.X) + (b2 * ColorIntended.Y) + (b3 * ColorIntended.Z);
            colorObject.Z = (c1 * ColorIntended.X) + (c2 * ColorIntended.Y) + (c3 * ColorIntended.Z);
            return colorObject;
        }

    }
}
