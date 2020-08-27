using System;
using System.Threading;
using Authenticator;

namespace XMLDB_Console
{
	internal class XMLDBConsole
	{
		[MTAThread]
		private static void Main(string[] args)
		{
			Thread authThread;
			if (args.Length > 0 && args[0].ToLower() == "db")
            {
				XMLDB3.XMLDB3.Start();
			}
			else if (args.Length > 0 && args[0].ToLower() == "auth")
			{
				Authenticator.Authenticator.Start();
			}
			else
            {
                authThread = new Thread(Authenticator.Authenticator.Start);
                authThread.Start();
                XMLDB3.XMLDB3.Start();
            }
		}
	}
}
