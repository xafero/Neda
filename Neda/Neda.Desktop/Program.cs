using System;
using Gdk;
using Gtk;
using Window = Gtk.Window;

namespace Neda.Desktop
{
	internal static class Program
	{
		private static readonly int width = 640;
		private static readonly int height = 480;

		private static void Main(string[] _)
		{
			Application.Init();

			var gtk = new EmuWindow();

			var frame = new Window("Neda Desktop");
			frame.DeleteEvent += delegate
			{
				Application.Quit();
				Environment.Exit(0);
			};
			frame.SetDefaultSize((int)(width * 1.5), (int)(height * 1.5));
			frame.SetPosition(WindowPosition.Center);

			var icon = Resources.FindResource("/icons/app.png");
			frame.Icon = new Pixbuf(icon);

			frame.Add(gtk);
			frame.ShowAll();

			Application.Run();
		}
	}
}