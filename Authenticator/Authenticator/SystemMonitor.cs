using System;
using System.Diagnostics;
using System.Threading;

namespace Authenticator
{
	public class SystemMonitor
	{
		public static int GetProcessorUsage()
		{
			Process currentProcess = Process.GetCurrentProcess();
			TimeSpan totalProcessorTime = currentProcess.TotalProcessorTime;
			Thread.Sleep(100);
			TimeSpan totalProcessorTime2 = currentProcess.TotalProcessorTime;
			return (totalProcessorTime2 - totalProcessorTime).Milliseconds;
		}

		public static int GetMemoryUsage()
		{
			return (int)Process.GetCurrentProcess().VirtualMemorySize64;
		}
	}
}
