using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using PerceptionLib;


namespace STColorPerception.Util
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

   



  }
}
