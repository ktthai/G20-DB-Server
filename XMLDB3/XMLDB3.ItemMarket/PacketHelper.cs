using System;
using System.IO;
using System.Net;
using System.Text;

namespace XMLDB3.ItemMarket
{
	public class PacketHelper
	{
		private static Encoding strEncoder;

		public static void Init(int codePage)
		{
			strEncoder = Encoding.GetEncoding(codePage);
		}

		public static void WriteStringPacket(BinaryWriter _bw, string _text)
		{
			byte[] bytes = strEncoder.GetBytes(_text);
			_bw.Write(IPAddress.HostToNetworkOrder((short)bytes.Length));
			_bw.Write(bytes);
		}

		public static string ReadStringPacket(BinaryReader _br)
		{
			ushort count = (ushort)IPAddress.NetworkToHostOrder(_br.ReadInt16());
			byte[] bytes = _br.ReadBytes(count);
			return strEncoder.GetString(bytes, 0, count);
		}

		public static void WriteItemPacket(BinaryWriter _bw, Item _item, int _serverNo)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			WriteStringPacket(_bw, _item.data);
			WriteStringPacket(_bw, $"{_serverNo}:{_item.id}");
			int num = 0;
			binaryWriter.Write(IPAddress.HostToNetworkOrder(_item.id));
			num++;
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder(_item.Class));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder(_item.color_01));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder(_item.color_02));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder(_item.color_03));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder(_item.price));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder(_item.sellingprice));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder((int)_item.bundle));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder(_item.figure));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder((int)_item.flag));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder(_item.durability));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder(_item.durability_max));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder(_item.origin_durability_max));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder((int)_item.attack_min));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder((int)_item.attack_max));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder((int)_item.wattack_min));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder((int)_item.wattack_max));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder((int)_item.balance));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder((int)_item.critical));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder(_item.defence));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder((int)_item.protect));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder((int)_item.effective_range));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder((int)_item.attack_speed));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder((int)_item.down_hit_count));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder((int)_item.experience));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder((int)_item.exp_point));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder((int)_item.upgraded));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder((int)_item.upgrade_max));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder((int)_item.grade));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder((int)_item.prefix));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder((int)_item.suffix));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder(_item.expiration));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder(_item.varint));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder((int)_item.storedtype));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder(_item.options.Length));
			num++;
			binaryWriter.Write(IPAddress.HostToNetworkOrder(9));
			num++;
			ItemOption[] options = _item.options;
			foreach (ItemOption itemOption in options)
			{
				binaryWriter.Write(IPAddress.HostToNetworkOrder((int)itemOption.type));
				num++;
				binaryWriter.Write(IPAddress.HostToNetworkOrder(itemOption.flag));
				num++;
				binaryWriter.Write(IPAddress.HostToNetworkOrder((int)itemOption.execute));
				num++;
				binaryWriter.Write(IPAddress.HostToNetworkOrder(itemOption.execdata));
				num++;
				num++;
				binaryWriter.Write(IPAddress.HostToNetworkOrder((int)itemOption.open));
				num++;
				binaryWriter.Write(IPAddress.HostToNetworkOrder(itemOption.opendata));
				num++;
				binaryWriter.Write(IPAddress.HostToNetworkOrder((int)itemOption.enable));
				num++;
				binaryWriter.Write(IPAddress.HostToNetworkOrder(itemOption.enabledata));
				num++;
			}
			_bw.Write(IPAddress.HostToNetworkOrder(num));
			_bw.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Position);
		}

		public static Item ReadItemPacket(BinaryReader _br, bool _bReadSecurityInfo)
		{
			string _itemSecurityInfo = string.Empty;
			return ReadItemPacket(_br, _bReadSecurityInfo, out _itemSecurityInfo);
		}

		public static Item ReadItemPacket(BinaryReader _br, bool _bReadSecurityInfo, out string _itemSecurityInfo)
		{
			_itemSecurityInfo = string.Empty;
			try
			{
				Item item = new Item();
				item.data = ReadStringPacket(_br);
				if (_bReadSecurityInfo)
				{
					_itemSecurityInfo = ReadStringPacket(_br);
				}
				int num = IPAddress.NetworkToHostOrder(_br.ReadInt32());
				item.id = IPAddress.HostToNetworkOrder(_br.ReadInt64());
				num--;
				num--;
				item.Class = IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.color_01 = IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.color_02 = IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.color_03 = IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.price = IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.sellingprice = IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.bundle = (short)IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.figure = IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.flag = (byte)IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.durability = IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.durability_max = IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.origin_durability_max = IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.attack_min = (short)IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.attack_max = (short)IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.wattack_min = (short)IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.wattack_max = (short)IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.balance = (byte)IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.critical = (byte)IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.defence = IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.protect = (short)IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.effective_range = (short)IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.attack_speed = (byte)IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.down_hit_count = (byte)IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.experience = (short)IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.exp_point = (byte)IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.upgraded = (byte)IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.upgrade_max = (byte)IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.grade = (byte)IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.prefix = (short)IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.suffix = (short)IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.expiration = IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.varint = IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				item.storedtype = (byte)IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				int num2 = IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				int num3 = IPAddress.NetworkToHostOrder(_br.ReadInt32());
				num--;
				switch (num3)
				{
				case 8:
					if (num2 > 0)
					{
						item.options = new ItemOption[num2];
						for (int j = 0; j < num2; j++)
						{
							item.options[j] = new ItemOption();
							item.options[j].type = (byte)IPAddress.NetworkToHostOrder(_br.ReadInt32());
							num--;
							item.options[j].flag = IPAddress.NetworkToHostOrder(_br.ReadInt32());
							num--;
							item.options[j].execute = (short)IPAddress.NetworkToHostOrder(_br.ReadInt32());
							num--;
							item.options[j].execdata = IPAddress.NetworkToHostOrder(_br.ReadInt32());
							num--;
							item.options[j].open = (byte)IPAddress.NetworkToHostOrder(_br.ReadInt32());
							num--;
							item.options[j].opendata = IPAddress.NetworkToHostOrder(_br.ReadInt32());
							num--;
							item.options[j].enable = (byte)IPAddress.NetworkToHostOrder(_br.ReadInt32());
							num--;
							item.options[j].enabledata = IPAddress.NetworkToHostOrder(_br.ReadInt32());
							num--;
						}
					}
					break;
				case 9:
					if (num2 > 0)
					{
						item.options = new ItemOption[num2];
						for (int i = 0; i < num2; i++)
						{
							item.options[i] = new ItemOption();
							item.options[i].type = (byte)IPAddress.NetworkToHostOrder(_br.ReadInt32());
							num--;
							item.options[i].flag = IPAddress.NetworkToHostOrder(_br.ReadInt32());
							num--;
							item.options[i].execute = (short)IPAddress.NetworkToHostOrder(_br.ReadInt32());
							num--;
							item.options[i].execdata = IPAddress.NetworkToHostOrder(_br.ReadInt64());
							num--;
							num--;
							item.options[i].open = (byte)IPAddress.NetworkToHostOrder(_br.ReadInt32());
							num--;
							item.options[i].opendata = IPAddress.NetworkToHostOrder(_br.ReadInt32());
							num--;
							item.options[i].enable = (byte)IPAddress.NetworkToHostOrder(_br.ReadInt32());
							num--;
							item.options[i].enabledata = IPAddress.NetworkToHostOrder(_br.ReadInt32());
							num--;
						}
					}
					break;
				}
				if (num != 0)
				{
					throw new Exception("Invalid Item Packet");
				}
				return item;
			}
			catch (Exception ex)
			{
				ExceptionMonitor.ExceptionRaised(ex);
				return null;
			}
		}
	}
}
