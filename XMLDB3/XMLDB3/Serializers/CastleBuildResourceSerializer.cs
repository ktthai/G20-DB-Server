using Mabinogi;

namespace XMLDB3
{
	public class CastleBuildResourceSerializer
	{
		public static CastleBuildResource Serialize(Message _message)
		{
			CastleBuildResource castleBuildResource = new CastleBuildResource();
			castleBuildResource.classID = _message.ReadS32();
			castleBuildResource.curAmount = _message.ReadS32();
			castleBuildResource.maxAmount = _message.ReadS32();
			return castleBuildResource;
		}

		public static void Deserialize(CastleBuildResource _resource, Message _message)
		{
			_message.WriteS32(_resource.classID);
			_message.WriteS32(_resource.curAmount);
			_message.WriteS32(_resource.maxAmount);
		}
	}
}
