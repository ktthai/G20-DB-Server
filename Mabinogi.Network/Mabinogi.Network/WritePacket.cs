using System;
using System.IO;

namespace Mabinogi.Network
{
	public class WritePacket : Packet
	{
		public static WritePacket Build(byte[] _Buffer, byte _WriteCount, bool _bEncription, bool _bSystemMsg, bool _bUsingChecksum, XOREncription _Encript)
		{
			WritePacket writePacket = new WritePacket();
			writePacket.Header = 85;
			writePacket.Count = _WriteCount;
			writePacket.IsEncription = _bEncription;
			writePacket.IsSystemMsg = _bSystemMsg;
			writePacket.IsUsingChecksum = _bUsingChecksum;
			if (_bEncription)
			{
				_Encript.Encription(ref _Buffer, 0, _Buffer.Length);
			}
			writePacket.Data = _Buffer;
			writePacket.Length = _Buffer.Length;
			return writePacket;
		}

		public static WritePacket Build(Message _Msg, byte _WriteCount, bool _bEncription, bool _bSystemMsg, bool _bUsingChecksum, XOREncription _Encript)
		{
			return Build(_Msg.ToBuffer(), _WriteCount, _bEncription, _bSystemMsg, _bUsingChecksum, _Encript);
		}

		public static WritePacket Build(byte[] _Buffer, int _Length, byte _WriteCount, bool _bEncription, bool _bSystemMsg, bool _bUsingChecksum, XOREncription _Encript)
		{
			byte[] array = new byte[_Length];
			Array.Copy(_Buffer, 0, array, 0, _Length);
			return Build(array, _WriteCount, _bEncription, _bSystemMsg, _bUsingChecksum, _Encript);
		}

		public byte[] ToBuffer()
		{
			uint value = 0u;
			if (IsUsingChecksum)
			{
				CheckSumCRC32 checkSumCRC = new CheckSumCRC32();
				checkSumCRC.Add(Data, Data.Length);
				value = checkSumCRC.GetValue();
				Length += 4;
			}
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(Header);
			binaryWriter.Write(Count);
			byte b = 0;
			if (IsEncription)
			{
				b = (byte)(b | 1);
			}
			if (IsSystemMsg)
			{
				b = (byte)(b | 2);
			}
			if (IsCompressed)
			{
				b = (byte)(b | 4);
			}
			if (IsUsingChecksum)
			{
				b = (byte)(b | 8);
			}
			binaryWriter.Write(b);
			while (Length > 0)
			{
				byte b2 = (byte)(Length & 0x7F);
				if (Length > 127)
				{
					b2 = (byte)(b2 | 0x80);
				}
				binaryWriter.Write(b2);
				Length >>= 7;
			}
			if (IsUsingChecksum)
			{
				binaryWriter.Write(value);
			}
			binaryWriter.Write(Data);
			int num = (int)memoryStream.Position;
			byte[] array = new byte[num];
			Array.Copy(memoryStream.GetBuffer(), 0, array, 0, num);
			return array;
		}
	}
}
