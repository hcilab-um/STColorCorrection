using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Media.Media3D;

namespace STColorPerception.Util
{
    class CATCalulation
    {

      public static double ClosestL, ClosestA, ClosestB;

        public static double closestColorInsideTheBin = double.MaxValue;
        public static double closestColorOnAddition = double.MaxValue;
        public static double HueAngeDifference = double.MaxValue;
        /// to use when c>5 in huediference function
        public static int HueDifferenceFlag = 0;

        public static PerceptionLib.CIEXYZ bradford(PerceptionLib.CIEXYZ ColorIntended)
        {
            //double a1 = 1.21356178444864, a2 = 0.0147697138198462, a3 = 0.000993117471369376,
            //       b1 = -0.00599909318707803, b2 = 1.23251321283751, b3 = 0.000563974739215607,
            //       c1 = -0.0229710501639069, c2 = 0.109301363961475, c3 = 1.22235691025888;

          double a1 = 3.680878397355853, a2 = -0.125561420765456, a3 = -0.031693982497234,


                  b1 = 0.130942826233715, b2 = 3.378993619275563, b3 = -0.017998480571422,


                  c1 = -0.455826584037247, c2 = -1.394610457951430, c3 = 3.596564478605494;

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



          double a1 = 3.563498356215192, a2 = 0.294040219362152, a3 = -0.229656814142911,


                   b1 = 0.032298232574425, b2 = 3.454281611478240, b3 = -0.006515457264759,

                   c1 = 0, c2 = 0, c3 = 2.426091800356506;

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

          double a1 = 3.495660169179846, a2 = 0, a3 = 0,
                   b1 = 0, b2 = 3.474635163307853, b3 = 0,
                   c1 = 0, c2 = 0, c3 = 2.426091800356506;

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

        public static int MatchWithBinnedColorsWithHue(PerceptionLib.Color ColorToCompensate, DataTable bin, PerceptionLib.CIEXYZ BG, PerceptionLib.Color ColorToShow)
        {
          int BinNUmber = 0;
          int i;
          double closestColor = double.MaxValue;
          double closestColorValue = double.MaxValue;
          double angleDifference = double.MaxValue;


          PerceptionLib.Color colorToCompare = new PerceptionLib.Color();
          PerceptionLib.CIEXYZ colorToCompareXYZ = new PerceptionLib.CIEXYZ(0, 0, 0);

          PerceptionLib.CIEXYZ colorToComparePlsBG_XYZ = new PerceptionLib.CIEXYZ(0, 0, 0);
          PerceptionLib.Color colorToComparePlsBG = new PerceptionLib.Color();

          for (int index = 0; index < bin.Rows.Count; index++)
          {
            i = index;

            colorToCompare.LA = Convert.ToDouble(bin.Rows[index][9].ToString());
            colorToCompare.A = Convert.ToDouble(bin.Rows[index][10].ToString());
            colorToCompare.B = Convert.ToDouble(bin.Rows[index][11].ToString());

            double distance = PerceptionLib.Color.ColorDistanceCalAB(colorToCompare, ColorToCompensate);
            if (distance >= closestColor)
              continue;

            int BinNUmber1 = index;
            colorToCompareXYZ.X = Convert.ToDouble(bin.Rows[index][12].ToString());
            colorToCompareXYZ.Y = Convert.ToDouble(bin.Rows[index][13].ToString());
            colorToCompareXYZ.Z = Convert.ToDouble(bin.Rows[index][14].ToString());

            colorToComparePlsBG_XYZ = Util.ColorSpaceConverter.AddXYZ(colorToCompareXYZ, BG);
            colorToComparePlsBG = PerceptionLib.Color.ToLUV(colorToComparePlsBG_XYZ);

            double ColorDistance = PerceptionLib.Color.ColorDistanceCalAB(colorToComparePlsBG, ColorToShow);
            if (ColorDistance >= closestColorValue)
              continue;


            closestColorValue = ColorDistance;
            closestColor = distance;


            closestColorInsideTheBin = distance;
            closestColorOnAddition = ColorDistance;
            HueAngeDifference = angleDifference;
            BinNUmber = index;

          }

          //if (closestColorOnAddition >= 5)
          //{
          //  double LA = Convert.ToDouble(bin.Rows[BinNUmber][9].ToString());
          //  double A = Convert.ToDouble(bin.Rows[BinNUmber][10].ToString());
          //  double B = Convert.ToDouble(bin.Rows[BinNUmber][11].ToString());

          //  for (int index = 0; index < bin.Rows.Count; index++)
          //  {
          //    colorToCompare.LA = Convert.ToDouble(bin.Rows[index][9].ToString());
          //    colorToCompare.A = Convert.ToDouble(bin.Rows[index][10].ToString());
          //    colorToCompare.B = Convert.ToDouble(bin.Rows[index][11].ToString());

          //    if (colorToCompare.LA <= LA + 4 & colorToCompare.LA >= LA - 4)
          //    {
          //      colorToCompareXYZ.X = Convert.ToDouble(bin.Rows[index][12].ToString());
          //      colorToCompareXYZ.Y = Convert.ToDouble(bin.Rows[index][13].ToString());
          //      colorToCompareXYZ.Z = Convert.ToDouble(bin.Rows[index][14].ToString());

          //      colorToComparePlsBG_XYZ = Util.ColorSpaceConverter.AddXYZ(colorToCompareXYZ, BG);
          //      colorToComparePlsBG = PerceptionLib.Color.ToLUV(colorToComparePlsBG_XYZ);

          //      double colorToShowAngle = HueAngle(ColorToShow);
          //      double colorToCompareangle = HueAngle(colorToComparePlsBG);

          //      double angleDiff = Math.Abs(colorToShowAngle - colorToCompareangle);
          //      if (angleDiff >= angleDifference)
          //        continue;

          //      double ColorDistance = PerceptionLib.Color.ColorDistanceCalAB(colorToComparePlsBG, ColorToShow);
          //      double distance = PerceptionLib.Color.ColorDistanceCalAB(colorToCompare, ColorToCompensate);

          //      angleDifference = angleDiff;

          //      closestColorValue = ColorDistance;
          //      closestColor = distance;


          //      closestColorInsideTheBin = distance;
          //      closestColorOnAddition = ColorDistance;
          //      HueAngeDifference = angleDifference;
          //      BinNUmber = index;
          //    }
          //  }
          //}

          if (closestColorOnAddition >= 5)
          {
            double LA = Convert.ToDouble(bin.Rows[BinNUmber][9].ToString());
            double A = Convert.ToDouble(bin.Rows[BinNUmber][10].ToString());
            double B = Convert.ToDouble(bin.Rows[BinNUmber][11].ToString());

            for (int index = 0; index < bin.Rows.Count; index++)
            {
              colorToCompare.LA = Convert.ToDouble(bin.Rows[index][9].ToString());
              colorToCompare.A = Convert.ToDouble(bin.Rows[index][10].ToString());
              colorToCompare.B = Convert.ToDouble(bin.Rows[index][11].ToString());

              if (colorToCompare.LA <= LA + 4 & colorToCompare.LA >= LA - 4)
              {
                colorToCompareXYZ.X = Convert.ToDouble(bin.Rows[index][12].ToString());
                colorToCompareXYZ.Y = Convert.ToDouble(bin.Rows[index][13].ToString());
                colorToCompareXYZ.Z = Convert.ToDouble(bin.Rows[index][14].ToString());

                colorToComparePlsBG_XYZ = Util.ColorSpaceConverter.AddXYZ(colorToCompareXYZ, BG);
                colorToComparePlsBG = PerceptionLib.Color.ToLUV(colorToComparePlsBG_XYZ);

                double hueDiff = hueDifference(colorToCompare, colorToComparePlsBG);
                if (HueDifferenceFlag != 1)
                {
                  if (hueDiff >= angleDifference)
                    continue;


                  double ColorDistance = PerceptionLib.Color.ColorDistanceCalAB(colorToComparePlsBG, ColorToShow);
                  double distance = PerceptionLib.Color.ColorDistanceCalAB(colorToCompare, ColorToCompensate);

                  angleDifference = hueDiff;

                  closestColorValue = ColorDistance;
                  closestColor = distance;


                  closestColorInsideTheBin = distance;
                  closestColorOnAddition = ColorDistance;
                  HueAngeDifference = angleDifference;
                  BinNUmber = index;
                }

              }
            }

          }
          return BinNUmber;
        }

      /// <summary>
      /// funtion to calcullate bin number
      /// </summary>
      /// <param name="ColorToCompensate"></param>
      /// <param name="bin"></param>
      /// <returns></returns>
        public static int MatchWithBinnedColors(PerceptionLib.Color ColorToCompensate, DataTable bin,PerceptionLib.CIEXYZ BG,PerceptionLib.Color ColorToShow)
        {
          int BinNUmber=0;
          int i;
          double closestColor = double.MaxValue;
          double closestColorValue = double.MaxValue;
          double angleDifference = double.MaxValue;
         
          
          PerceptionLib.Color colorToCompare = new PerceptionLib.Color();
          PerceptionLib.CIEXYZ colorToCompareXYZ = new PerceptionLib.CIEXYZ(0,0,0);
          
          PerceptionLib.CIEXYZ colorToComparePlsBG_XYZ= new PerceptionLib.CIEXYZ(0,0,0);
          PerceptionLib.Color colorToComparePlsBG = new PerceptionLib.Color();
         
          for (int index = 0; index < bin.Rows.Count; index++)
          {
            i = index;
            
            colorToCompare.LA = Convert.ToDouble(bin.Rows[index][9].ToString());
            colorToCompare.A = Convert.ToDouble(bin.Rows[index][10].ToString());
            colorToCompare.B = Convert.ToDouble(bin.Rows[index][11].ToString());

            double distance = PerceptionLib.Color.ColorDistanceCalAB(colorToCompare, ColorToCompensate);
            if (distance>= closestColor)
              continue;

            ClosestL = colorToCompare.LA;
            ClosestA = colorToCompare.A;
            ClosestB = colorToCompare.B;

            int BinNUmber1 = index;
            colorToCompareXYZ.X = Convert.ToDouble(bin.Rows[index][12].ToString());
            colorToCompareXYZ.Y = Convert.ToDouble(bin.Rows[index][13].ToString());
            colorToCompareXYZ.Z = Convert.ToDouble(bin.Rows[index][14].ToString());

            colorToComparePlsBG_XYZ = Util.ColorSpaceConverter.AddXYZ(colorToCompareXYZ, BG);
            colorToComparePlsBG = PerceptionLib.Color.ToLUV(colorToComparePlsBG_XYZ);
            
            double ColorDistance = PerceptionLib.Color.ColorDistanceCalAB(colorToComparePlsBG, ColorToShow);
            if (ColorDistance >= closestColorValue)
              continue;
          

            closestColorValue = ColorDistance;
            closestColor = distance;
            
            
            closestColorInsideTheBin = distance;
            closestColorOnAddition = ColorDistance;
            HueAngeDifference = angleDifference;
            BinNUmber = index;
            
          }

          //if (closestColorOnAddition >= 5)
          //{
          //  double LA = Convert.ToDouble(bin.Rows[BinNUmber][9].ToString());
          //  double A = Convert.ToDouble(bin.Rows[BinNUmber][10].ToString());
          //  double B = Convert.ToDouble(bin.Rows[BinNUmber][11].ToString());

          //  for (int index = 0; index < bin.Rows.Count; index++)
          //  {
          //    colorToCompare.LA = Convert.ToDouble(bin.Rows[index][9].ToString());
          //    colorToCompare.A = Convert.ToDouble(bin.Rows[index][10].ToString());
          //    colorToCompare.B = Convert.ToDouble(bin.Rows[index][11].ToString());

          //    if (colorToCompare.LA <= LA + 4 & colorToCompare.LA >= LA - 4)
          //    {
          //      colorToCompareXYZ.X = Convert.ToDouble(bin.Rows[index][12].ToString());
          //      colorToCompareXYZ.Y = Convert.ToDouble(bin.Rows[index][13].ToString());
          //      colorToCompareXYZ.Z = Convert.ToDouble(bin.Rows[index][14].ToString());

          //      colorToComparePlsBG_XYZ = Util.ColorSpaceConverter.AddXYZ(colorToCompareXYZ, BG);
          //      colorToComparePlsBG = PerceptionLib.Color.ToLUV(colorToComparePlsBG_XYZ);

          //      double colorToShowAngle = HueAngle(ColorToShow);
          //      double colorToCompareangle = HueAngle(colorToComparePlsBG);

          //      double angleDiff = Math.Abs(colorToShowAngle - colorToCompareangle);
          //      if (angleDiff >= angleDifference)
          //        continue;

          //      double ColorDistance = PerceptionLib.Color.ColorDistanceCalAB(colorToComparePlsBG, ColorToShow);
          //      double distance = PerceptionLib.Color.ColorDistanceCalAB(colorToCompare, ColorToCompensate);

          //      angleDifference = angleDiff;

          //      closestColorValue = ColorDistance;
          //      closestColor = distance;


          //      closestColorInsideTheBin = distance;
          //      closestColorOnAddition = ColorDistance;
          //      HueAngeDifference = angleDifference;
          //      BinNUmber = index;
          //    }
          //  }
          //}

          //if (closestColorOnAddition >= 5)
          //{
          //  double LA = Convert.ToDouble(bin.Rows[BinNUmber][9].ToString());
          //  double A = Convert.ToDouble(bin.Rows[BinNUmber][10].ToString());
          //  double B = Convert.ToDouble(bin.Rows[BinNUmber][11].ToString());

          //  for (int index = 0; index < bin.Rows.Count; index++)
          //  {
          //    colorToCompare.LA = Convert.ToDouble(bin.Rows[index][9].ToString());
          //    colorToCompare.A = Convert.ToDouble(bin.Rows[index][10].ToString());
          //    colorToCompare.B = Convert.ToDouble(bin.Rows[index][11].ToString());

          //    if (colorToCompare.LA <= LA + 4 & colorToCompare.LA >= LA - 4)
          //    {
          //      colorToCompareXYZ.X = Convert.ToDouble(bin.Rows[index][12].ToString());
          //      colorToCompareXYZ.Y = Convert.ToDouble(bin.Rows[index][13].ToString());
          //      colorToCompareXYZ.Z = Convert.ToDouble(bin.Rows[index][14].ToString());

          //      colorToComparePlsBG_XYZ = Util.ColorSpaceConverter.AddXYZ(colorToCompareXYZ, BG);
          //      colorToComparePlsBG = PerceptionLib.Color.ToLUV(colorToComparePlsBG_XYZ);
                
          //      double hueDiff = hueDifference(colorToCompare, colorToComparePlsBG);
          //      if (HueDifferenceFlag != 1)
          //      {
          //        if (hueDiff >= angleDifference)
          //          continue;


          //        double ColorDistance = PerceptionLib.Color.ColorDistanceCalAB(colorToComparePlsBG, ColorToShow);
          //        double distance = PerceptionLib.Color.ColorDistanceCalAB(colorToCompare, ColorToCompensate);

          //        angleDifference = hueDiff;

          //        closestColorValue = ColorDistance;
          //        closestColor = distance;


          //        closestColorInsideTheBin = distance;
          //        closestColorOnAddition = ColorDistance;
          //        HueAngeDifference = angleDifference;
          //        BinNUmber = index;
          //      }
              
          //    }
          //  }
                       
          //}
          return BinNUmber;
        }


        public static int MatchWithBinned(PerceptionLib.Color ColorToCompensate, DataTable bin, PerceptionLib.CIEXYZ BG, PerceptionLib.Color ColorToShow)
        {
          int BinNUmber = 0;
          int i;
          double closestColor = double.MaxValue;
         // double closestColorValue = double.MaxValue;
          //double angleDifference = double.MaxValue;


          PerceptionLib.Color colorToCompare = new PerceptionLib.Color();
          PerceptionLib.CIEXYZ colorToCompareXYZ = new PerceptionLib.CIEXYZ(0, 0, 0);

          PerceptionLib.CIEXYZ colorToComparePlsBG_XYZ = new PerceptionLib.CIEXYZ(0, 0, 0);
          PerceptionLib.Color colorToComparePlsBG = new PerceptionLib.Color();

          for (int index = 0; index < bin.Rows.Count; index++)
          {
            i = index;

            colorToCompare.LA = Convert.ToDouble(bin.Rows[index][9].ToString());
            colorToCompare.A = Convert.ToDouble(bin.Rows[index][10].ToString());
            colorToCompare.B = Convert.ToDouble(bin.Rows[index][11].ToString());

            double distance = PerceptionLib.Color.ColorDistanceCalAB(colorToCompare, ColorToCompensate);
            if (distance >= closestColor)
              continue;

            //ClosestL = colorToCompare.LA;
            //ClosestA = colorToCompare.A;
            //ClosestB = colorToCompare.B;

            //int BinNUmber1 = index;
            //colorToCompareXYZ.X = Convert.ToDouble(bin.Rows[index][12].ToString());
            //colorToCompareXYZ.Y = Convert.ToDouble(bin.Rows[index][13].ToString());
            //colorToCompareXYZ.Z = Convert.ToDouble(bin.Rows[index][14].ToString());

            //colorToComparePlsBG_XYZ = Util.ColorSpaceConverter.AddXYZ(colorToCompareXYZ, BG);
            //colorToComparePlsBG = PerceptionLib.Color.ToLUV(colorToComparePlsBG_XYZ);

            //double ColorDistance = PerceptionLib.Color.ColorDistanceCalAB(colorToComparePlsBG, ColorToShow);
            //if (ColorDistance >= closestColorValue)
            //  continue;


            //closestColorValue = ColorDistance;
            closestColor = distance;


            closestColorInsideTheBin = distance;
            //closestColorOnAddition = ColorDistance;
            //HueAngeDifference = angleDifference;
            BinNUmber = index;

          }
          return BinNUmber;
        }
    
      
      
      public static double HueAngle(PerceptionLib.Color colorToShow)
      {
       double value1 = colorToShow.B / colorToShow.A;
        //double value1 = -107.9785394 / 78.5459795;
      
      
        double radians = Math.Atan(value1);
        double ColorToShowangle = radians * (180 / Math.PI);

        if (ColorToShowangle < 0)
        {
          ColorToShowangle = ColorToShowangle + 360;
        }
        if (ColorToShowangle >= 360)
        {
          ColorToShowangle = ColorToShowangle - 360;
        }

        return ColorToShowangle;
      }

      /// <summary>
      /// function lakes to lab value to find the huedeffirence between them 
      /// based on :http://www.hunterlab.com/kb/hardware/d25ltvalues.pdf
      /// </summary>
      /// <param name="colorToCompare"></param>
      /// <param name="colorToComparePlsBG"></param>
      /// <returns></returns>
      public static double hueDifference(PerceptionLib.Color colorToCompare, PerceptionLib.Color colorToComparePlsBG)
      {
        double HueDifference = 0;
        // difference in L
        double DifferenceInL = (colorToCompare.LA - colorToComparePlsBG.LA) * (colorToCompare.LA - colorToComparePlsBG.LA);

        //color equlidin distance calculation
        double ColorDifference = PerceptionLib.Color.ColorDistanceCalAB(colorToCompare, colorToComparePlsBG);
        ColorDifference = ColorDifference * ColorDifference;
        
        //diffence in c calculation
        double Ca = (colorToCompare.A * colorToCompare.A) + (colorToCompare.B * colorToCompare.B);
        double colorToCompareC = Math.Sqrt(Ca);
        if (colorToCompareC > 5)
        {
          Ca = (colorToComparePlsBG.A * colorToComparePlsBG.A) + (colorToComparePlsBG.B * colorToComparePlsBG.B);
          double colorToComparePlsBGC = Math.Sqrt(Ca);
          if (colorToComparePlsBGC > 5)
          {
            double DifferenceInC = Math.Abs(colorToCompareC - colorToComparePlsBGC);
            DifferenceInC = DifferenceInC * DifferenceInC;

            HueDifference = Math.Sqrt(ColorDifference - DifferenceInL - DifferenceInC);
            HueDifferenceFlag = 0;
          }
          else
            HueDifferenceFlag = 1;
        }
        else
          HueDifferenceFlag = 1;
        
        return HueDifference;
        
      }


        public static int MatchWithBinnedColorsBasedOn3D(PerceptionLib.Color ColorToCompensate, DataTable bin, PerceptionLib.CIEXYZ BG, PerceptionLib.Color ColorToShow)
        {
          int BinNUmber = 0;
          int i;
          double closestColor = double.MaxValue;
          double closestColorValue = double.MaxValue;

          PerceptionLib.Color colorToCompare = new PerceptionLib.Color();

          double l, a, b;
          
          PerceptionLib.CIEXYZ colorToCompareXYZ = new PerceptionLib.CIEXYZ(0, 0, 0);

          PerceptionLib.CIEXYZ colorToComparePlsBG_XYZ = new PerceptionLib.CIEXYZ(0, 0, 0);
          PerceptionLib.Color colorToComparePlsBG = new PerceptionLib.Color();

          Point3D cX = new Point3D();
          Point3D cXPrime = new Point3D(ColorToCompensate.LA, ColorToCompensate.A, ColorToCompensate.B);

          for (int index = 0; index < bin.Rows.Count; index++)
          {
            i = index;
            l = Convert.ToDouble(bin.Rows[index][9].ToString());
            a = Convert.ToDouble(bin.Rows[index][10].ToString());
            b = Convert.ToDouble(bin.Rows[index][11].ToString());


             Point3D colorInScreen = new Point3D(l, a, b); 
             Vector3D distance = cXPrime - colorInScreen;
            
            
            if (distance.Length >= closestColor)
              continue;
            
            colorToCompareXYZ.X = Convert.ToDouble(bin.Rows[index][12].ToString());
            colorToCompareXYZ.Y = Convert.ToDouble(bin.Rows[index][13].ToString());
            colorToCompareXYZ.Z = Convert.ToDouble(bin.Rows[index][14].ToString());

            colorToComparePlsBG_XYZ = Util.ColorSpaceConverter.AddXYZ(colorToCompareXYZ, BG);
            colorToComparePlsBG = PerceptionLib.Color.ToLUV(colorToComparePlsBG_XYZ);

            double ColorDistance = PerceptionLib.Color.ColorDistanceCalAB(colorToComparePlsBG, ColorToShow);
            if (ColorDistance >= closestColorValue)
              continue;

            closestColorValue = ColorDistance;
            cX = colorInScreen;
            closestColor = distance.Length;
            BinNUmber = index;

          }

          return BinNUmber;
        }

    }
}









































































































































































































