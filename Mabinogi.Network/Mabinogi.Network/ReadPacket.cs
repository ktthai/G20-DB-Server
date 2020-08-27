using System;
using System.IO;

namespace Mabinogi.Network
{
	public class ReadPacket : Packet
	{
		protected BinaryReader ReadHelper;

		protected int iPacketTotalSize;

		public int PacketTotalSize => iPacketTotalSize;

		public BinaryReader Binary => ReadHelper;

		public ReadPacket()
		{
			iPacketTotalSize = 0;
		}

		private static bool IsEnableReadBytesFromStream(Stream _stream, int _readbyte)
		{
			return _stream.Length >= _stream.Position + _readbyte;
		}

		public static ReadPacket Build(byte[] _Buffer, int _Offset, int _PacketSize, XOREncription _Encription)
		{
			try
			{
				MemoryStream memoryStream = new MemoryStream(_Buffer, _Offset, _PacketSize);
				BinaryReader binaryReader = new BinaryReader(memoryStream);
				ReadPacket readPacket = new ReadPacket();
				if (!IsEnableReadBytesFromStream(memoryStream, 3))
				{
					return null;
				}
				readPacket.Header = binaryReader.ReadByte();
				if (readPacket.Header != 85)
				{
					throw new Exception("패킷헤더가 잘못되었습니다");
				}
				readPacket.Count = binaryReader.ReadByte();
				byte b = binaryReader.ReadByte();
				int num = 0;
				int num2 = 0;
				bool flag = false;
				while (!flag)
				{
					if (!IsEnableReadBytesFromStream(memoryStream, 1))
					{
						return null;
					}
					byte b2 = binaryReader.ReadByte();
					num2++;
					if ((b2 & 0x80) != 0)
					{
						b2 = (byte)(b2 & 0x7F);
					}
					else
					{
						flag = true;
					}
					num += b2 << (num2 - 1) * 7;
				}
				readPacket.Length = num;
				if (memoryStream.Length - memoryStream.Position < num)
				{
					return null;
				}
				if (!IsEnableReadBytesFromStream(memoryStream, num))
				{
					return null;
				}
				byte[] _Buffer2 = binaryReader.ReadBytes(num);
				if (_Buffer2.Length != num)
				{
					throw new Exception("스트림에서 읽은 바이트가 요청한 바이트와 다릅니다.");
				}
				if ((b & 1) != 0)
				{
					readPacket.IsEncription = true;
				}
				if ((b & 2) != 0)
				{
					readPacket.IsSystemMsg = true;
				}
				if ((b & 4) != 0)
				{
					readPacket.IsCompressed = true;
				}
				if ((b & 8) != 0)
				{
					readPacket.IsUsingChecksum = true;
				}
				if (readPacket.IsEncription)
				{
					if (_Encription == null)
					{
						throw new Exception("인크립션 루틴이 제공되지 않았습니다.");
					}
					_Encription.Decription(ref _Buffer2, 0, readPacket.Length);
				}
				if (readPacket.IsCompressed)
				{
					throw new Exception("압축패킷은 사용할 수 없습니다");
				}
				if (readPacket.IsUsingChecksum)
				{
					if (num < 4)
					{
						throw new Exception("체크섬을 사용하기에는 패킷이 너무 작습니다");
					}
					MemoryStream input = new MemoryStream(_Buffer2);
					BinaryReader binaryReader2 = new BinaryReader(input);
					uint num3 = (uint)binaryReader2.ReadInt32();
					byte[] array = binaryReader2.ReadBytes(num - 4);
					CheckSumCRC32 checkSumCRC = new CheckSumCRC32();
					checkSumCRC.Add(array, num - 4);
					if (num3 != checkSumCRC.GetValue())
					{
						throw new Exception("체크섬이 맞지 않습니다");
					}
					_Buffer2 = array;
					num -= 4;
					if (_Buffer2.Length != num)
					{
						throw new Exception("체크섬 매치에 실패하였습니다. 데이터가 손상되었습니다.");
					}
				}
				readPacket.SetData(_Buffer2);
				readPacket.iPacketTotalSize = (int)binaryReader.BaseStream.Position;
				return readPacket;
			}
			catch (EndOfStreamException)
			{
				return null;
			}
		}

		protected void SetData(byte[] _Data)
		{
			MemoryStream input = new MemoryStream(_Data);
			ReadHelper = new BinaryReader(input);
			Data = _Data;
			Length = _Data.Length;
		}

		public Message ToMessage()
		{
			return new Message(Data, Length);
		}
	}
}
