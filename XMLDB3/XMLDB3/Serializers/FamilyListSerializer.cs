using Mabinogi;

namespace XMLDB3
{
	public class FamilyListSerializer
	{
		public static void Deserialize(FamilyList _list, Message _message)
		{
			if (_list == null || _list.Families == null || _list.Families.Count == 0)
			{
				_message.WriteS32(0);
				return;
			}
			_message.WriteS32(_list.Families.Count);

			foreach (FamilyListFamily data in _list.Families)
			{
				FamilySerializer.Deserialize(data, _message);
			}
		}
	}
}
