using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using PerceptionLib;


namespace DataGrid
{
  class ColorSpaceConverter
  {

   

    public static System.Drawing.Color ToGetRGB(int r, int g, int b)
    {
      System.Drawing.Color mRGB = new System.Drawing.Color();
      mRGB = System.Drawing.Color.FromArgb(r, g, b);
      return mRGB;
    }
      
    public static PerceptionLib.Color ToGetLUV(int r, int g, int b)
    {
      System.Drawing.Color mRGB = ToGetRGB(r, g, b);
      PerceptionLib.Color colorObject = PerceptionLib.Color.ToLUV(mRGB);
      return colorObject;
    }

    public static PerceptionLib.CIEXYZ ToGetXYZ(int r, int g, int b)
    {
        System.Drawing.Color mRGB = ToGetRGB(r, g, b);
        PerceptionLib.CIEXYZ colorObject = PerceptionLib.Color.RGBToXYZ(mRGB);
        return colorObject;
    }

    
    public static PerceptionLib.CIEXYZ AddXYZ(CIEXYZ C1xyz, CIEXYZ C2xyz)
    {
        PerceptionLib.CIEXYZ C3XYZ = new PerceptionLib.CIEXYZ(0, 0, 0);
        C3XYZ.X = Convert.ToDouble(C1xyz.X.ToString()) + Convert.ToDouble(C2xyz.X.ToString());
        C3XYZ.Y = Convert.ToDouble(C1xyz.Y.ToString()) + Convert.ToDouble(C2xyz.Y.ToString());
        C3XYZ.Z = Convert.ToDouble(C1xyz.Z.ToString()) + Convert.ToDouble(C2xyz.Z.ToString());
        return C3XYZ;
    }

    public static PerceptionLib.CIEXYZ SubtractXYZ(CIEXYZ C1xyz, CIEXYZ C2xyz)
    {
      PerceptionLib.CIEXYZ C3XYZ = new PerceptionLib.CIEXYZ(0, 0, 0);
      C3XYZ.X = Math.Abs(Convert.ToDouble(C1xyz.X.ToString()) - Convert.ToDouble(C2xyz.X.ToString()));
      C3XYZ.Y = Math.Abs(Convert.ToDouble(C1xyz.Y.ToString()) - Convert.ToDouble(C2xyz.Y.ToString()));
      C3XYZ.Z = Math.Abs(Convert.ToDouble(C1xyz.Z.ToString()) - Convert.ToDouble(C2xyz.Z.ToString()));
      return C3XYZ;
    }

    
    public static PerceptionLib.Color XYZToLABLUV(CIEXYZ C1xyz,CIEXYZ C2xyz)
    {
        PerceptionLib.CIEXYZ C3XYZ = AddXYZ( C1xyz,  C2xyz);

        PerceptionLib.Color colorObject = PerceptionLib.Color.ToLUV(C3XYZ);
        
        return colorObject;
    }

    public static PerceptionLib.RGBValue XYZToRGB(CIEXYZ C1xyz, CIEXYZ C2xyz)
    {
        PerceptionLib.CIEXYZ C3XYZ = AddXYZ(C1xyz, C2xyz);

        PerceptionLib.RGBValue colorObject = PerceptionLib.Color.ToRBG(C3XYZ);

        return colorObject;
    }

   
   



  }
}
