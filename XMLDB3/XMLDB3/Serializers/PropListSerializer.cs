using Mabinogi;

namespace XMLDB3
{
	public class PropListSerializer
	{
		public static void Deserialize(PropIDList _list, Message _message)
		{
			if (_list == null)
			{
				_list = new PropIDList();
			}
			if (_list.propID != null)
			{
				_message.WriteS32(_list.propID.Count);

				foreach (long data in _list.propID)
				{
					_message.WriteS64(data);
				}
			}
			else
			{
				_message.WriteS32(0);
			}
		}
	}
}
