using Mabinogi;

namespace XMLDB3
{
	public class WorldMetaSerializer
	{
		private enum EMetaElementType
		{
			metaNotAvail,
			metaByte,
			metaWord,
			metaDWord,
			metaQWord,
			metaBool,
			metaFloat,
			metaDouble,
			metaString,
			metaBinary,
			metaPtr
		}

		public static WorldMeta Serialize(Message _message)
		{
			WorldMeta worldMeta = new WorldMeta();
			worldMeta.key = _message.ReadString();
			worldMeta.type = _message.ReadU8();
			switch (worldMeta.type)
			{
			case 1:
				worldMeta.value = _message.ReadU8().ToString();
				break;
			case 2:
				worldMeta.value = _message.ReadU16().ToString();
				break;
			case 3:
				worldMeta.value = _message.ReadU32().ToString();
				break;
			case 4:
				worldMeta.value = _message.ReadU64().ToString();
				break;
			case 5:
				worldMeta.value = (_message.ReadU8() != 0).ToString();
				break;
			case 6:
				worldMeta.value = _message.ReadFloat().ToString();
				break;
			case 8:
				worldMeta.value = _message.ReadString();
				break;
			default:
				return null;
			}
			return worldMeta;
		}

		public static void Deserialize(WorldMeta _worldmeta, Message _message)
		{
			if (_worldmeta != null)
			{
				_message.WriteString(_worldmeta.key);
				_message.WriteU8(_worldmeta.type);
				switch (_worldmeta.type)
				{
				case 7:
					break;
				case 1:
					_message.WriteU8(byte.Parse(_worldmeta.value));
					break;
				case 2:
					_message.WriteU16(ushort.Parse(_worldmeta.value));
					break;
				case 3:
					_message.WriteU32(uint.Parse(_worldmeta.value));
					break;
				case 4:
					_message.WriteU64(ulong.Parse(_worldmeta.value));
					break;
				case 5:
					_message.WriteU8((byte)(bool.Parse(_worldmeta.value) ? 1 : 0));
					break;
				case 6:
					_message.WriteFloat(float.Parse(_worldmeta.value));
					break;
				case 8:
					_message.WriteString(_worldmeta.value);
					break;
				}
			}
		}
	}
}
