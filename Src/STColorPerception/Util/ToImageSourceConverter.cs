using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

using System.Threading;
using System.Media;

using System.Runtime.InteropServices;



using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;


namespace STColorPerception.Util
{

  /// <summary>
  ///  class with fuction which coverts the Image Type
  /// </summary>
  public static class ToImageSourceConverter
  {

    [DllImport("gdi32")]
    private static extern int DeleteObject(IntPtr o);

    public static BitmapSource ToBitmapSource(IImage image)
    {
      using (System.Drawing.Bitmap source = image.Bitmap)
      {
        IntPtr ptr = source.GetHbitmap(); //obtain the Hbitmap

        BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
            ptr,
            IntPtr.Zero,
            Int32Rect.Empty,
            System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

        DeleteObject(ptr); //release the HBitmap
        return bs;
      }
    }

        
  }
}
