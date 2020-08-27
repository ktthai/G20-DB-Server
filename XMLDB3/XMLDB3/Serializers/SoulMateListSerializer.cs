using Mabinogi;

namespace XMLDB3
{
	public class SoulMateListSerializer
	{
		public static void Deserialize(SoulMateList _list, Message _message)
		{
			if (_list == null || _list.soulmate == null || _list.soulmate.Count == 0)
			{
				_message.WriteS32(0);
				return;
			}
			_message.WriteS32(_list.soulmate.Count);

			foreach (SoulMate data in _list.soulmate)
			{
				SoulMateSerializer.Deserialize(data, _message);
			}
		}
	}
}
