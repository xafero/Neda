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
        private readonly Timer _cursorTimer;
        private readonly Size _size;
        private Cursor _cursor;
        private char[] _screen;

        public EmuWindow()
        {
            _size = new Size { Cols = 80, Rows = 25 };
            _cursor = new Cursor { Row = 0, Col = 0 };
            _screen = new char[_size.Cols * _size.Rows];

            var duration = 250;
            _cursorTimer = new Timer(OnCursorTimer, this, duration, duration);
        }

        private void OnCursorTimer(object state)
        {
            var index = (_cursor.Row * _size.Cols) + _cursor.Col;
            if (_screen[index] == '_')
                _screen[index] = ' ';
            else
                _screen[index] = '_';
            Refresh((Widget)state);
        }

        public void Clear()
        {
            for (var i = 0; i < _screen.Length; i++)
                _screen[i] = default;
            _cursor.Row = 0;
            _cursor.Col = 0;
            Refresh(this);
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
                var index = (_cursor.Row * _size.Cols) + _cursor.Col;
                _screen[index] = letter;
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

            return false;
        }

        private readonly StringBuilder _currentLine = new StringBuilder();
        private readonly ManualResetEvent _nextLineEvent = new ManualResetEvent(false);

        protected override bool OnKeyPressEvent(EventKey e)
        {
            if (e.Key == Gdk.Key.Return || e.Key == Gdk.Key.KP_Enter)
            {
                _nextLineEvent.Set();
            }
            else if (e.Key == Gdk.Key.BackSpace)
            {
                //
            }
            else if (e.Key == Gdk.Key.Left)
            {
                // 
            }
            else if (e.Key == Gdk.Key.Right)
            {
                //
            }
            else
            {
                var letter = (char)Gdk.Keyval.ToUnicode(e.KeyValue);
                if (letter == default)
                {
                    Console.Error.WriteLine("Special key ?! " + e.Key);
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
            _nextLineEvent.WaitOne();
            _nextLineEvent.Reset();
            var line = _currentLine.ToString();
            _currentLine.Clear();
            InsertNewLine();
            return line;
        }
    }
}