using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neda.API;
using Neda.Core;

namespace Neda.Desktop
{
	public class DesktopKernel
	{
		private readonly IHardware _hw;
		private readonly MainKernel _main;

		public DesktopKernel()
		{
			_hw = new DesktopHardware();
			_main = new MainKernel(_hw);
		}
	}
}