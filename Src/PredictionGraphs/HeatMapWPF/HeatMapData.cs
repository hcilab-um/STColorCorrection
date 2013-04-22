using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HeatMapWPF.Properties;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;

namespace HeatMapWPF
{
	class HeatMapData
	{
		public const int GRID_SIZE = 30;

		public static readonly double DataWidth = Settings.Default.DataMapWidth / GRID_SIZE;
		public static readonly double DataHeight = Settings.Default.DataMapHeight / GRID_SIZE;

		public static readonly double GraphWidth = Settings.Default.HeatMapPixelWidth/GRID_SIZE;
		public static readonly double GraphHeight = Settings.Default.HeatMapPixelHeight/GRID_SIZE;

		public int DataSize { get; set; }
		public double DataSum { get; set; }
		public double DataAverageValue 
		{
			get 
			{
				if (DataSize == 0)
					return 0;
				return DataSum / DataSize; 
			} 
		}

		public double GraphX { get; set; }
		public double GraphY { get; set; }

		public double DataX { get; set; }
		public double DataY { get; set; }

		public int Row { get; set; }
		public int Column { get; set; }

    public bool IsOutsideGammut { get; set; }

		public HeatMapData(int row, int col) 
		{
			Row = row;
			Column = col;
			DataSize = 0;
			DataSum = 0;
			GraphY = (GRID_SIZE - row - 1) * GraphHeight;
			GraphX = col * GraphWidth;

			DataX = col * DataWidth-100;
			DataY = (GRID_SIZE - row - 1) * DataHeight-100;
      IsOutsideGammut = false;
		}

		public Rectangle drawRectangle()
		{
			Rectangle rect = new Rectangle();
			rect.Width = GraphWidth;
			rect.Height = GraphHeight;

      if (!IsOutsideGammut)
      {
        rect.Fill = Brushes.Blue;
        double opacity = 0;
        if (DataAverageValue >= 100)
          opacity = 1;
        else
          opacity = DataAverageValue / 100;
        rect.Opacity = opacity;
      }
      else
      {
        rect.Fill = Brushes.Red;
        rect.Opacity = 1;
      }

			Canvas.SetLeft(rect, GraphX);
			Canvas.SetBottom(rect, GraphY);
			return rect;
		}

	}
}
