using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PerceptionLib
{

  public class Color : INotifyPropertyChanged
  {

    private double l, u, v;

    public double L 
    {
      get { return l; }
      set 
      {
        l = value;
        OnPropertyChanged("L");
      }
    }

    public double U
    {
      get { return u; }
      set
      {
        u = value;
        OnPropertyChanged("U");
      }
    }

    public double V
    {
      get { return v; }
      set
      {
        v = value;
        OnPropertyChanged("V");
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(String name)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(name));
    }
  }

}
