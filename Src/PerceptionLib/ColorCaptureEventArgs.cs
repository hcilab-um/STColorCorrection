using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerceptionLib
{
  class ColorCaptureEventArgs : EventArgs
  {
    public MeasurementPair Pair { get; set; }
  }
}
