using System;
using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class ItemMarketCommand
	{
		protected byte[] packetBuffer;

		protected MemoryStream ms;

		public byte[] Packet => packetBuffer;

		public void BuildPacket(int _packetNo)
		{
			BuildPacket(_packetNo, ms.GetBuffer(), (int)ms.Position);
		}

		protected void BuildPacket(int _packetNo, byte[] _data, int _length)
		{
			byte value = 160;
			packetBuffer = new byte[_length + 9];
			MemoryStream output = new MemoryStream(packetBuffer);
			BinaryWriter binaryWriter = new BinaryWriter(output);
			try
			{
				binaryWriter.Write(value);
				binaryWriter.Write(IPAddress.HostToNetworkOrder(_length + 4));
				binaryWriter.Write(IPAddress.HostToNetworkOrder(_packetNo));
				binaryWriter.Write(_data, 0, _length);
			}
			catch (Exception ex)
			{
				ExceptionMonitor.ExceptionRaised(ex);
			}
			finally
			{
				binaryWriter.Close();
			}
		}
	}
}
