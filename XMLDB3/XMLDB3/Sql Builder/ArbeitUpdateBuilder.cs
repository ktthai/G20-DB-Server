using Mabinogi;
using Mabinogi.SQL;
using System.Text.Json;

namespace XMLDB3
{
	public class ArbeitUpdateBuilder
	{
		private static string BuildHistoryXmlData(CharacterArbeitDay[] _history, out Message _outOverflowMsg)
		{
			_outOverflowMsg = new Message();
			if (_history != null && _history.Length > 0)
			{
				string text = JsonSerializer.Serialize(_history);
				Message message = new Message();
				ushort num = 0;
				foreach (CharacterArbeitDay characterArbeitDay in _history)
				{
					num = (ushort)(num + 1);
					message.WriteU32((uint)characterArbeitDay.daycount);
					if (characterArbeitDay.info != null && characterArbeitDay.info.Length > 0)
					{
						message.WriteU32((uint)characterArbeitDay.info.Length);
						CharacterArbeitDayInfo[] info2 = characterArbeitDay.info;
						foreach (CharacterArbeitDayInfo characterArbeitDayInfo2 in info2)
						{
							message.WriteU16((ushort)characterArbeitDayInfo2.category);
						}
					}
					else
					{
						message.WriteU32(0u);
					}
				}
				if (0 < num)
				{
					_outOverflowMsg.WriteU16(num);
					_outOverflowMsg += message;
				}
				return text;
			}
			return string.Empty;
		}

		public static bool Build(Character _new, Character _old, SimpleCommand cmd, out Message _outBuildResultMsg)
		{
			bool result = false;
			_outBuildResultMsg = new Message();
			if (_old.arbeit != null && _new.arbeit != null)
			{
				Message _outOverflowMsg;
				string text = BuildHistoryXmlData(_new.arbeit.history, out _outOverflowMsg);
				Message _outOverflowMsg2;
				string b = BuildHistoryXmlData(_old.arbeit.history, out _outOverflowMsg2);

				string text2 = JsonSerializer.Serialize(_new.arbeit.collection);
				string b2 = JsonSerializer.Serialize(_old.arbeit.collection);

				if (text != b)
				{
					if (!_outOverflowMsg.IsNull())
					{
						_outBuildResultMsg.WriteU32(2u);
						_outBuildResultMsg += _outOverflowMsg;
					}
					cmd.Set(Mabinogi.SQL.Columns.Character.History, text);
                    result = true;
                }
				if (text2 != b2)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Collection, text2);
                    result = true;
                }
			}
			return result;
		}
	}
}
