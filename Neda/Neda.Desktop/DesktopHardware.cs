using Neda.API;

namespace Neda.Desktop
{
	public class DesktopHardware : IHardware, IPower
	{
		public DesktopHardware()
		{
			Power = this;
		}

		public IPower Power { get; }

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