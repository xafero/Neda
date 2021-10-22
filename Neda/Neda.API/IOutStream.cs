namespace Neda.API
{
	public interface IOutStream
	{
		void Write(string text);

		void WriteLine(string text = null);
	}
}