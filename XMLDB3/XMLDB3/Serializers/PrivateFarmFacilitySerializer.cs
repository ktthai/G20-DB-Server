using Mabinogi;

namespace XMLDB3
{
	public class PrivateFarmFacilitySerializer
	{
		public static PrivateFarmFacility Serialize(Message _message)
		{
			PrivateFarmFacility privateFarmFacility = new PrivateFarmFacility();
			privateFarmFacility.facilityId = _message.ReadS64();
			if (privateFarmFacility.facilityId == 0)
			{
				return null;
			}
			privateFarmFacility.privateFarmId = _message.ReadS64();
			privateFarmFacility.classId = _message.ReadS32();
			privateFarmFacility.customName = _message.ReadString();
			privateFarmFacility.x = _message.ReadS32();
			privateFarmFacility.y = _message.ReadS32();
			privateFarmFacility.dir = _message.ReadU8();
			privateFarmFacility.color = new int[9];
			for (int i = 0; i < 9; i++)
			{
				privateFarmFacility.color[i] = _message.ReadS32();
			}
			privateFarmFacility.finishTime = _message.ReadS64();
			privateFarmFacility.lastProcessingTime = _message.ReadS64();
			privateFarmFacility.linkedFacilityId = _message.ReadS64();
			privateFarmFacility.meta = _message.ReadString();
			privateFarmFacility.permissionFlag = _message.ReadS32();
			return privateFarmFacility;
		}

		public static void Deserialize(PrivateFarmFacility _facility, Message _message)
		{
			_message.WriteS64(_facility.facilityId);
			_message.WriteS64(_facility.privateFarmId);
			_message.WriteS32(_facility.classId);
			_message.WriteString(_facility.customName);
			_message.WriteS32(_facility.x);
			_message.WriteS32(_facility.y);
			_message.WriteU8(_facility.dir);
			for (int i = 0; i < 9; i++)
			{
				_message.WriteS32(_facility.color[i]);
			}
			_message.WriteS64(_facility.finishTime);
			_message.WriteS64(_facility.lastProcessingTime);
			_message.WriteS64(_facility.linkedFacilityId);
			_message.WriteString(_facility.meta);
			_message.WriteS32(_facility.permissionFlag);
		}
	}
}
