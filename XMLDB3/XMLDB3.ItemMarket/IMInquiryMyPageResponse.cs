using Mabinogi;
using System;
using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class IMInquiryMyPageResponse : ItemMarketResponse
	{
		public override void Build(BinaryReader _br, Message _message)
		{
			result = IPAddress.NetworkToHostOrder(_br.ReadInt32());
			if (result != 1)
			{
				_message.WriteU8(51);
				_message.WriteS32(result);
				return;
			}
			_message.WriteU8(1);
			_message.WriteS32(IPAddress.NetworkToHostOrder(_br.ReadInt32()));
			int num = IPAddress.NetworkToHostOrder(_br.ReadInt32());
			_message.WriteS32(num);
			for (int i = 0; i < num; i++)
			{
				_br.ReadInt32();
				_message.WriteS64(IPAddress.NetworkToHostOrder(_br.ReadInt64()));
				long num2 = _br.ReadInt64();
				_message.WriteS64(IPAddress.NetworkToHostOrder(num2));
				_br.ReadInt32();
				PacketHelper.ReadStringPacket(_br);
				_br.ReadInt16();
				_message.WriteS32(IPAddress.NetworkToHostOrder(_br.ReadInt32()));
				_message.WriteS32(IPAddress.NetworkToHostOrder(_br.ReadInt32()));
				_message.WriteS64(IPAddress.NetworkToHostOrder(_br.ReadInt64()));
				_message.WriteS64(IPAddress.NetworkToHostOrder(_br.ReadInt64()));
				_message.WriteS64(IPAddress.NetworkToHostOrder(_br.ReadInt64()));
				PacketHelper.ReadStringPacket(_br);
				_br.ReadInt32();
				PacketHelper.ReadStringPacket(_br);
				_message.WriteString(PacketHelper.ReadStringPacket(_br));
				Item item = PacketHelper.ReadItemPacket(_br, _bReadSecurityInfo: false);
				if (item != null)
				{
					_message.WriteU8(1);
					ItemSerializer.Deserialize(item, _message);
				}
				else
				{
					ExceptionMonitor.ExceptionRaised(new Exception("Invalid Item String"), num2);
					_message.WriteU8(0);
				}
			}
		}
	}
}
