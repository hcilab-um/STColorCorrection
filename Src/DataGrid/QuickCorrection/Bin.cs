using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace QuickCorrection
{

  public enum Location { OutOfBounds, Top, Bottom, Left, Right, Forward, Backward }

  public class Bin
  {
    public const int RANGEL = 21;
    public const int RANGEA = 41;
    public const int RANGEB = 45;

    public Point3D binLAB;
    public Point3D binRGB;
    public Point3D measuredLAB;
    public Point3D measuredXYZ;

    public bool isEmpty;
    public bool isMoreAccurateThanOrigin;
    public int cycles;
    public double distanceLAB;
    public Location location;
    public double weight;

    public Bin(int l, int a, int b)
    {
      binLAB = new Point3D(l, a, b);
      isEmpty = true;
      distanceLAB = Double.MaxValue;
      location = Location.OutOfBounds;
      isMoreAccurateThanOrigin = false;
    }

    public override string ToString()
    {
      return String.Format("Coordinates: {0}, IsEmpty: {1}, Location: {2}", binLAB, isEmpty, location);
    }
  }
}
