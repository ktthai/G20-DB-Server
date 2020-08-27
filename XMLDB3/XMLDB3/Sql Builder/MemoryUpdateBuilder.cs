using Mabinogi;
using System.Text.Json;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class MemoryUpdateBuilder
	{
		private static string BuildMemoryXmlData(CharacterMemory[] _memorys, out Message _outOverflowMsg)
		{
			_outOverflowMsg = new Message();
			if (_memorys != null && _memorys.Length > 0)
			{
				
				Message message = new Message();
				ushort num = 0;
				foreach (CharacterMemory characterMemory in _memorys)
				{
					num = (ushort)(num + 1);
					message.WriteString(characterMemory.target);
					message.WriteU8(characterMemory.favor);
					message.WriteU8(characterMemory.memory);
					message.WriteU8(characterMemory.time_stamp);
				}
				if (0 < num)
				{
					_outOverflowMsg.WriteU16(num);
					_outOverflowMsg += message;
				}
				return JsonSerializer.Serialize( _memorys );
			}
			return string.Empty;
		}

		public static bool Build(Character _new, Character _old, SimpleCommand cmd, out Message _outBuildResultMsg)
		{
			bool result = false;
			_outBuildResultMsg = new Message();
			Message _outOverflowMsg;
			string text = BuildMemoryXmlData(_new.memorys, out _outOverflowMsg);
			Message _outOverflowMsg2;
			string b = BuildMemoryXmlData(_old.memorys, out _outOverflowMsg2);
			if (text != b)
			{
				if (!_outOverflowMsg.IsNull())
				{
					_outBuildResultMsg.WriteU32(3u);
					_outBuildResultMsg += _outOverflowMsg;
				}

				cmd.Set(Mabinogi.SQL.Columns.Character.Memory, text);
                result = true;
            }
			return result;
		}
	}
}
