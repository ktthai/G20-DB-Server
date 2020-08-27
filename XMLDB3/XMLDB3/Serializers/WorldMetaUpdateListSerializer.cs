using Mabinogi;
using System.Collections;
using System.Collections.Generic;

namespace XMLDB3
{
	public class WorldMetaUpdateListSerializer
	{
		private enum EMetaElementType
		{
			eSetValue = 1,
			eRemoveValue
		}

		public static bool Serialize(Message _message, out WorldMetaList _updatelist, out string[] _removeKeys)
		{
			List<WorldMeta> worldList = new List<WorldMeta>();
			List<string> stringList = new List<string>();
			_updatelist = null;
			_removeKeys = null;
			uint num = _message.ReadU32();
			for (uint num2 = 0u; num2 < num; num2++)
			{
				switch (_message.ReadU8())
				{
				case 1:
				{
					WorldMeta worldMeta = WorldMetaSerializer.Serialize(_message);
					if (worldMeta == null)
					{
						return false;
					}
					worldList.Add(worldMeta);
					break;
				}
				case 2:
				{
					string value = _message.ReadString();
					stringList.Add(value);
					break;
				}
				default:
					return false;
				}
			}
			_updatelist = new WorldMetaList();
			_updatelist.metas = worldList;
			_removeKeys = stringList.ToArray();
			return true;
		}
	}
}
