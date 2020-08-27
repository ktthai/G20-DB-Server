using System.IO;

namespace Mabinogi.Network
{
	public class ByteEncoding
	{
		public static int BinaryToInt(BinaryReader reader)
		{
			int num = 0;
			int num2 = 0;
			bool flag = false;
			while (!flag)
			{
				byte b = reader.ReadByte();
				num2++;
				if ((b & 0x80) != 0)
				{
					b = (byte)(b & 0x7F);
				}
				else
				{
					flag = true;
				}
				num += b << (num2 - 1) * 7;
			}
			return num;
		}

		public static void IntToBinary(int _num_raw, BinaryWriter _writer)
		{
			uint num = (uint)_num_raw;
			if (num == 0)
			{
				_writer.Write((byte)0);
				return;
			}
			while (num != 0)
			{
				byte b = (byte)(num & 0x7F);
				if (num > 127)
				{
					b = (byte)(b | 0x80);
				}
				_writer.Write(b);
				num >>= 7;
			}
		}
	}
}
