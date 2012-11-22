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
            //double a1 = 1.21356178444864, a2 = 0.0147697138198462, a3 = 0.000993117471369376,
            //       b1 = -0.00599909318707803, b2 = 1.23251321283751, b3 = 0.000563974739215607,
            //       c1 = -0.0229710501639069, c2 = 0.109301363961475, c3 = 1.22235691025888;

            double a1 = 1.66997942570149, a2 = 0.529273267534138, a3 = -0.0398957525262512,
                  b1 = 0.0445536547010412, b2 = 1.89208916427668, b3 = -0.0226561280769514,
                  c1 = -2.93697083618917, c2 = 2.40588556132798, c3 = 1.95416848325763;

            PerceptionLib.CIEXYZ colorObject = new PerceptionLib.CIEXYZ(0,0,0);
            colorObject.X = (a1 * ColorIntended.X) + (a2 * ColorIntended.Y) + (a3 * ColorIntended.Z);
            colorObject.Y = (b1 * ColorIntended.X) + (b2 * ColorIntended.Y) + (b3 * ColorIntended.Z);
            colorObject.Z = (c1 * ColorIntended.X) + (c2 * ColorIntended.Y) + (c3 * ColorIntended.Z);
            return colorObject;
        }

        public static PerceptionLib.CIEXYZ VonKries(PerceptionLib.CIEXYZ ColorIntended)
        {
            //double a1 = 1.22406776615540, a2 = -0.0114965173376411, a3 = 0.0169498465470608,
            //       b1 = -0.00126281088883867, b2 = 1.22833797178289, b3 = 0.000254513376677992,
            //       c1 = 0, c2 = 0, c3 = 1.30801388705357;

            double a1 = 1.97620864855590, a2 = 0.234281091697971, a3 = 0.0118196554618588,
                   b1 = 0.0257341162507148, b2 = 1.88918851830237, b3 = -0.00519693489961165,
                   c1 = 0, c2 = 0, c3 = 2.03473987143071;

            PerceptionLib.CIEXYZ colorObject = new PerceptionLib.CIEXYZ(0, 0, 0);
            colorObject.X = (a1 * ColorIntended.X) + (a2 * ColorIntended.Y) + (a3 * ColorIntended.Z);
            colorObject.Y = (b1 * ColorIntended.X) + (b2 * ColorIntended.Y) + (b3 * ColorIntended.Z);
            colorObject.Z = (c1 * ColorIntended.X) + (c2 * ColorIntended.Y) + (c3 * ColorIntended.Z);
            return colorObject;
        }

        public static PerceptionLib.CIEXYZ XYZScaling(PerceptionLib.CIEXYZ ColorIntended)
        {
            //double a1 = 1.23020670195830, a2 = 0, a3 = 0,
            //       b1 = 0, b2 = 1.22740048850539, b3 = 0,
            //       c1 = 0, c2 = 0, c3 = 1.30801388705357;

            double a1 = 2.28753309265945, a2 = 0, a3 = 0,
                   b1 = 0, b2 = 1.90425410366759, b3 = 0,
                   c1 = 0, c2 = 0, c3 = 2.03473987143071;

            PerceptionLib.CIEXYZ colorObject = new PerceptionLib.CIEXYZ(0, 0, 0);
            colorObject.X = (a1 * ColorIntended.X) + (a2 * ColorIntended.Y) + (a3 * ColorIntended.Z);
            colorObject.Y = (b1 * ColorIntended.X) + (b2 * ColorIntended.Y) + (b3 * ColorIntended.Z);
            colorObject.Z = (c1 * ColorIntended.X) + (c2 * ColorIntended.Y) + (c3 * ColorIntended.Z);
            return colorObject;
        }

        public static double sdev(List<double> nums)
        {
            List<double> store = new List<double>();
            foreach (double n in nums)
                store.Add((n - nums.Average()) * (n - nums.Average()));

            return Math.Sqrt(store.Sum() / store.Count);
        }


        public static double rootMeanSquare(int[] x)
        {
            double sum = 0;
            for (int i = 0; i < x.Length; i++)
            {
                sum += (x[i] * x[i]);
            }
            return Math.Sqrt(sum / x.Length);
        }

    }
}
