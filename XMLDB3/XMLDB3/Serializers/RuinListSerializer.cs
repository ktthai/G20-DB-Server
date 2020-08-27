using Mabinogi;

namespace XMLDB3
{
	public class RuinListSerializer
	{
		public static Message Deserialize(RuinList _list, Message _Msg)
		{
			if (_list.ruins != null && _list.ruins.Length > 0)
			{
				_Msg.WriteS32(_list.ruins.Length);
				Ruin[] ruins = _list.ruins;
				foreach (Ruin ruin in ruins)
				{
					RuinSerializer.Deserialize(ruin, _Msg);
				}
			}
			else
			{
				_Msg.WriteS32(0);
			}
			return _Msg;
		}

		public static RuinList Serialize(Message _Msg)
		{
			RuinList ruinList = new RuinList();
			int num = _Msg.ReadS32();
			if (num > 0)
			{
				ruinList.ruins = new Ruin[num];
				for (int i = 0; i < num; i++)
				{
					ruinList.ruins[i] = RuinSerializer.Serialize(_Msg);
				}
			}
			else
			{
				ruinList.ruins = null;
			}
			return ruinList;
		}
	}
}
