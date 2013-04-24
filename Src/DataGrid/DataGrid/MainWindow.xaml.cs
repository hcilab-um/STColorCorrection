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
using System.Drawing;

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
      PerceptionLib.CIEXYZ ColorToShowXYZ = new CIEXYZ(0, 0, 0);
      PerceptionLib.CIEXYZ BradXYZ = new CIEXYZ(0, 0, 0);

      ColorToShowXYZ.X = 0.003889613;
      ColorToShowXYZ.Y = 0.001555845;
      ColorToShowXYZ.Z = 0.020485292;

      BradXYZ = CATCalulation.bradford(ColorToShowXYZ);

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

    private void MathCal_Click(object sender, RoutedEventArgs e)
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
      PerceptionLib.Color ColorToShow = new PerceptionLib.Color();

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

        ColorToShowXYZ.X = Convert.ToDouble(dr["X"].ToString());
        ColorToShowXYZ.Y = Convert.ToDouble(dr["Y"].ToString());
        ColorToShowXYZ.Z = Convert.ToDouble(dr["Z"].ToString());

        BradXYZ = CATCalulation.bradford(ColorToShowXYZ);
        VonXYZ = CATCalulation.VonKries(ColorToShowXYZ);
        ScalingXYZ = CATCalulation.XYZScaling(ColorToShowXYZ);

        dr["bradX"] = BradXYZ.X.ToString();
        dr["bradY"] = BradXYZ.Y.ToString();
        dr["bradZ"] = BradXYZ.Z.ToString();

        dr["VonX"] = VonXYZ.X.ToString();
        dr["VonY"] = VonXYZ.Y.ToString();
        dr["VonZ"] = VonXYZ.Z.ToString();

        dr["ScalX"] = ScalingXYZ.X.ToString();
        dr["ScalY"] = ScalingXYZ.Y.ToString();
        dr["ScalZ"] = ScalingXYZ.Z.ToString();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //ColorToShow.LA = Convert.ToDouble(dr["L"].ToString());
        //ColorToShow.A = Convert.ToDouble(dr["A"].ToString());
        //ColorToShow.B = Convert.ToDouble(dr["B"].ToString());
        //PerceptionLib.RGBValue ab = PerceptionLib.Color.ToRBGFromLAB(ColorToShow);

        ////Hex color
        //String Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
        //dr["HEX"] = "0x" + Hex;
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
        // hue calculation
        //ColorToShow.LA = Convert.ToDouble(dr["L"].ToString());
        //ColorToShow.A = Convert.ToDouble(dr["A"].ToString());
        //ColorToShow.B = Convert.ToDouble(dr["B"].ToString());
        //double hue= CATCalulation.HueAngle(ColorToShow);
        //dr["Hue"] = hue.ToString();



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

    private void Phonedata_Click(object sender, RoutedEventArgs e)
    {
      dtgrid_corrDisplay.IsEnabled = true;


      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\PerceptionLib\bin\previous data\cs-200 data\color mixing\phone\mixtureGroundtruth\bg6_HEX88.csv");
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\phone\bg\nobg_88Phone.csv");
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\phone\bincal\0pts\800_2.csv");
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\PerceptionLib\bin\previous data\cs-200 data\color mixing\1phone\14bg_600.csv");
      DataTable bin = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));

      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\Phone_Bin_wt0.csv");
      DataTable binTable = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        binTable = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));

      DataRow newRow;

      for (int i = 0; i < bin.Rows.Count; i++)
      {
        byte Rt = 0;

        byte Gt = 0;

        byte Bt = 0;


        if (i > 0)
        {
          Rt = Convert.ToByte(bin.Rows[i - 1][0].ToString());
          Gt = Convert.ToByte(bin.Rows[i - 1][1].ToString());
          Bt = Convert.ToByte(bin.Rows[i - 1][2].ToString());

        }
        byte R = Convert.ToByte(bin.Rows[i][0].ToString());
        byte G = Convert.ToByte(bin.Rows[i][1].ToString());
        byte B = Convert.ToByte(bin.Rows[i][2].ToString());

        newRow = binTable.NewRow();


        //if (R >= 224 & G >= 197 & B >= 153)
        // if (R < 10 & G <= 60 & B <= 89)
        // if (R <=70 & G <= 95 & B <= 123)
        if (R <= 45 & G <= 110 & B <= 97)
        {
          newRow[0] = 0;
          newRow[1] = 0;
          newRow[2] = 0;
          newRow[3] = 0;
          newRow[4] = 0;
          newRow[5] = 0;
          newRow[6] = 0;
          newRow[7] = 0;
          newRow[8] = 0;

        }
        else
        {
          //if (Rt >= 224 & Gt >= 197 & Bt >= 153)
          //if (Rt < 10 & Gt <= 60 & Bt <= 89)
          if (Rt <=45 & Gt <= 110 & Bt <= 97)
          {
            newRow[0] = 0;
            newRow[1] = 0;
            newRow[2] = 0;
            newRow[3] = 0;
            newRow[4] = 0;
            newRow[5] = 0;
            newRow[6] = 0;
            newRow[7] = 0;
            newRow[8] = 0;

          }

          else
          {
            newRow[0] = bin.Rows[i][0].ToString();
            newRow[1] = bin.Rows[i][1].ToString();
            newRow[2] = bin.Rows[i][2].ToString();
            newRow[3] = bin.Rows[i][3].ToString();
            newRow[4] = bin.Rows[i][4].ToString();
            newRow[5] = bin.Rows[i][5].ToString();
            newRow[6] = bin.Rows[i][6].ToString();
            newRow[7] = bin.Rows[i][7].ToString();
            newRow[8] = bin.Rows[i][8].ToString();

          }
        }
        binTable.Rows.Add(newRow);
      }

      Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = binTable.DefaultView));
      Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

      ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      // //zero elimination
      bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\Phone_Bin_wt0.csv");
      binTable = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        binTable = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));

      for (int i = 1; i < bin.Rows.Count; i++)
      {

        byte R = Convert.ToByte(bin.Rows[i - 1][0].ToString());
        byte G = Convert.ToByte(bin.Rows[i - 1][1].ToString());
        byte B = Convert.ToByte(bin.Rows[i - 1][2].ToString());

        byte r = Convert.ToByte(bin.Rows[i][0].ToString());
        byte g = Convert.ToByte(bin.Rows[i][1].ToString());
        byte b = Convert.ToByte(bin.Rows[i][2].ToString());

        if (R == 0 & G == 0 & B == 0)
        {
          if (r > 0 || g > 0 || b > 0)
          {
            byte r1 = Convert.ToByte(bin.Rows[i][0].ToString());
            byte g1 = Convert.ToByte(bin.Rows[i][1].ToString());
            byte b1 = Convert.ToByte(bin.Rows[i][2].ToString());
            double l1 = Convert.ToDouble(bin.Rows[i][3].ToString());
            double a1 = Convert.ToDouble(bin.Rows[i][4].ToString());
            double lb1 = Convert.ToDouble(bin.Rows[i][5].ToString());
            double x1 = Convert.ToDouble(bin.Rows[i][6].ToString());
            double y1 = Convert.ToDouble(bin.Rows[i][7].ToString());
            double z1 = Convert.ToDouble(bin.Rows[i][8].ToString());

            byte r2 = Convert.ToByte(bin.Rows[i + 1][0].ToString());
            byte g2 = Convert.ToByte(bin.Rows[i + 1][1].ToString());
            byte b2 = Convert.ToByte(bin.Rows[i + 1][2].ToString());
            double l2 = Convert.ToDouble(bin.Rows[i + 1][3].ToString());
            double a2 = Convert.ToDouble(bin.Rows[i + 1][4].ToString());
            double lb2 = Convert.ToDouble(bin.Rows[i + 1][5].ToString());
            double x2 = Convert.ToDouble(bin.Rows[i + 1][6].ToString());
            double y2 = Convert.ToDouble(bin.Rows[i + 1][7].ToString());
            double z2 = Convert.ToDouble(bin.Rows[i + 1][8].ToString());

            byte r3 = Convert.ToByte(bin.Rows[i + 2][0].ToString());
            byte g3 = Convert.ToByte(bin.Rows[i + 2][1].ToString());
            byte b3 = Convert.ToByte(bin.Rows[i + 2][2].ToString());
            double l3 = Convert.ToDouble(bin.Rows[i + 2][3].ToString());
            double a3 = Convert.ToDouble(bin.Rows[i + 2][4].ToString());
            double lb3 = Convert.ToDouble(bin.Rows[i + 2][5].ToString());
            double x3 = Convert.ToDouble(bin.Rows[i + 2][6].ToString());
            double y3 = Convert.ToDouble(bin.Rows[i + 2][7].ToString());
            double z3 = Convert.ToDouble(bin.Rows[i + 2][8].ToString());

            byte rFinal = Convert.ToByte((r1 + r2 + r3) / 3);
            byte gFinal = Convert.ToByte((g1 + g2 + g3) / 3);
            byte bFinal = Convert.ToByte((b1 + b2 + b3) / 3);
            double lFinal = (l1 + l2 + l3) / 3.0;
            double aFinal = (a1 + a2 + a3) / 3.0;
            double lbFinal = (lb1 + lb2 + lb3) / 3.0;
            double xFinal = (x1 + x2 + x3) / 3.0;
            double yFinal = (y1 + y2 + y3) / 3.0;
            double zFinal = (z1 + z2 + z3) / 3.0;

            newRow = binTable.NewRow();
            newRow[0] = r1.ToString();
            newRow[1] = g1.ToString();
            newRow[2] = b1.ToString();
            newRow[3] = l1.ToString();
            newRow[4] = a1.ToString();
            newRow[5] = lb1.ToString();
            newRow[6] = x1.ToString();
            newRow[7] = y1.ToString();
            newRow[8] = z1.ToString();



            //newRow = binTable.NewRow();
            //newRow[0] = bin.Rows[i][0].ToString();
            //newRow[1] = bin.Rows[i][1].ToString();
            //newRow[2] = bin.Rows[i][2].ToString();
            //newRow[3] = bin.Rows[i][3].ToString();
            //newRow[4] = bin.Rows[i][4].ToString();
            //newRow[5] = bin.Rows[i][5].ToString();
            //newRow[6] = bin.Rows[i][6].ToString();
            //newRow[7] = bin.Rows[i][7].ToString();
            //newRow[8] = bin.Rows[i][8].ToString();


            binTable.Rows.Add(newRow);
          }

        }
      }
      Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = binTable.DefaultView));
      Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));

      btn_ExportGrid.IsEnabled = true;
    }
    //cat
    private void CatCal_Click(object sender, RoutedEventArgs e)
    {
       PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\small pro cat mixture\SmallPro800.csv");
      DataTable bin = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));
      PerceptionLib.CIEXYZ ColorToShowXYZ = new CIEXYZ(0, 0, 0);
      PerceptionLib.CIEXYZ ColorMeasuredXYZ = new CIEXYZ(0, 0, 0);
      
      PerceptionLib.CIEXYZ DBg = new CIEXYZ(0, 0, 0);
      PerceptionLib.CIEXYZ TDBg= new CIEXYZ(0, 0, 0);
      
      PerceptionLib.CIEXYZ AXYZ = new CIEXYZ(0, 0, 0);
      
      PerceptionLib.CIEXYZ BinXYZ = new CIEXYZ(0, 0, 0);
      PerceptionLib.CIEXYZ TDBinXYZ = new CIEXYZ(0, 0, 0);
      PerceptionLib.CIEXYZ DBinXYZ = new CIEXYZ(0, 0, 0);

            
      PerceptionLib.CIEXYZ BradXYZ = new CIEXYZ(0, 0, 0);
      PerceptionLib.CIEXYZ VonXYZ = new CIEXYZ(0, 0, 0);
      PerceptionLib.CIEXYZ ScalXYZ = new CIEXYZ(0, 0, 0);

      PerceptionLib.CIEXYZ TDBradXYZ = new CIEXYZ(0, 0, 0);
      PerceptionLib.CIEXYZ TDVonXYZ = new CIEXYZ(0, 0, 0);
      PerceptionLib.CIEXYZ TDScalXYZ = new CIEXYZ(0, 0, 0);


      PerceptionLib.CIEXYZ DBradXYZ = new CIEXYZ(0, 0, 0);
      PerceptionLib.CIEXYZ DVonXYZ = new CIEXYZ(0, 0, 0);
      PerceptionLib.CIEXYZ DScalXYZ = new CIEXYZ(0, 0, 0);


      PerceptionLib.Color Acolor = new PerceptionLib.Color();
      PerceptionLib.Color TDAcolor = new PerceptionLib.Color();
      PerceptionLib.Color DAcolor = new PerceptionLib.Color();
      
      PerceptionLib.Color Bin = new PerceptionLib.Color();
      PerceptionLib.Color TDBin = new PerceptionLib.Color();
      PerceptionLib.Color DBin = new PerceptionLib.Color();

      PerceptionLib.Color Brad       =    new PerceptionLib.Color();
      PerceptionLib.Color Von      =     new PerceptionLib.Color();
      PerceptionLib.Color Scal    = new PerceptionLib.Color();
      PerceptionLib.Color TDBrad    =  new PerceptionLib.Color();
      PerceptionLib.Color TDVon        =    new PerceptionLib.Color();
      PerceptionLib.Color TDScal   = new PerceptionLib.Color();
      PerceptionLib.Color DBrad        = new PerceptionLib.Color();
      PerceptionLib.Color DVon       = new PerceptionLib.Color();
      PerceptionLib.Color DScal    = new PerceptionLib.Color();
      PerceptionLib.RGBValue ab ;


      CIEXYZ screenWt = new CIEXYZ(0.265572, 0.282182, 0.481033);

      CIEXYZ screenBgWt = new CIEXYZ(0.9504, 0.990041, 1.0888);

       //     ;
       //string Von      ;
       //string Scaling  ;
       //string TDBrad   ;
       //string TDVon    ;
       //string TDScaling;
       //string DBrad    ;
       //string DVon     ;
       //string DScaling ;

      PerceptionLib.Color ColorToShow = new PerceptionLib.Color();
      PerceptionLib.Color ColorMeasured = new PerceptionLib.Color();
      int i = 0;
      foreach (DataRow dr in bin.Rows)
      {
        //i++;
        //DBg.X = Convert.ToDouble(dr["BX"].ToString());
        //DBg.Y = Convert.ToDouble(dr["BY"].ToString());
        //DBg.Z = Convert.ToDouble(dr["BZ"].ToString());

        //TDBg.X = Convert.ToDouble(dr["BMX"].ToString());
        //TDBg.Y = Convert.ToDouble(dr["BMY"].ToString());
        //TDBg.Z = Convert.ToDouble(dr["BMZ"].ToString());

        ////bin fg
        //BinXYZ.X = Convert.ToDouble(dr["BinX"].ToString());
        //BinXYZ.Y = Convert.ToDouble(dr["BinY"].ToString());
        //BinXYZ.Z = Convert.ToDouble(dr["BinZ"].ToString());

        //Bin = PerceptionLib.Color.ToLAB(BinXYZ, screenWt);
        //dr["BP_Fg_L"] = Bin.LA.ToString();
        //dr["BP_Fg_a"] = Bin.A.ToString();
        //dr["BP_Fg_b"] = Bin.B.ToString();

        //ab = PerceptionLib.Color.ToRBGFromLAB(Bin);
        //String Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
        //dr["BP_Fg_RRGGBB"] = "0x" + Hex;

        ////pure
        //Acolor.LA = Convert.ToDouble(dr["Fg_L"].ToString());
        //Acolor.A = Convert.ToDouble(dr["Fg_A"].ToString());
        //Acolor.B = Convert.ToDouble(dr["Fg_B"].ToString());

        //TDAcolor.LA = Convert.ToDouble(dr["P_Bc_TD_L"].ToString());
        //TDAcolor.A = Convert.ToDouble(dr["P_Bc_TD_A"].ToString());
        //TDAcolor.B = Convert.ToDouble(dr["P_Bc_TD_B"].ToString());

        //DAcolor.LA = Convert.ToDouble(dr["P_Bc_ND_L"].ToString());
        //DAcolor.A = Convert.ToDouble(dr["P_Bc_ND_A"].ToString());
        //DAcolor.B = Convert.ToDouble(dr["P_Bc_ND_B"].ToString());

        ////cat
        //BradXYZ.X = Convert.ToDouble(dr["BradX"].ToString());
        //BradXYZ.Y = Convert.ToDouble(dr["BradY"].ToString());
        //BradXYZ.Z = Convert.ToDouble(dr["BradZ"].ToString());

        //Brad = PerceptionLib.Color.ToLAB(BradXYZ, screenWt);
        //dr["CAT1_Fg_L"] = Brad.LA.ToString();
        //dr["CAT1_Fg_a"] = Brad.A.ToString();
        //dr["CAT1_Fg_b"] = Brad.B.ToString();

        //ab = PerceptionLib.Color.ToRBGFromLAB(Brad);
        //Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
        //dr["CAT1_Fg_RRGGBB"] = "0x" + Hex;

        //VonXYZ.X = Convert.ToDouble(dr["VonX"].ToString());
        //VonXYZ.Y = Convert.ToDouble(dr["VonY"].ToString());
        //VonXYZ.Z = Convert.ToDouble(dr["VonZ"].ToString());

        //Von = PerceptionLib.Color.ToLAB(VonXYZ, screenWt);
        //dr["CAT2_Fg_L"] = Von.LA.ToString();
        //dr["CAT2_Fg_a"] = Von.A.ToString();
        //dr["CAT2_Fg_b"] = Von.B.ToString();
        //ab = PerceptionLib.Color.ToRBGFromLAB(Von);
        //Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
        //dr["CAT2_Fg_RRGGBB"] = "0x" + Hex;

        //ScalXYZ.X = Convert.ToDouble(dr["ScalX"].ToString());
        //ScalXYZ.Y = Convert.ToDouble(dr["ScalY"].ToString());
        //ScalXYZ.Z = Convert.ToDouble(dr["ScalZ"].ToString());

        //Scal = PerceptionLib.Color.ToLAB(ScalXYZ, screenWt);
        //dr["CAT3_Fg_L"] = Scal.LA.ToString();
        //dr["CAT3_Fg_a"] = Scal.A.ToString();
        //dr["CAT3_Fg_b"] = Scal.B.ToString();
        //ab = PerceptionLib.Color.ToRBGFromLAB(Scal);
        //Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
        //dr["CAT3_Fg_RRGGBB"] = "0x" + Hex;

        // color measured
        ColorMeasuredXYZ.X = Convert.ToDouble(dr["MX"].ToString());
        ColorMeasuredXYZ.Y = Convert.ToDouble(dr["MY"].ToString());
        ColorMeasuredXYZ.Z = Convert.ToDouble(dr["MZ"].ToString());
        ColorMeasured = PerceptionLib.Color.ToLAB(ColorMeasuredXYZ, screenBgWt);

        dr["Bc_L"] = ColorMeasured.LA.ToString();
        dr["Bc_a"] = ColorMeasured.A.ToString();
        dr["Bc_b"] = ColorMeasured.B.ToString();
        ab = PerceptionLib.Color.ToRBGFromLAB(ColorMeasured);
        string Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
        dr["Bc_RRGGBB"] = "0x" + Hex;


        ////brad

        //TDBradXYZ.X = BradXYZ.X + TDBg.X;
        //TDBradXYZ.Y = BradXYZ.Y + TDBg.Y;
        //TDBradXYZ.Z = BradXYZ.Z + TDBg.Z;

        //TDBrad = PerceptionLib.Color.ToLAB(TDBradXYZ, screenBgWt);
        //dr["CAT1_P_Bc_TD_L"] = TDBrad.LA.ToString();
        //dr["CAT1_P_Bc_TD_a"] = TDBrad.A.ToString();
        //dr["CAT1_P_Bc_TD_b"] = TDBrad.B.ToString();
        //ab = PerceptionLib.Color.ToRBGFromLAB(TDBrad);
        //Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
        //dr["CAT1_P_Bc_TD_RRGGBB"] = "0x" + Hex;


        //DBradXYZ.X = BradXYZ.X + DBg.X;
        //DBradXYZ.Y = BradXYZ.Y + DBg.Y;
        //DBradXYZ.Z = BradXYZ.Z + DBg.Z;

        //DBrad = PerceptionLib.Color.ToLAB(DBradXYZ, screenBgWt);
        //dr["CAT1_P_Bc_ND_L"] = DBrad.LA.ToString();
        //dr["CAT1_P_Bc_ND_a"] = DBrad.A.ToString();
        //dr["CAT1_P_Bc_ND_b"] = DBrad.B.ToString();
        //ab = PerceptionLib.Color.ToRBGFromLAB(DBrad);
        //Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
        //dr["CAT1_P_Bc_ND_RRGGBB"] = "0x" + Hex;


        ////von
        //TDVonXYZ.X = VonXYZ.X + TDBg.X;
        //TDVonXYZ.Y = VonXYZ.Y + TDBg.Y;
        //TDVonXYZ.Z = VonXYZ.Z + TDBg.Z;

        //TDVon = PerceptionLib.Color.ToLAB(TDVonXYZ, screenBgWt);
        //dr["CAT2_P_Bc_TD_L"] = TDVon.LA.ToString();
        //dr["CAT2_P_Bc_TD_a"] = TDVon.A.ToString();
        //dr["CAT2_P_Bc_TD_b"] = TDVon.B.ToString();
        //ab = PerceptionLib.Color.ToRBGFromLAB(TDVon);
        //Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
        //dr["CAT2_P_Bc_TD_RRGGBB"] = "0x" + Hex;

        //DVonXYZ.X = VonXYZ.X + DBg.X;
        //DVonXYZ.Y = VonXYZ.Y + DBg.Y;
        //DVonXYZ.Z = VonXYZ.Z + DBg.Z;

        //DVon = PerceptionLib.Color.ToLAB(DVonXYZ, screenBgWt);
        //dr["CAT2_P_Bc_ND_L"] = DVon.LA.ToString();
        //dr["CAT2_P_Bc_ND_a"] = DVon.A.ToString();
        //dr["CAT2_P_Bc_ND_b"] = DVon.B.ToString();
        //ab = PerceptionLib.Color.ToRBGFromLAB(DVon);
        //Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
        //dr["CAT2_P_Bc_ND_RRGGBB"] = "0x" + Hex;

        //// scal

        //TDScalXYZ.X = ScalXYZ.X + TDBg.X;
        //TDScalXYZ.Y = ScalXYZ.Y + TDBg.Y;
        //TDScalXYZ.Z = ScalXYZ.Z + TDBg.Z;

        //TDScal = PerceptionLib.Color.ToLAB(TDScalXYZ, screenBgWt);
        //dr["CAT3_P_Bc_TD_L"] = TDScal.LA.ToString();
        //dr["CAT3_P_Bc_TD_a"] = TDScal.A.ToString();
        //dr["CAT3_P_Bc_TD_b"] = TDScal.B.ToString();
        //ab = PerceptionLib.Color.ToRBGFromLAB(TDScal);
        //Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
        //dr["CAT3_P_Bc_TD_RRGGBB"] = "0x" + Hex;

        //DScalXYZ.X = ScalXYZ.X + DBg.X;
        //DScalXYZ.Y = ScalXYZ.Y + DBg.Y;
        //DScalXYZ.Z = ScalXYZ.Z + DBg.Z;

        //DScal = PerceptionLib.Color.ToLAB(DScalXYZ, screenBgWt);
        //dr["CAT3_P_Bc_ND_L"] = DScal.LA.ToString();
        //dr["CAT3_P_Bc_ND_a"] = DScal.A.ToString();
        //dr["CAT3_P_Bc_ND_b"] = DScal.B.ToString();
        //ab = PerceptionLib.Color.ToRBGFromLAB(DScal);
        //Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
        //dr["CAT3_P_Bc_ND_RRGGBB"] = "0x" + Hex;


        ////bin
        //TDBinXYZ.X = BinXYZ.X + TDBg.X;
        //TDBinXYZ.Y = BinXYZ.Y + TDBg.Y;
        //TDBinXYZ.Z = BinXYZ.Z + TDBg.Z;

        //TDBin = PerceptionLib.Color.ToLAB(TDBinXYZ, screenBgWt);
        //dr["BP_P_Bc_TD_L"] = TDBin.LA.ToString();
        //dr["BP_P_Bc_TD_a"] = TDBin.A.ToString();
        //dr["BP_P_Bc_TD_b"] = TDBin.B.ToString();
        //ab = PerceptionLib.Color.ToRBGFromLAB(TDBin);
        //Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
        //dr["BP_P_Bc_TD_RRGGBB"] = "0x" + Hex;

        //DBinXYZ.X = BinXYZ.X + DBg.X;
        //DBinXYZ.Y = BinXYZ.Y + DBg.Y;
        //DBinXYZ.Z = BinXYZ.Z + DBg.Z;

        //DBin = PerceptionLib.Color.ToLAB(DBinXYZ, screenBgWt);
        //dr["BP_P_Bc_ND_L"] = DBin.LA.ToString();
        //dr["BP_P_Bc_ND_a"] = DBin.A.ToString();
        //dr["BP_P_Bc_ND_b"] = DBin.B.ToString();
        //ab = PerceptionLib.Color.ToRBGFromLAB(DBin);
        //Hex = PerceptionLib.Color.RGBtoHEX(ab.R, ab.G, ab.B);
        //dr["BP_P_Bc_ND_RRGGBB"] = "0x" + Hex;


        //dr["Dist_DM_P_TD"] = (PerceptionLib.Color.ColorDistanceCalAB(TDAcolor, ColorMeasured)).ToString();
        //dr["Dist_DM_P_ND"] = (PerceptionLib.Color.ColorDistanceCalAB(DAcolor, ColorMeasured)).ToString();

        //dr["Dist_CAT1_P_TD"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, TDBrad)).ToString();
        //dr["Dist_CAT1_P_ND"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, DBrad)).ToString();

        //dr["Dist_CAT2_P_TD"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, TDVon)).ToString();
        //dr["Dist_CAT2_P_ND"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, DVon)).ToString();

        //dr["Dist_CAT3_P_TD"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, TDScal)).ToString();
        //dr["Dist_CAT3_P_ND"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, DScal)).ToString();


        //dr["Dist_BP_P_TD"] = (PerceptionLib.Color.ColorDistanceCalAB(TDBin, ColorMeasured)).ToString();
        //dr["Dist_BP_P_ND"] = (PerceptionLib.Color.ColorDistanceCalAB(DBin, ColorMeasured)).ToString();

        //dr["Dist_Fg"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, Acolor)).ToString();
        //dr["Dist_CAT1_Fg"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, Brad)).ToString();
        //dr["Dist_CAT2_Fg"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, Von)).ToString();
        //dr["Dist_CAT3_Fg"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, Scal)).ToString();
        //dr["Dist_BP_Fg"] = (PerceptionLib.Color.ColorDistanceCalAB(ColorMeasured, Bin)).ToString();


      }

       
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = bin.DefaultView));
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
      btn_ExportGrid.IsEnabled = true;
    }

    private void Reduce_Click(object sender, RoutedEventArgs e)
    {
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\phone\phoneBin_full.csv");
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\Reduscer_outPut.csv");
      
      DataTable bin = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));


      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\phone\phoneBin_template.csv");
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\Reduscer.csv");
      DataTable binTable = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        binTable = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));

     // DataTable binTable = new DataTable();
      //binTable = bin.Clone();
      Random random = new Random();
      for (int j = 0; j < bin.Rows.Count; j = j++)
      {

        for (int i = 0; i < binTable.Rows.Count; i = i+200)
        {
          int bgno = Convert.ToInt32(binTable.Rows[i][0].ToString());
          int bgref = Convert.ToInt32(bin.Rows[j][3].ToString());
          if (bgno == bgref)
            binTable.ImportRow(bin.Rows[j]);
        }
      }
      Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = binTable.DefaultView));
      Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
      

      btn_ExportGrid.IsEnabled = true;
    }

    private void FileConvertor_Click(object sender, RoutedEventArgs e)
    {

      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\phone\phoneBin_full.csv");
      PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\Reduscer_BPND_cal.csv");
      DataTable bin = new DataTable();
      Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      {
        dtgrid_corrDisplay.Items.Refresh();
        bin = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      }));


      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\phone\phoneBin_template.csv");
      //PopulateGrid(@"C:\see-through-project\gt\STColorCorrection\Src\STColorPerception\bin\value\Reduscer_outPut.csv");
      //DataTable binTable = new DataTable();
      //Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
      //{
      //  dtgrid_corrDisplay.Items.Refresh();
      //  binTable = ((DataView)dtgrid_corrDisplay.ItemsSource).ToTable();

      //}));
       int randomNumber ;
      DataTable binTable = new DataTable();
      binTable = bin.Clone();
     
      int startPt=0;
      int EndPt=0;
       Random random = new Random();
      for (int bg = 1; bg < 24; bg++)
      {

        for (int i = startPt; i < bin.Rows.Count; i = i + 832)//417)
        {
          if (i > 19273)//9612)
          {
            EndPt = i;
            break;
          }
          while ( bg> Convert.ToInt32(bin.Rows[i][0].ToString()))
            i++;
          if (Convert.ToInt32(bin.Rows[i][0].ToString()) == bg)
            EndPt = i;
          else
            break;

         
        }

        int index = 0;
          for (int k = 0; k < 200; k++)
          {
            randomNumber = random.Next(startPt, EndPt);

            binTable.ImportRow(bin.Rows[randomNumber]);

            
            index++;
          }
          int p = index;
        startPt = EndPt;
        if (startPt > 19273)//9612)
          break;
       }

        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.ItemsSource = binTable.DefaultView));
        Dispatcher.Invoke(new Action(() => dtgrid_corrDisplay.Items.Refresh()));
      

      btn_ExportGrid.IsEnabled = true;

    }
    
    }
  

  }

