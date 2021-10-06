using System;
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
        private readonly int _cols;
        private readonly int _rows;
        private char[] _screen;

        public EmuWindow()
        {
            _cols = 80;
            _rows = 25;
            _screen = new char[_cols * _rows];
        }

        public void Clear()
        {
            for (var i = 0; i < _screen.Length; i++)
                _screen[i] = default;
            _currentRow = 0;
            _currentCol = 0;
            Refresh(this);
        }

        private int _currentRow = 0;
        private int _currentCol = 0;

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
                var index = (_currentRow * _cols) + _currentCol;
                _screen[index] = letter;
                _currentCol++;
                if (_currentCol >= _cols)
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

        private readonly StringBuilder _currentLine = new StringBuilder();
        private readonly ManualResetEvent _nextLineEvent = new ManualResetEvent(false);

        protected override bool OnKeyPressEvent(EventKey e)
        {
            var letter = (char)Gdk.Keyval.ToUnicode(e.KeyValue);
            if (letter == default)
            {
                Console.Error.WriteLine("Special key ?! " + e.Key);
            }
            else
            {
                if (e.Key == Gdk.Key.Return || e.Key == Gdk.Key.KP_Enter)
                {
                    _nextLineEvent.Set();
                }
                else
                {
                    _currentLine.Append(letter);
                }
            }
            return base.OnKeyPressEvent(e);
        }

        private void InsertNewLine()
        {
            _currentRow++;
            _currentCol = 0;
            if (_currentRow >= _rows)
            {
                _currentRow = _rows - 1;
                var newScreen = new char[_screen.Length];
                var start = 1 * _cols;
                Array.Copy(_screen, start, newScreen, 0, _screen.Length - start);
                _screen = newScreen;
            }
        }

        public string ReadLine()
        {
            _nextLineEvent.WaitOne();
            _nextLineEvent.Reset();
            var line = _currentLine.ToString();
            _currentLine.Clear();
            InsertNewLine();
            return line;
        }
    }
}