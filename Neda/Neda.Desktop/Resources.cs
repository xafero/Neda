using System.IO;

namespace Neda.Desktop
{
	internal static class Resources
	{
		internal static Stream FindResource(string path)
		{
			var type = typeof(Resources);
			var dll = type.Assembly;
			var fullName = type.FullName + path.Replace('/', '.');
			return dll.GetManifestResourceStream(fullName);
		}
	}
}