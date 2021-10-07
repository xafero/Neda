using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Cairo;
using Gdk;
using Gtk;
using Neda.API;
using Context = Cairo.Context;

namespace Neda.Desktop
{
	internal class EmuWindow : DrawingArea, IConsole
	{
		private readonly Timer _cursorTimer;
		private readonly Size _size;
		private readonly IList<(int r, int c, char s)> _layer;
		private Cursor _cursor;
		private char[] _screen;
		private Cursor _editAnchor;

		public EmuWindow()
		{
			_size = new Size { Cols = 80, Rows = 25 };
			_cursor = new Cursor { Row = 0, Col = 0 };
			_screen = new char[_size.Cols * _size.Rows];
			_layer = new List<(int r, int c, char s)>();

			const int duration = 250;
			_cursorTimer = new Timer(OnCursorTimer, this, duration, duration);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			_cursorTimer.Dispose();
		}

		private void OnCursorTimer(object state)
		{
			const char cSign = '_';
			const char lSign = ' ';
			char letter;
			if (_layer.FirstOrDefault().s == cSign)
				letter = lSign;
			else
				letter = cSign;
			_layer.Clear();
			_layer.Add((r: _cursor.Row, c: _cursor.Col, s: letter));
			Refresh((Widget)state);
		}

		public void Clear()
		{
			for (var i = 0; i < _screen.Length; i++)
				_screen[i] = default;
			_cursor.Row = 0;
			_cursor.Col = 0;
			_editAnchor = default;
			Refresh(this);
		}

		private void SetChar(int row, int col, char letter)
		{
			var index = (row * _size.Cols) + col;
			_screen[index] = letter;
		}

		public void Write(string text)
		{
			for (var i = 0; i < text.Length; i++)
			{
				var letter = text[i];
				if (letter == '\r')
					continue;
				if (letter == '\n')
				{
					InsertNewLine();
					continue;
				}
				SetChar(_cursor.Row, _cursor.Col, letter);
				_cursor.Col++;
				if (_cursor.Col >= _size.Cols)
					InsertNewLine();
			}
			Refresh(this);
		}

		public void WriteLine(string text)
			=> Write(text + Environment.NewLine);

		private static void Refresh(Widget widget)
			=> Application.Invoke((o, e) => widget.QueueDraw());

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

			for (var y = 0; y < _size.Rows; y++)
			{
				var bld = new StringBuilder();
				for (var x = 0; x < _size.Cols; x++)
				{
					var arrayIndex = y * _size.Cols + x;
					var letter = _screen[arrayIndex];
					bld.Append(letter);
				}
				g.SetSourceRGB(255, 255, 255);
				g.MoveTo(xShift, yShift + (ySize * y));
				g.ShowText(bld.ToString());
			}

			foreach (var (r, c, s) in _layer)
			{
				g.SetSourceRGB(255, 255, 255);
				g.MoveTo(xShift, yShift + (ySize * r));
				var spaces = string.Join(string.Empty, Enumerable.Repeat(' ', c));
				g.ShowText(spaces + s);
			}

			return false;
		}

		private readonly StringBuilder _currentLine = new();
		private readonly ManualResetEvent _nextLineEvent = new(false);

		protected override bool OnKeyPressEvent(EventKey e)
		{
			if (e.Key == Gdk.Key.Return || e.Key == Gdk.Key.KP_Enter)
			{
				_nextLineEvent.Set();
			}
			else if (e.Key == Gdk.Key.BackSpace)
			{
				if (_cursor.Col >= 1 && _currentLine.Length >= 1)
				{
					_cursor.Col--;
					SetChar(_cursor.Row, _cursor.Col, default);
					_currentLine.Remove(_currentLine.Length - 1, 1);
				}
			}
			else if (e.Key == Gdk.Key.Left)
			{
				if (_cursor.Col >= 1 && _cursor.Col > _editAnchor.Col)
					_cursor.Col--;
			}
			else if (e.Key == Gdk.Key.Right)
			{
				if (_cursor.Col < (_size.Cols - 1) && _cursor.Col < (_editAnchor.Col + _currentLine.Length))
					_cursor.Col++;
			}
			else
			{
				var letter = (char)Keyval.ToUnicode(e.KeyValue);
				if (letter == default)
				{
					Console.Error.WriteLine("Special key ?! " + e.Key);
				}
				else
				{
					_currentLine.Append(letter);
					Write(letter.ToString());
				}
			}
			return base.OnKeyPressEvent(e);
		}

		private void InsertNewLine()
		{
			_cursor.Row++;
			_cursor.Col = 0;
			if (_cursor.Row >= _size.Rows)
			{
				_cursor.Row = _size.Rows - 1;
				var newScreen = new char[_screen.Length];
				var start = 1 * _size.Cols;
				Array.Copy(_screen, start, newScreen, 0, _screen.Length - start);
				_screen = newScreen;
			}
		}

		public string ReadLine()
		{
			_editAnchor = _cursor;
			_nextLineEvent.WaitOne();
			_nextLineEvent.Reset();
			var line = _currentLine.ToString();
			_currentLine.Clear();
			InsertNewLine();
			return line;
		}
	}
}