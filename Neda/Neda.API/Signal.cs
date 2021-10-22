namespace Neda.API
{
	public enum Signal
	{
		/// <summary>
		/// Sent to a process to tell it to abort/terminate
		/// </summary>
		SIGABRT,

		/// <summary>
		/// Sent to a process when the time limit specified elapses
		/// </summary>
		SIGALRM,

		/// <summary>
		/// Sent to a process when it causes a bus error
		/// </summary>
		SIGBUS,

		/// <summary>
		/// Sent to a process when a child process terminates
		/// </summary>
		SIGCHLD,

		/// <summary>
		/// Instructs the operating system to restart a process previously paused
		/// </summary>
		SIGCONT,

		/// <summary>
		/// Sent to a process when its controlling terminal is closed
		/// </summary>
		SIGHUP,

		/// <summary>
		/// Sent to a process when it attempts illegal or malformed instruction
		/// </summary>
		SIGILL,

		/// <summary>
		/// Sent to a process when a user wishes to interrupt the process
		/// </summary>
		SIGINT,

		/// <summary>
		/// Sent to a process to cause it to terminate immediately (no clean-up)
		/// </summary>
		SIGKILL,

		/// <summary>
		/// Sent to a process to request its termination
		/// </summary>
		SIGTERM,

		/// <summary>
		/// Sent to a process when it writes to a not connected pipe
		/// </summary>
		SIGPIPE,

		/// <summary>
		/// Sent when an event occurred on an explicitly watched file
		/// </summary>
		SIGPOLL,

		/// <summary>
		/// Sent to a process when it makes an invalid virtual memory reference
		/// </summary>
		SIGSEGV,

		/// <summary>
		/// Instructs the operating system to stop a process for later resumption
		/// </summary>
		SIGSTOP,

		/// <summary>
		/// Sent to a process to request it to stop
		/// </summary>
		SIGTSTP,

		/// <summary>
		/// Sent to a process when an exception or trap occurs
		/// </summary>
		SIGTRAP,

		/// <summary>
		/// Sent to a process when its terminal changes its size
		/// </summary>
		SIGWINCH,

		/// <summary>
		/// Sent to a process when the system experiences a power failure
		/// </summary>
		SIGPWR
	}
}