using Mabinogi;
using System.Text.Json;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class ConditionUpdateBuilder
	{
		private static string BuildConditionXmlData(CharacterCondition[] _conditions, out Message _outOverflowMsg)
		{
			_outOverflowMsg = new Message();
			if (_conditions != null && _conditions.Length > 0)
			{
				
				Message message = new Message();
				ushort num = 0;
				foreach (CharacterCondition characterCondition in _conditions)
				{
					num = (ushort)(num + 1);
					message.WriteU32((uint)characterCondition.flag);
					message.WriteU8((byte)characterCondition.timemode);
					message.WriteU64((ulong)characterCondition.time);
					if (characterCondition.meta != null && 0 < characterCondition.meta.Length)
					{
						message.WriteString(characterCondition.meta);
					}
				}
				if (0 < num)
				{
					_outOverflowMsg.WriteU16(num);
					_outOverflowMsg += message;
				}
				return JsonSerializer.Serialize(_conditions);
			}
			return string.Empty;
		}

		public static bool Build(Character _new, Character _old, SimpleCommand cmd, out Message _outBuildResultMsg)
		{
			bool result = false;
			_outBuildResultMsg = new Message();
			Message _outOverflowMsg;
			string text = BuildConditionXmlData(_new.conditions, out _outOverflowMsg);
			Message _outOverflowMsg2;
			string b = BuildConditionXmlData(_old.conditions, out _outOverflowMsg2);
			if (text != b)
			{
				if (!_outOverflowMsg.IsNull())
				{
					_outBuildResultMsg.WriteU32(1u);
					_outBuildResultMsg += _outOverflowMsg;
				}
				cmd.Set(Mabinogi.SQL.Columns.Character.Condition, text);
                result = true;
            }
			return result;
		}
	}
}
