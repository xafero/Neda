using System.IO;

namespace Neda.Desktop.Core
{
	internal static class Resources
	{
		internal static Stream FindResource(string path)
		{
			var type = typeof(Resources);
			var dll = type.Assembly;
			var name = type.FullName?.Replace('.' + nameof(Core), "");
			var fullName = name + path.Replace('/', '.');
			return dll.GetManifestResourceStream(fullName);
		}
	}
}