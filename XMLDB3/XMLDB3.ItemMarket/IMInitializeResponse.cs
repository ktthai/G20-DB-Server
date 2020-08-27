using Mabinogi;
using System.IO;

namespace XMLDB3.ItemMarket
{
	public class IMInitializeResponse : ItemMarketResponse
	{
		public override bool IsSystemMessage => true;

		public override void Build(BinaryReader _br, Message _message)
		{
			_br.ReadInt32();
			_br.ReadInt32();
			result = _br.ReadByte();
		}
	}
}
