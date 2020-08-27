using Mabinogi;

namespace XMLDB3
{
	public class WorldMetaListSerializer
	{
		public static void Deserialize(WorldMetaList _list, Message _message)
		{
			if (_list == null || _list.metas == null || _list.metas.Count == 0)
			{
				_message.WriteS32(0);
				return;
			}
			_message.WriteS32(_list.metas.Count);

			foreach (WorldMeta worldmeta in _list.metas)
			{
				WorldMetaSerializer.Deserialize(worldmeta, _message);
			}
		}
	}
}
