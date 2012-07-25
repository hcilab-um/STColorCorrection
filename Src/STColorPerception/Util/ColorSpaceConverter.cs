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
    
  
    
    public static System.Drawing.Color ToGetRGB(int R, int G, int B)
    {
      System.Drawing.Color mRGB = new System.Drawing.Color();
      mRGB = System.Drawing.Color.FromArgb(R, G, B);
      return mRGB;
    }


    public static PerceptionLib.Color ToGetLUV(int R, int G, int B)
    {
       PerceptionLib.Color colorObject = new PerceptionLib.Color();
     
      System.Drawing.Color mRGB = new System.Drawing.Color();
      mRGB = new System.Drawing.Color();
      mRGB = System.Drawing.Color.FromArgb(R, G, B);
      
      colorObject = colorObject.ToLUV(mRGB);
      return colorObject;
    
    }




  }
}
