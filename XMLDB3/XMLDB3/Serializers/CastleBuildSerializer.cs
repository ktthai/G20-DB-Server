using Mabinogi;
using System;

namespace XMLDB3
{
	public class CastleBuildSerializer
	{
		public static CastleBuild Serialize(Message _message)
		{
			CastleBuild castleBuild = new CastleBuild();
			castleBuild.durability = _message.ReadS32();
			castleBuild.maxDurability = _message.ReadS32();
			castleBuild.buildState = _message.ReadU8();
			castleBuild.buildNextTime = new DateTime(_message.ReadS64());
			castleBuild.buildStep = _message.ReadU8();
			int num = _message.ReadS32();
			if (num > 0)
			{
				castleBuild.resource = new CastleBuildResource[num];
				for (int i = 0; i < num; i++)
				{
					castleBuild.resource[i] = CastleBuildResourceSerializer.Serialize(_message);
				}
			}
			else
			{
				castleBuild.resource = null;
			}
			return castleBuild;
		}

		public static void Deserialize(CastleBuild _build, Message _message)
		{
			if (_build == null)
			{
				_build = new CastleBuild();
			}
			_message.WriteS32(_build.durability);
			_message.WriteS32(_build.maxDurability);
			_message.WriteU8(_build.buildState);
			_message.WriteS64(_build.buildNextTime.Ticks);
			_message.WriteU8(_build.buildStep);
			if (_build.resource != null)
			{
				_message.WriteS32(_build.resource.Length);
				CastleBuildResource[] resource = _build.resource;
				foreach (CastleBuildResource resource2 in resource)
				{
					CastleBuildResourceSerializer.Deserialize(resource2, _message);
				}
			}
			else
			{
				_message.WriteS32(0);
			}
		}
	}
}
