using Mabinogi;

namespace XMLDB3
{
	public class EquipmentCollectionCountListSerializer
	{
		public static void Deserialize(CollectionCountList _collectionCountList, Message _message)
		{
			if (_collectionCountList == null || _collectionCountList.collectionCounts == null)
			{
				_message.WriteS32(0);
				return;
			}
			_message.WriteS32(_collectionCountList.collectionCounts.Count);

			foreach (CollectionCount collectionCount in _collectionCountList.collectionCounts)
			{
				_message.WriteS32(collectionCount.itemType);
				_message.WriteS32(collectionCount.collectionId);
			}
		}
	}
}
