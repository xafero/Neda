using System;
using System.Text;
using Cairo;
using Gtk;
using Context = Cairo.Context;

namespace Neda.Desktop
{
	internal class EmuWindow : DrawingArea
	{
		private readonly int _cols;
		private readonly int _rows;
		private readonly char[] _screen;

		public EmuWindow()
		{
			_cols = 80;
			_rows = 25;
			_screen = new char[_cols * _rows];
		}

		protected override bool OnDrawn(Context g)
		{
			var width = AllocatedWidth;
			var height = AllocatedHeight;

			g.SetSourceRGB(0, 0, 0);
			g.Rectangle(0, 0, width, height);
			g.Fill();

			var wFactor = width / (Program.Width * 1d);
			var hFactor = height / (Program.Height * 1d);
			var factor = Math.Min(wFactor, hFactor);

			g.Scale(factor, factor);

			const int xShift = 10;
			const int yShift = 20;
			const int ySize = 10;

			g.SelectFontFace("Courier New", FontSlant.Normal, FontWeight.Bold);
			g.SetFontSize(13);

			for (var y = 0; y < _rows; y++)
			{
				var bld = new StringBuilder();
				for (var x = 0; x < _cols; x++)
				{
					var arrayIndex = y * _cols + x;
					var letter = _screen[arrayIndex];
					bld.Append(letter);
				}
				g.SetSourceRGB(255, 255, 255);
				g.MoveTo(xShift, yShift + (ySize * y));
				g.ShowText(bld.ToString());
			}

			return false;
		}
	}
}