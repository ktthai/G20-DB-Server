using System;
using System.Text;

namespace XMLDB3
{
	public class PrivateFarmUpdateTimeUpdateBuilder
	{
		public static string Build(PrivateFarm _new, PrivateFarm _old)
		{
			if (_new == null)
			{
				throw new ArgumentException("개인농장 데이터가 없습니다1.", "_new");
			}
			if (_old == null)
			{
				throw new ArgumentException("개인농장 캐쉬 데이터가 없습니다1.", "_old");
			}
			StringBuilder stringBuilder = new StringBuilder(512);
			stringBuilder.Append(" @id=" + _new.id);
			stringBuilder.Append("\n");
			return "exec UpdatePrivateFarmUpdateTime" + stringBuilder.ToString();
		}
	}
}
