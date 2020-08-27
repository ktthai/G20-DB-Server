using Mabinogi;
using System.Collections.Generic;

namespace XMLDB3
{
	public class HouseBlockSerializer
	{
		public static void Deserialize(List<HouseBlock> _blocks, Message _message)
		{
			if (_blocks != null)
			{
				_message.WriteS32(_blocks.Count);
				foreach (HouseBlock houseBlock in _blocks)
				{
					_message.WriteString(houseBlock.gameName);
					_message.WriteU8(houseBlock.flag);
				}
			}
			else
			{
				_message.WriteS32(0);
			}
		}

		public static List<HouseBlock> Serialize(Message _message)
		{
			int num = _message.ReadS32();
			if (num > 0)
			{
				List<HouseBlock> list = new List<HouseBlock>(num);
				for (int i = 0; i < num; i++)
				{
					list[i] = new HouseBlock();
					list[i].gameName = _message.ReadString();
					list[i].flag = _message.ReadU8();
				}
				return list;
			}
			return null;
		}
	}
}
