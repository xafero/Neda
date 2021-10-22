namespace Neda.API
{
	/// <summary>
	/// stdio
	/// </summary>
	public interface IInputOutput
	{
		/// <summary>
		/// stdin
		/// </summary>
		IInStream StandardInput { get; }

		/// <summary>
		/// stdout
		/// </summary>
		IOutStream StandardOutput { get; }

		/// <summary>
		/// stderr
		/// </summary>
		IOutStream StandardError { get; }
	}
}