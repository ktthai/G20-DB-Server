using System;
using System.IO;
using System.Text;

namespace XMLDB3
{
	public class MailSender
	{
		private const string header = "[mabi/dbserver/report]";

		private const string defaultTitle = "DB 서버에서 오류 발생";

		private static object thisLock = new object();

		public static void Send(string _html)
		{
			Send("DB 서버에서 오류 발생", _html);
		}

		public static void Send(string _title, string _html)
		{
			lock (thisLock)
			{
				string path = "_mail/" + DateTime.Now.Year + DateTime.Now.Month.ToString("D02") + DateTime.Now.Day.ToString("D02") + "_" + _title + ".txt";
				FileStream fileStream = null;
				StreamWriter streamWriter = null;
				try
				{
					Directory.CreateDirectory("_mail");
					fileStream = (File.Exists(path) ? File.Open(path, FileMode.Append, FileAccess.Write) : File.Create(path));
					streamWriter = new StreamWriter(fileStream, Encoding.Unicode);
					streamWriter.WriteLine(DateTime.Now.ToLongTimeString() + "\t" + _html);
					streamWriter.Close();
				}
				catch (Exception ex)
				{
					ExceptionMonitor.ExceptionRaised(ex);
				}
				finally
				{
					streamWriter?.Close();
					fileStream?.Close();
				}
			}
		}
	}
}
