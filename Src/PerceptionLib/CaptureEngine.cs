using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace PerceptionLib
{
  public class CaptureEngine
  {
   

    [DllImport("Kmsecs200.dll")]//, CharSet = CharSet.Unicode)]
    public static extern int get_num();

    [DllImport("Kmsecs200.dll")]//, CharSet = CharSet.Unicode)]
    public static extern int int_usb(int index);

    [DllImport("Kmsecs200.dll")]//, CharSet = CharSet.Unicode)]
    public static extern int end_usb(int index);

    [DllImport("Kmsecs200.dll", CharSet = CharSet.Unicode)]
    public static extern int write64_usb(int index, [MarshalAs(UnmanagedType.LPStr)]string cmd, int timeout, int writeLen);
    
    [DllImport("Kmsecs200.dll", CharSet = CharSet.Unicode)]
    public static extern int read64_usb(int index, [MarshalAs(UnmanagedType.LPStr)] StringBuilder dat, int timeout, int readLen);

   
  }
}
