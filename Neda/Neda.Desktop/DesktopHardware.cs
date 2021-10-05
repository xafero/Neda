using Neda.API;

namespace Neda.Desktop
{
	public class DesktopHardware : IHardware, IPower
	{
		public DesktopHardware(IConsole con)
		{
			Power = this;
			Console = con;
		}

		public IPower Power { get; }
		public IConsole Console { get; }

		public void Shutdown()
		{
			throw new System.NotImplementedException();
		}

		public void Reboot()
		{
			throw new System.NotImplementedException();
		}
	}
}