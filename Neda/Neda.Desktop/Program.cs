using System;
using System.Threading;
using Gdk;
using Gtk;
using Neda.Desktop.Core;
using Window = Gtk.Window;

namespace Neda.Desktop
{
	internal static class Program
	{
		internal static readonly int Width = 645;
		internal static readonly int Height = 280;

		private static void Main(string[] _)
		{
			Application.Init();

			var gtk = new EmuWindow();
			gtk.Shown += OnEmuStart;
			gtk.CanFocus = true;

			var frame = new Window("Neda Desktop");
			frame.DeleteEvent += DoQuit;
			frame.SetDefaultSize((int)(Width * 1.5), (int)(Height * 1.5));
			frame.SetPosition(WindowPosition.Center);

			var mb = new MenuBar();
			var fileMenu = new Menu();
			var file = new MenuItem("File") { Submenu = fileMenu };
			var exit = new MenuItem("Exit");
			exit.Activated += DoQuit;
			fileMenu.Append(exit);
			mb.Append(file);

			var box = new VBox(false, 2);
			box.PackStart(mb, false, false, 0);
			box.Add(gtk);

			var icon = Resources.FindResource("/icons/app.png");
			frame.Icon = new Pixbuf(icon);

			frame.Add(box);
			frame.ShowAll();

			Application.Run();
		}

		private static void DoQuit<T>(object o, T args) where T : EventArgs
		{
			Application.Quit();
			Environment.Exit(0);
		}

		private static void OnEmuStart(object sender, EventArgs e)
		{
			var gtk = (EmuWindow)sender;
			gtk.Shown -= OnEmuStart;
			gtk.GrabFocus();
			ThreadPool.QueueUserWorkItem(kernel =>
			{
				kernel.BeforeRun();
				kernel.Run();
				kernel.AfterRun();
			}, new DesktopKernel(gtk), false);
		}
	}
}