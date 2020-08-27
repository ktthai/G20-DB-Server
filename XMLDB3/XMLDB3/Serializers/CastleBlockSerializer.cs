using Mabinogi;

namespace XMLDB3
{
	public class CastleBlockSerializer
	{
		public static CastleBlock[] Serialize(Message _message)
		{
			int num = _message.ReadS32();
			if (num == 0)
			{
				return null;
			}
			CastleBlock[] array = new CastleBlock[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = new CastleBlock();
				array[i].gameName = _message.ReadString();
				array[i].flag = _message.ReadU8();
				array[i].entry = _message.ReadU8();
			}
			return array;
		}

		public static void Deserialize(CastleBlock[] _blocks, Message _message)
		{
			if (_blocks == null)
			{
				_message.WriteS32(0);
				return;
			}
			_message.WriteS32(_blocks.Length);
			foreach (CastleBlock castleBlock in _blocks)
			{
				_message.WriteString(castleBlock.gameName);
				_message.WriteU8(castleBlock.flag);
				_message.WriteU8(castleBlock.entry);
			}
		}
	}
}
