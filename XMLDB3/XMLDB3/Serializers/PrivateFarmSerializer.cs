using Mabinogi;

namespace XMLDB3
{
	public class PrivateFarmSerializer
	{
		public static PrivateFarm Serialize(Message _message)
		{
			PrivateFarm privateFarm = new PrivateFarm();
			privateFarm.bindedChannel = _message.ReadS16();
			privateFarm.nextBindableTime = _message.ReadS64();
			privateFarm.id = _message.ReadS64();
			privateFarm.ownerId = _message.ReadS64();
			privateFarm.classId = _message.ReadS32();
			privateFarm.level = _message.ReadS32();
			privateFarm.exp = _message.ReadS64();
			privateFarm.name = _message.ReadString();
			privateFarm.worldPosX = _message.ReadS16();
			privateFarm.worldPosY = _message.ReadS16();
			privateFarm.deleteFlag = _message.ReadU8();
			privateFarm.ownerName = _message.ReadString();
			_message.ReadS64();
			privateFarm.penalty = _message.ReadU32();
			privateFarm.field = PrivateFarmFieldSerializer.Serialize(_message);
			privateFarm.visitorList = PrivateFarmVisitorListSerializer.Serialize(_message);
			return privateFarm;
		}

		public static void Deserialize(PrivateFarm _privateFarm, Message _message)
		{
			_message.WriteS16(_privateFarm.bindedChannel);
			_message.WriteS64(_privateFarm.nextBindableTime);
			_message.WriteS64(_privateFarm.id);
			_message.WriteS64(_privateFarm.ownerId);
			_message.WriteS32(_privateFarm.classId);
			_message.WriteS32(_privateFarm.level);
			_message.WriteS64(_privateFarm.exp);
			_message.WriteString(_privateFarm.name);
			_message.WriteS16(_privateFarm.worldPosX);
			_message.WriteS16(_privateFarm.worldPosY);
			_message.WriteU8(_privateFarm.deleteFlag);
			_message.WriteString(_privateFarm.ownerName);
			_message.WriteS64(_privateFarm.updatetime.Ticks);
			_message.WriteU32(_privateFarm.penalty);
			PrivateFarmFieldSerializer.Deserialize(_privateFarm.field, _message);
			PrivateFarmVisitorListSerializer.Deserialize(_privateFarm.visitorList, _message);
		}
	}
}
