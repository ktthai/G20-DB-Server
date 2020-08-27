using Mabinogi;

namespace XMLDB3
{
	public class RoyalAlchemistListSerializer
	{
		public static void Deserialize(RoyalAlchemistList _list, Message _message)
		{
			if (_list == null || _list.alchemists == null || _list.alchemists.Count == 0)
			{
				_message.WriteS32(0);
				return;
			}
			_message.WriteS32(_list.alchemists.Count);

			foreach (RoyalAlchemist data in _list.alchemists)
			{
				RoyalAlchemistSerializer.Deserialize(data, _message);
			}
		}
	}
}
