using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Cs200Demo
{
  /// <summary>
  /// class to import and initalize the Kmsecs200.dll
  /// </summary>
  public class ConnectionToCS200
  {
    // Note before you tru to run this program pls inclue the path of the Kmsecs200.dll in project ->properties-> build Events


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

    
    //[DllImport("Kmsecs200.dll", CallingConvention = CallingConvention.Cdecl)]
    //public static extern int write64_usb(int index, System.IntPtr cmd, int timeout, int writeLen);

    
    // public static extern int read64_usb(int index, [MarshalAs(UnmanagedType.LPStr)] string dat, int timeout, int readLen);

    //[DllImport("Kmsecs200.dll", CallingConvention = CallingConvention.Cdecl)]
    //public static extern int read64_usb(int index, System.IntPtr dat, int timeout, int readLen);
  }
}

