using System.IO;

namespace Mabinogi.Network
{
	public class Packet
	{
		protected byte Header;

		protected byte Count;

		protected bool IsEncription;

		protected bool IsSystemMsg;

		protected bool IsCompressed;

		protected bool IsUsingChecksum;

		protected int Length;

		protected byte[] Data;

		public Packet()
		{
			Header = 0;
			Count = 0;
			Length = 0;
			IsEncription = false;
			IsSystemMsg = false;
			IsCompressed = false;
			IsUsingChecksum = false;
			Data = null;
		}

		public bool IsSystemPacket()
		{
			return IsSystemMsg;
		}

		public int GetLength()
		{
			return Length;
		}

		public static int GetPacketLength(byte[] _Buffer, int _Offset, int _Size, ref int _OutPacketLength)
		{
			int num = _Size - _Offset;
			MemoryStream input = new MemoryStream(_Buffer, _Offset, num);
			BinaryReader binaryReader = new BinaryReader(input);
			_OutPacketLength = 0;
			int num2 = 0;
			bool flag = false;
			while (num > num2 && !flag)
			{
				byte b = binaryReader.ReadByte();
				num2++;
				if ((b & 0x80) != 0)
				{
					b = (byte)(b & 0x7F);
				}
				else
				{
					flag = true;
				}
				_OutPacketLength += b << (num2 - 1) * 7;
			}
			if (!flag)
			{
				return 0;
			}
			return num2;
		}
	}
}
