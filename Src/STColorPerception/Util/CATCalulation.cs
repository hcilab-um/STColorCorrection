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

            double a1 = 1.94670801288840, a2 = 0.387347790817122, a3 = -0.0248409510410495,


                  b1 = 0.0176272423311252, b2 = 2.13563606074240, b3 = -0.0141067590583479,


                  c1 = -2.02741583261552, c2 = 1.84794945404951, c3 = 2.15647964603646;

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



            double a1 = 2.17967337842881, a2 = 0.144706753178677, a3 = 0.0169629426377413,


                   b1 = 0.0158950104832374, b2 = 2.12592426697082, b3 = -0.00321023413306597,

                   c1 = 0, c2 = 0, c3 = 2.26367983367983;

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

            double a1 = 2.36883162197189, a2 = 0, a3 = 0,
                   b1 = 0, b2 = 2.13625002670313, b3 = 0,
                   c1 = 0, c2 = 0, c3 = 2.26367983367983;

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
