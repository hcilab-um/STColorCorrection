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
using System.Windows.Data;
using System.Data;
using System.Windows.Threading;
using PerceptionLib;

namespace DataGrid
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void PopulateGrid(string fileName)
    {
      //DataTable table = CSV.GetDataTableFromCSV(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\color.txt");
      DataTable table = PerceptionLib.CSV.GetDataTableFromCSV(@fileName);
      if (table.Columns.Count == 0)
        System.Windows.MessageBox.Show("Error!");
      else
        dtgrid_corrDisplay.ItemsSource = table.DefaultView;

      dtgrid_corrDisplay.AutoGenerateColumns = true;

    }

    private void button1_Click(object sender, RoutedEventArgs e)
    {
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\Reduscer.csv");
      DataTable bin = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));
      PerceptionLib.CIEXYZ ColorToShowXYZ = new CIEXYZ(0, 0, 0);
      PerceptionLib.CIEXYZ BradXYZ = new CIEXYZ(0, 0, 0);
      PerceptionLib.CIEXYZ VonXYZ = new CIEXYZ(0, 0, 0);
      PerceptionLib.CIEXYZ ScalingXYZ = new CIEXYZ(0, 0, 0);
      PerceptionLib.Color ColorToShow;

      foreach (DataRow dr in bin.Rows)
      {
        
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //CAT LAB caluclation

        //brad
        //ColorToShowXYZ.X = Convert.ToDouble(dr["bradX"].ToString());
        //ColorToShowXYZ.Y = Convert.ToDouble(dr["bradY"].ToString());
        //ColorToShowXYZ.Z = Convert.ToDouble(dr["bradZ"].ToString());

        //ColorToShow = PerceptionLib.Color.ToLAB(ColorToShowXYZ);

        //dr["bradL"] = ColorToShow.LA.ToString();
        //dr["bradA"] = ColorToShow.A.ToString();
        //dr["bradB"] = ColorToShow.B.ToString();

        ////Von
        //ColorToShowXYZ.X = Convert.ToDouble(dr["VonX"].ToString());
        //ColorToShowXYZ.Y = Convert.ToDouble(dr["VonY"].ToString());
        //ColorToShowXYZ.Z = Convert.ToDouble(dr["VonZ"].ToString());

        //ColorToShow = PerceptionLib.Color.ToLAB(ColorToShowXYZ);

        ////scaling
        //dr["vonL"] = ColorToShow.LA.ToString();
        //dr["vonA"] = ColorToShow.A.ToString();
        //dr["vonB"] = ColorToShow.B.ToString();

        //ColorToShowXYZ.X = Convert.ToDouble(dr["SX"].ToString());
        //ColorToShowXYZ.Y = Convert.ToDouble(dr["SY"].ToString());
        //ColorToShowXYZ.Z = Convert.ToDouble(dr["SZ"].ToString());

        //ColorToShow = PerceptionLib.Color.ToLAB(ColorToShowXYZ);

        //dr["sL"] = ColorToShow.LA.ToString();
        //dr["sA"] = ColorToShow.A.ToString();
        //dr["sB"] = ColorToShow.B.ToString();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //xyz caluclation

        //ColorToShowXYZ.X = Convert.ToDouble(dr["X"].ToString());
        //ColorToShowXYZ.Y = Convert.ToDouble(dr["Y"].ToString());
        //ColorToShowXYZ.Z = Convert.ToDouble(dr["Z"].ToString());

        //BradXYZ = CATCalulation.bradford(ColorToShowXYZ);
        //VonXYZ = CATCalulation.VonKries(ColorToShowXYZ);
        //ScalingXYZ = CATCalulation.XYZScaling(ColorToShowXYZ);

        //dr["bradX"] = BradXYZ.X.ToString();
        //dr["bradY"] = BradXYZ.Y.ToString();
        //dr["bradZ"] = BradXYZ.Z.ToString();

        //dr["VonX"] = VonXYZ.X.ToString();
        //dr["VonY"] = VonXYZ.Y.ToString();
        //dr["VonZ"] = VonXYZ.Z.ToString();

        //dr["ScalX"] = ScalingXYZ.X.ToString();
        //dr["ScalY"] = ScalingXYZ.Y.ToString();
        //dr["ScalZ"] = ScalingXYZ.Z.ToString();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ColorToShow.LA = Convert.ToByte(dr["L"].ToString());
        ColorToShow.A = Convert.ToByte(dr["A"].ToString());
        ColorToShow.B = Convert.ToByte(dr["B"].ToString());
        PerceptionLib.RGBValue ab = PerceptionLib.Color.ToRBGFromLAB(ColorToShow);
               
        //Hex color
        String Hex = PerceptionLib.Color.RGBtoHEX(R, G, B);
        dr["HEX"] = "0x" + Hex;
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //ColorToShowXYZ = new CIEXYZ(0, 0, 0);
        //ColorToShowXYZ.X = Convert.ToDouble(dr["X"].ToString());
        //ColorToShowXYZ.Y = Convert.ToDouble(dr["Y"].ToString());
        //ColorToShowXYZ.Z = Convert.ToDouble(dr["Z"].ToString());

        //ColorToShow = PerceptionLib.Color.ToLUV(ColorToShowXYZ);
        //PerceptionLib.RGBValue ab = PerceptionLib.Color.ToRBGFromLAB(ColorToShow);

        //dr["L"] = ColorToShow.LA.ToString();
        //dr["A"] = ColorToShow.A.ToString();
        //dr["LB"] = ColorToShow.B.ToString();
        //dr["R"] = ab.R.ToString();
        //dr["G"] = ab.G.ToString();
        //dr["B"] = ab.B.ToString();

        //String Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
        //dr["HEX"] = "0x" + Hex;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = bin.DefaultView));
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

      }
      btn_ExportGrid.IsEnabled = true;
    }

    private void btn_ExportGrid_Click(object sender, RoutedEventArgs e)
    {
      DataTable dt = new DataTable();


      dt = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      PerceptionLib.CSV.ToCSVFromDataTable(dt);
      DataTable table = CSV.GetDataTableFromCSV(@"C:\see-through-project\gt\STColorCorrection\Src\PerceptionLib\bin\GridData.csv");
      if (table.Columns.Count == 0)
        System.Windows.MessageBox.Show("Error!");
      else
        System.Windows.MessageBox.Show("Success!");
    }
  }
}
