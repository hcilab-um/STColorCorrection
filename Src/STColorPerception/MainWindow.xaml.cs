using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PerceptionLib.Hacks;
using PerceptionLib;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using System.ComponentModel;

namespace STColorPerception
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window, INotifyPropertyChanged
  {
    // color class object
    private PerceptionLib.Color colorToShow;
    private PerceptionLib.Color colorMeasured;
    private MTObservableCollection<MeasurementPair> pairs;

    //input values
    private byte r, g, b, mr, mg, mb;

    //EMGU CV objects
    private Image<Bgr, Byte> captureImage;
    private Image<Bgr, Byte> croppedImage;

    // this is a color object of BGR value ,.. uit  gives avg blue green and red value from cropped image.
    Bgr avgRGB;

    // obj for web cam capture
    private Capture captureDevice;

    public PerceptionLib.Color ColorToShow
    {
      get { return colorToShow; }
      set
      {
        colorToShow = value;
        OnPropertyChanged("ColorToShow");
      }
    }

    public PerceptionLib.Color ColorMeasured
    {
      get { return colorMeasured; }
      set
      {
        colorMeasured = value;
        OnPropertyChanged("ColorMeasured");
      }
    }

    //Displayed RGB
    public byte R
    {
      get { return r; }
      set
      {
        r = value;
        OnPropertyChanged("R");
      }
    }

    public byte G
    {
      get { return g; }
      set
      {
        g = value;
        OnPropertyChanged("G");
      }
    }

    public byte B
    {
      get { return b; }
      set
      {
        b = value;
        OnPropertyChanged("B");
      }
    }


    //measured RGB
    public byte MR
    {
      get { return mr; }
      set
      {
        mr = value;
        OnPropertyChanged("MR");
      }
    }

    public byte MG
    {
      get { return mg; }
      set
      {
        mg = value;
        OnPropertyChanged("MG");
      }
    }

    public byte MB
    {
      get { return mb; }
      set
      {
        mb = value;
        OnPropertyChanged("MB");
      }
    }

    public MainWindow()
    {
      InitializeComponent();
      captureDevice = new Capture();

      ColorToShow = new PerceptionLib.Color();
      pairs = new MTObservableCollection<MeasurementPair>();
      cie1976C.DataContext = pairs;

      PropertyChanged += new PropertyChangedEventHandler(MainWindow_PropertyChanged);
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      //Btn_Load_File.IsEnabled = false;
      btn_StartMeasurment.IsEnabled = false;

      //MTObservableCollection<MeasurementPair> pairs = new MTObservableCollection<MeasurementPair>();
      //pairs.Add(new MeasurementPair()
      //{
      //  ColorToShow = new PerceptionLib.Color() { L = 0, UP = 0, VP = 0 },
      //  ColorCaptured = new PerceptionLib.Color() { L = 0, UP = 0.1, VP = 0.2 }
      //});


      //  pairs.Add(new MeasurementPair()
      //  {
      //    ColorToShow = new PerceptionLib.Color() { L = 0, UP = 0.6, VP = 0 },
      //    ColorCaptured = new PerceptionLib.Color() { L = 0, UP = 0.5, VP = 0.1 }
      //  });
      //  pairs.Add(new MeasurementPair()
      //  {
      //    ColorToShow = new PerceptionLib.Color() { L = 0, UP = 0, VP = 0.6 },
      //    ColorCaptured = new PerceptionLib.Color() { L = 0, UP = 0.1, VP = 0.5 }
      //  });
      //  pairs.Add(new MeasurementPair()
      //  {
      //    ColorToShow = new PerceptionLib.Color() { L = 0, UP = 0.6, VP = 0.6 },
      //    ColorCaptured = new PerceptionLib.Color() { L = 0, UP = 0.5, VP = 0.5 }
      //  });
      //pairs.Add(new MeasurementPair());
      //pairs.Add(new MeasurementPair() { ColorToShow = new PerceptionLib.Color() { L = 0, UP = 0.3, VP = 0.3 } });
      //cie1976C.DataContext = pairs;
    }

    void MainWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if ("R".Equals(e.PropertyName) || "G".Equals(e.PropertyName) || "B".Equals(e.PropertyName))
      {
        btn_StartMeasurment.IsEnabled = true;

        // to measure LUV from Color class
        ColorToShow = Util.ColorSpaceConverter.ToGetLUV(R, G, B);

        rec_shown.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
        rec_displayColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
      }
    }

    //void Measured_PropertyChanged(object sender, PropertyChangedEventArgs e)
    //{
    //  if ("MR".Equals(e.PropertyName) || "MG".Equals(e.PropertyName) || "MB".Equals(e.PropertyName))
    //  {


    //  }
    //}


    private void Btn_StartMeasurment_Click(object sender, RoutedEventArgs e)
    {

      // temp int cariables to do the calculations for AVG rgb form the cropped pics
      int tempMr = 0, tempMg = 0, tempMb = 0;
      for (int i = 0; i < 6; i++)
      {

        GetImage();
        CropImage();
        if (i == 0)
        {
          tempMr = 0;
          tempMg = 0;
          tempMb = 0;

        }

        tempMr = (int)(tempMr + avgRGB.Red);
        tempMg = (int)(tempMg + avgRGB.Green);
        tempMb = (int)(tempMb + avgRGB.Blue);
      }

      tempMr = tempMr / 5;
      tempMg = tempMg / 5;
      tempMb = tempMb / 5;

      MR = (byte)(tempMr);
      MG = (byte)(tempMg);
      MB = (byte)(tempMb);

      DisplayMeasuredValues();

    }


    /// <summary>
    /// funtions to capture Image via web cam , Crop it to its center , and get avg RGB value of its center
    /// </summary>

    private void GetImage()
    {

      System.Threading.Thread.Sleep(500);
      
      captureDevice = new Capture();

      captureImage = captureDevice.QueryFrame();
      croppedImage = captureImage.Copy();
      if (captureImage != null)
      {

        Image_Camera.Source = Util.ToImageSourceConverter.ToBitmapSource(captureImage);
        captureDevice.QueryFrame().Dispose();

      }
    }

    private void CropImage()
    {
      int Center_x, Center_y;
      Center_x = croppedImage.Width / 2;
      Center_y = croppedImage.Height / 2;
      avgRGB = new Bgr();

      croppedImage.ROI = new System.Drawing.Rectangle(Center_x, Center_y, 100, 100);
      Image_Croped.Source = Util.ToImageSourceConverter.ToBitmapSource(croppedImage);
      avgRGB = croppedImage.GetAverage();

      captureImage.Dispose();
      croppedImage.Dispose();
     //// captureDevice.Dispose();

      //((IDisposable)captureDevice).Dispose();
      //((IDisposable)croppedImage).Dispose();
      ////((IDisposable)captureImage).Dispose();
     }

    //private void MeasureRGB()
    //{      
    //  System.Drawing.Color RGB = new System.Drawing.Color();

    //  // fget avg give the avg color value of the image in its present ROI


    //  //Avg_B = Convert.ToInt32(a.Blue);
    //  //Avg_G = Convert.ToInt32(a.Green);
    //  //Avg_R = Convert.ToInt32(a.Red);



    //  //RGB = Util.ColorSpaceConverter.ToGetRGB(Avg_R, Avg_G, Avg_B);



    //  ////to display the color in the rectangle 
    //  //Rectangle_Captured.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(RGB.R, RGB.G, RGB.B));

    //lbl_MR.Content = RGB.R.ToString();
    //lbl_MG.Content = RGB.G.ToString();
    //lbl_MB.Content = RGB.B.ToString();

    //  //lbl_ML.Content = colorMeasured.L.ToString();
    //  //lbl_MU.Content = colorMeasured.U.ToString();
    //  //lbl_MV.Content = colorMeasured.V.ToString();
    //  //lbl_MUP.Content = colorMeasured.UP.ToString();
    //  //lbl_MVP.Content = colorMeasured.VP.ToString();


    //}

    private void DisplayMeasuredValues()
    {
      ColorMeasured = Util.ColorSpaceConverter.ToGetLUV(MR, MG, MB);
      //to display the color in the rectangle 

      Rectangle_Captured.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(MR, MG, MB));

      // to display the shift
      pairs.Clear();
      pairs.Add(new MeasurementPair()
      {
        ColorToShow = new PerceptionLib.Color() { L = 0, UP = colorToShow.UP, VP = colorToShow.VP },
        ColorCaptured = new PerceptionLib.Color() { L = 0, UP = colorMeasured.UP, VP = colorMeasured.VP }
      });


    }


    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(String name)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(name));
    }

  }
}
