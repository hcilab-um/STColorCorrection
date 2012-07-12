using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerceptionLib
{
  class CaptureEngine
  {
    public event EventHandler CaptureFinished;
    public event EventHandler<ColorCaptureEventArgs> PreCapture;
    public event EventHandler<ColorCaptureEventArgs> CaptureReady;

    public void Load() 
    { }

    public void Start()
    { }

    public void Stop()
    { }
  }
}
