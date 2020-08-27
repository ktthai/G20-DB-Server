using System;

namespace XMLDB3
{
	public class UpdateUtility
	{
		private static DateTime cacheMissDate;

		public static DateTime CacheMissDate => cacheMissDate;

		static UpdateUtility()
		{
			cacheMissDate = new DateTime(2000, 1, 1, 0, 0, 0, 0);
		}

		public static string BuildString(string _data)
		{
			if (_data != null)
			{
				_data = _data.Replace("'", "''");
				return "'" + _data + "'";
			}
			return "''";
		}

		public static string BuildDateTime(DateTime _data)
		{
			return "'" + _data.Year + '-' + _data.Month + '-' + _data.Day + ' ' + _data.Hour + ':' + _data.Minute + ":" + _data.Second + "." + _data.Millisecond + "'";
		}
	}
}
