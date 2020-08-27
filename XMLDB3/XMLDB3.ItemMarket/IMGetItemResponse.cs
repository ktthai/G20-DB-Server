using Mabinogi;
using System;
using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class IMGetItemResponse : ItemMarketResponse
	{
		public override void Build(BinaryReader _br, Message _message)
		{
			_br.ReadInt32();
			PacketHelper.ReadStringPacket(_br);
			result = IPAddress.NetworkToHostOrder(_br.ReadInt32());
			if (result == 1)
			{
				_message.WriteU8(1);
				_message.WriteS32(IPAddress.NetworkToHostOrder(_br.ReadInt32()));
				long num = _br.ReadInt64();
				_message.WriteS64(IPAddress.NetworkToHostOrder(num));
				_br.ReadInt32();
				PacketHelper.ReadStringPacket(_br);
				_br.ReadInt16();
				string _itemSecurityInfo = string.Empty;
				Item item = PacketHelper.ReadItemPacket(_br, _bReadSecurityInfo: true, out _itemSecurityInfo);
				if (item != null)
				{
					_message.WriteU8(1);
					string[] array = _itemSecurityInfo.Split(':');
					int num2 = Convert.ToInt32(array[0]);
					if (ConfigManager.ItemMarketServerNo != num2)
					{
						_message.WriteU8(1);
					}
					else
					{
						_message.WriteU8(0);
					}
					_message.WriteString(_itemSecurityInfo);
					ItemSerializer.Deserialize(item, _message);
				}
				else
				{
					ExceptionMonitor.ExceptionRaised(new Exception("Invalid Item String"), num);
					_message.WriteU8(0);
				}
			}
			else
			{
				_message.WriteU8(51);
				_message.WriteS32(result);
			}
		}
	}
}
