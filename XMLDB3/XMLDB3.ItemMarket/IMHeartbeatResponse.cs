using Mabinogi;
using System.IO;

namespace XMLDB3.ItemMarket
{
	public class IMHeartbeatResponse : ItemMarketResponse
	{
		public override bool IsSystemMessage => true;

		public override void Build(BinaryReader _br, Message _message)
		{
			result = _br.ReadByte();
		}
	}
}
