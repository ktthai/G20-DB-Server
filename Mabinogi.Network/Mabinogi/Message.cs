using Mabinogi.Network;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;

namespace Mabinogi
{
	public class Message
	{
		public enum MessageFieldType
		{
			VT_NONE,
			VT_BYTE,
			VT_WORD,
			VT_DWORD,
			VT_QWORD,
			VT_FLOAT,
			VT_STRING,
			VT_BINARY,
			VT_LONGBINARY
		}

		private struct MessageToken
		{
			public MessageFieldType Type;

			public byte dataU8;

			public ushort dataU16;

			public uint dataU32;

			public ulong dataU64;

			public float dataFloat;

			public string dataString;

			public byte[] dataBinary;
		}

		protected uint MessageID;

		protected ulong TargetObject;

		protected int Length;

		protected int FieldNumber;

		protected byte ByteOrderType;

		protected ArrayList Datas;

		protected int ReadingOffset;

		public bool Log;

		public uint ID
		{
			get
			{
				return MessageID;
			}
			set
			{
				MessageID = value;
			}
		}

		public ulong Target
		{
			get
			{
				return TargetObject;
			}
			set
			{
				TargetObject = value;
			}
		}

		public int CurrentReadingOffset
		{
			get
			{
				return ReadingOffset;
			}
			set
			{
				ReadingOffset = value;
			}
		}

		public MessageFieldType CurrentFieldType => ((MessageToken)Datas[ReadingOffset]).Type;

		public Message(byte[] _Buffer, int _Size)
			: this(_Buffer, 0, _Size)
        {
		}


		public Message(byte[] _Buffer, int index, int _Size)
		{
			Datas = new ArrayList();
			MemoryStream input = new MemoryStream(_Buffer, index, _Size);
			BinaryReader binaryReader = new BinaryReader(input);
			MessageID = (uint)IPAddress.NetworkToHostOrder((int)binaryReader.ReadUInt32());
			TargetObject = (ulong)IPAddress.NetworkToHostOrder((long)binaryReader.ReadUInt64());
			Length = ByteEncoding.BinaryToInt(binaryReader);
			FieldNumber = ByteEncoding.BinaryToInt(binaryReader);
			ByteOrderType = binaryReader.ReadByte();
			ReadingOffset = 0;
			if (Length + binaryReader.BaseStream.Position != _Size)
			{
				throw new Exception("메시지 길이가 이상합니다");
			}
			BufferToData(binaryReader.ReadBytes(Length), FieldNumber);
		}

		public Message()
			: this(0u, 0uL)
		{
		}

		public Message(uint _MessageID)
			: this(_MessageID, 0uL)
		{
		}

		public override string ToString()
		{
			return $"{base.ToString()}=ID:{MessageID};L:{Length};T:{Target};";
		}

		public Message(uint _MessageID, ulong _Target)
		{
			Datas = new ArrayList();
			MessageID = _MessageID;
			TargetObject = _Target;
			ByteOrderType = 0;
		}

		public byte[] ToBuffer()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			byte[] array = DataToBuffer();
			Length = array.Length;
			FieldNumber = Datas.Count;
			binaryWriter.Write((uint)IPAddress.HostToNetworkOrder((int)MessageID));
			binaryWriter.Write((ulong)IPAddress.HostToNetworkOrder((long)TargetObject));
			ByteEncoding.IntToBinary(Length, binaryWriter);
			ByteEncoding.IntToBinary(FieldNumber, binaryWriter);
			binaryWriter.Write(ByteOrderType);
			binaryWriter.Write(array);
			byte[] array2 = new byte[memoryStream.Position];
			Array.Copy(memoryStream.GetBuffer(), 0, array2, 0, array2.Length);
			return array2;
		}

		protected byte[] DataToBuffer()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			Encoding uTF = Encoding.UTF8;
			foreach (MessageToken data in Datas)
			{
				binaryWriter.Write((byte)data.Type);
				switch (data.Type)
				{
				case MessageFieldType.VT_LONGBINARY:
				{
					byte[] dataBinary2 = data.dataBinary;
					binaryWriter.Write(IPAddress.HostToNetworkOrder(dataBinary2.GetLength(0)));
					binaryWriter.Write(dataBinary2);
					break;
				}
				case MessageFieldType.VT_BINARY:
				{
					byte[] dataBinary = data.dataBinary;
					binaryWriter.Write((ushort)IPAddress.HostToNetworkOrder((short)dataBinary.GetLength(0)));
					binaryWriter.Write(dataBinary);
					break;
				}
				case MessageFieldType.VT_STRING:
				{
					byte[] array = HostStringToNetwork(uTF.GetBytes(data.dataString));
					if (array == null)
					{
						throw new Exception("메시지 생성 과정에서 문자열을 버퍼로 변환하지 못하였습니다\n" + data.dataString);
					}
					binaryWriter.Write((ushort)IPAddress.HostToNetworkOrder((short)array.GetLength(0)));
					binaryWriter.Write(array);
					break;
				}
				case MessageFieldType.VT_QWORD:
					binaryWriter.Write((ulong)IPAddress.HostToNetworkOrder((long)data.dataU64));
					break;
				case MessageFieldType.VT_DWORD:
					binaryWriter.Write((uint)IPAddress.HostToNetworkOrder((int)data.dataU32));
					break;
				case MessageFieldType.VT_WORD:
					binaryWriter.Write((ushort)IPAddress.HostToNetworkOrder((short)data.dataU16));
					break;
				case MessageFieldType.VT_BYTE:
					binaryWriter.Write(data.dataU8);
					break;
				case MessageFieldType.VT_FLOAT:
					binaryWriter.Write(data.dataFloat);
					break;
				default:
					throw new Exception("invalid message field : DataToBuffer() \ntype number is " + (int)data.Type);
				}
			}
			byte[] array2 = new byte[memoryStream.Position];
			Array.Copy(memoryStream.GetBuffer(), 0, array2, 0, array2.GetLength(0));
			return array2;
		}

		protected void BufferToData(byte[] _Buffer, int _DataCount)
		{
			MemoryStream input = new MemoryStream(_Buffer);
			BinaryReader binaryReader = new BinaryReader(input);
			Encoding uTF = Encoding.UTF8;
			for (int i = 0; i < _DataCount; i++)
			{
				byte b = binaryReader.ReadByte();
				switch (b)
				{
				case 1:
				{
					byte b2 = 0;
					b2 = binaryReader.ReadByte();
					WriteU8(b2);
					break;
				}
				case 2:
				{
					ushort num3 = 0;
					num3 = (ushort)IPAddress.NetworkToHostOrder((short)binaryReader.ReadUInt16());
					WriteU16(num3);
					break;
				}
				case 3:
				{
					uint num2 = 0u;
					num2 = (uint)IPAddress.NetworkToHostOrder((int)binaryReader.ReadUInt32());
					WriteU32(num2);
					break;
				}
				case 4:
				{
					ulong num = 0uL;
					num = (ulong)IPAddress.NetworkToHostOrder((long)binaryReader.ReadUInt64());
					WriteU64(num);
					break;
				}
				case 5:
					WriteFloat(binaryReader.ReadSingle());
					break;
				case 6:
				{
					ushort count3 = (ushort)IPAddress.NetworkToHostOrder((short)binaryReader.ReadUInt16());
					WriteString(NetworkStringToHost(uTF.GetString(binaryReader.ReadBytes(count3))));
					break;
				}
				case 7:
				{
					ushort count2 = (ushort)IPAddress.NetworkToHostOrder((short)binaryReader.ReadUInt16());
					WriteBinary(binaryReader.ReadBytes(count2));
					break;
				}
				case 8:
				{
					int count = IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
					WriteLongBinary(binaryReader.ReadBytes(count));
					break;
				}
				default:
					throw new Exception("invalid message field : DataToBuffer() \ntype number is " + (int)b);
				}
			}
		}

		public void WriteBinary(byte[] _Data)
		{
			MessageToken messageToken = default(MessageToken);
			messageToken.Type = MessageFieldType.VT_BINARY;
			messageToken.dataBinary = (byte[])_Data.Clone();
			Datas.Add(messageToken);
		}

		public void WriteLongBinary(byte[] _Data)
		{
			MessageToken messageToken = default(MessageToken);
			messageToken.Type = MessageFieldType.VT_LONGBINARY;
			messageToken.dataBinary = (byte[])_Data.Clone();
			Datas.Add(messageToken);
		}

		public void WriteString(string _Data)
		{
			MessageToken messageToken = default(MessageToken);
			messageToken.Type = MessageFieldType.VT_STRING;
			messageToken.dataString = _Data;
			Datas.Add(messageToken);
		}

		public void WriteU64(ulong _Data)
		{
			MessageToken messageToken = default(MessageToken);
			messageToken.Type = MessageFieldType.VT_QWORD;
			messageToken.dataU64 = _Data;
			Datas.Add(messageToken);
		}

		public void WriteU32(uint _Data)
		{
			MessageToken messageToken = default(MessageToken);
			messageToken.Type = MessageFieldType.VT_DWORD;
			messageToken.dataU32 = _Data;
			Datas.Add(messageToken);
		}

		public void WriteU16(ushort _Data)
		{
			MessageToken messageToken = default(MessageToken);
			messageToken.Type = MessageFieldType.VT_WORD;
			messageToken.dataU16 = _Data;
			Datas.Add(messageToken);
		}

		public void WriteU8(byte _Data)
		{
			MessageToken messageToken = default(MessageToken);
			messageToken.Type = MessageFieldType.VT_BYTE;
			messageToken.dataU8 = _Data;
			Datas.Add(messageToken);
		}

		public void WriteS64(long _Data)
		{
			WriteU64((ulong)_Data);
		}

		public void WriteS32(int _Data)
		{
			WriteU32((uint)_Data);
		}

		public void WriteS16(short _Data)
		{
			WriteU16((ushort)_Data);
		}

		public void WriteS8(sbyte _Data)
		{
			WriteU8((byte)_Data);
		}

		public void WriteFloat(float _Data)
		{
			MessageToken messageToken = default(MessageToken);
			messageToken.Type = MessageFieldType.VT_FLOAT;
			messageToken.dataFloat = _Data;
			Datas.Add(messageToken);
		}

		public void WriteTypeOf(dynamic _var)
		{
			Type left = _var.GetType();
			if (left == typeof(sbyte))
			{
				this.WriteS8(_var);
				return;
			}
			if (left == typeof(short))
			{
				this.WriteS16(_var);
				return;
			}
			if (left == typeof(int))
			{
				this.WriteS32(_var);
				return;
			}
			if (left == typeof(long))
			{
				this.WriteS64(_var);
				return;
			}
			if (left == typeof(byte))
			{
				this.WriteU8(_var);
				return;
			}
			if (left == typeof(ushort))
			{
				this.WriteU16(_var);
				return;
			}
			if (left == typeof(uint))
			{
				this.WriteU32(_var);
				return;
			}
			if (left == typeof(ulong))
			{
				this.WriteU64(_var);
				return;
			}
			if (left == typeof(float))
			{
				this.WriteFloat(_var);
				return;
			}
			if (!(left == typeof(string)))
			{
				throw new Exception("Type Inference Failed");
			}
			this.WriteString(_var);
		}

		public byte[] ReadBinary()
		{
			if (Datas.Count > ReadingOffset)
			{
				MessageToken messageToken = (MessageToken)Datas[ReadingOffset];
				if (messageToken.Type == MessageFieldType.VT_BINARY)
				{
					ReadingOffset++;
					return messageToken.dataBinary;
				}
				throw new Exception("invalid type : ReadBinary()\ntype is " + (int)messageToken.Type);
			}
			throw new Exception("offset overflow : ReadBinary()");
		}

		public byte[] ReadLongBinary()
		{
			if (Datas.Count > ReadingOffset)
			{
				MessageToken messageToken = (MessageToken)Datas[ReadingOffset];
				if (messageToken.Type == MessageFieldType.VT_LONGBINARY)
				{
					ReadingOffset++;
					return messageToken.dataBinary;
				}
				throw new Exception("invalid type : ReadBinary()\ntype is " + (int)messageToken.Type);
			}
			throw new Exception("offset overflow : ReadBinary()");
		}

		public string ReadString()
		{
			if (Datas.Count > ReadingOffset)
			{
				MessageToken messageToken = (MessageToken)Datas[ReadingOffset];
				if (messageToken.Type == MessageFieldType.VT_STRING)
				{
					ReadingOffset++;
					return messageToken.dataString;
				}
				throw new Exception("invalid type : ReadString()\ntype is " + (int)messageToken.Type);
			}
			throw new Exception("offset overflow : ReadString()");
		}

		public ulong ReadU64()
		{
			if (Datas.Count > ReadingOffset)
			{
				MessageToken messageToken = (MessageToken)Datas[ReadingOffset];
				if (messageToken.Type == MessageFieldType.VT_QWORD)
				{
					ReadingOffset++;
					return messageToken.dataU64;
				}
				throw new Exception("invalid type : ReadU64()\ntype is " + (int)messageToken.Type);
			}
			throw new Exception("offset overflow : ReadU64()");
		}

		public uint ReadU32()
		{
			if (Datas.Count > ReadingOffset)
			{
				MessageToken messageToken = (MessageToken)Datas[ReadingOffset];
				if (messageToken.Type == MessageFieldType.VT_DWORD)
				{
					ReadingOffset++;
					return messageToken.dataU32;
				}
				throw new Exception("invalid type : ReadU32()\ntype is " + (int)messageToken.Type);
			}
			throw new Exception("offset overflow : ReadU32()");
		}

		public ushort ReadU16()
		{
			if (Datas.Count > ReadingOffset)
			{
				MessageToken messageToken = (MessageToken)Datas[ReadingOffset];
				if (messageToken.Type == MessageFieldType.VT_WORD)
				{
					ReadingOffset++;
					return messageToken.dataU16;
				}
				throw new Exception("invalid type : ReadU16()\ntype is " + (int)messageToken.Type);
			}
			throw new Exception("offset overflow : ReadU16()");
		}

		public byte ReadU8()
		{
			if (Datas.Count > ReadingOffset)
			{
				MessageToken messageToken = (MessageToken)Datas[ReadingOffset];
				if (messageToken.Type == MessageFieldType.VT_BYTE)
				{
					ReadingOffset++;
					return messageToken.dataU8;
				}
				throw new Exception("invalid type : ReadU8()\ntype is " + (int)messageToken.Type);
			}
			throw new Exception("offset overflow : ReadU8()");
		}

		public long ReadS64()
		{
			return (long)ReadU64();
		}

		public int ReadS32()
		{
			return (int)ReadU32();
		}

		public short ReadS16()
		{
			return (short)ReadU16();
		}

		public sbyte ReadS8()
		{
			return (sbyte)ReadU8();
		}

		public float ReadFloat()
		{
			if (Datas.Count > ReadingOffset)
			{
				MessageToken messageToken = (MessageToken)Datas[ReadingOffset];
				if (messageToken.Type == MessageFieldType.VT_FLOAT)
				{
					ReadingOffset++;
					return messageToken.dataFloat;
				}
				throw new Exception("invalid type : ReadU8()\ntype is " + (int)messageToken.Type);
			}
			throw new Exception("offset overflow : ReadFloat()");
		}

		public dynamic ReadTypeOf<T>(T _var)
		{
			Type typeFromHandle = typeof(T);
			if (typeFromHandle == typeof(sbyte))
			{
				return ReadS8();
			}
			if (typeFromHandle == typeof(short))
			{
				return ReadS16();
			}
			if (typeFromHandle == typeof(int))
			{
				return ReadS32();
			}
			if (typeFromHandle == typeof(long))
			{
				return ReadS64();
			}
			if (typeFromHandle == typeof(byte))
			{
				return ReadU8();
			}
			if (typeFromHandle == typeof(ushort))
			{
				return ReadU16();
			}
			if (typeFromHandle == typeof(uint))
			{
				return ReadU32();
			}
			if (typeFromHandle == typeof(ulong))
			{
				return ReadU64();
			}
			if (typeFromHandle == typeof(float))
			{
				return ReadFloat();
			}
			if (typeFromHandle == typeof(string))
			{
				return ReadString();
			}
			throw new Exception("Type Inference Failed");
		}

		protected static string NetworkStringToHost(string _Input)
		{
			string text = "";
			char[] array = _Input.ToCharArray();
			for (int i = 0; i < array.Length && array[i] != 0; i++)
			{
				text += array[i];
			}
			return text;
		}

		protected static byte[] HostStringToNetwork(byte[] _Input)
		{
			int length = _Input.GetLength(0);
			byte[] array = new byte[length + 1];
			bool flag = false;
			int i;
			for (i = 0; i < length; i++)
			{
				array[i] = _Input[i];
				if (_Input[i] == 0)
				{
					flag = true;
					i++;
					break;
				}
			}
			byte[] array2;
			if (!flag)
			{
				array[i] = 0;
				array2 = array;
			}
			else
			{
				array2 = new byte[i];
				Array.Copy(array, 0, array2, 0, i);
			}
			return array2;
		}

		public static Message operator +(Message _1, Message _2)
		{
			_1.Datas.AddRange(_2.Datas);
			return _1;
		}

		public Message Clone()
		{
			Message message = new Message(MessageID, TargetObject);
			message.MessageID = MessageID;
			message.TargetObject = TargetObject;
			message.Length = Length;
			message.FieldNumber = FieldNumber;
			message.ByteOrderType = ByteOrderType;
			message.ReadingOffset = ReadingOffset;
			message.Datas = (ArrayList)Datas.Clone();
			return message;
		}

		public bool IsNull()
		{
			return 0 == Datas.Count;
		}
	}
}
