using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerceptionLib
{
  public class Core
  {
    private static Core instance = null;
    public static Core Instance
    {
      get
      {
        if (instance == null)
          instance = new Core();
        return instance;
      }
    }

    private Core()
    {
    }

    private CaptureEngine CaptureE { get; set; }

  }
}
