using System;

namespace Mabinogi.Network
{
	public class XOREncription
	{
		private uint Seed;

		private uint Size;

		private byte[] XORMap;

		private uint _Hash(uint _Seed)
		{
			uint num = _Seed;
			uint num2 = _Seed;
			for (int i = 0; i < 4; i++)
			{
				char c = (char)(num2 & 0xFF);
				num2 >>= 8;
				num = (num << 4) + c;
				uint num3 = (uint)((int)num & -268435456);
				if (num3 != 0)
				{
					num ^= num3 >> 24;
				}
				num &= ~num3;
				num ^= 3281429949u >> i;
			}
			return num;
		}

		public XOREncription(uint _Size, uint _Seed)
		{
			Size = _Size;
			Seed = _Seed;
			XORMap = new byte[Size];
			uint num = _Hash(_Seed);
			for (int i = 0; i < _Size; i++)
			{
				XORMap[i] = (byte)(num & 0xFF);
				num = _Hash(num);
			}
		}

		public void Encription(ref byte[] _Buffer, int _Offset, int _Size)
		{
			throw new Exception("[XOREncription::Encription()] can't use encript option");
		}

		public void Decription(ref byte[] _Buffer, int _Offset, int _Size)
		{
			throw new Exception("[XOREncription::Decription()] can't use encript option");
		}

		public uint GetSeed()
		{
			return Seed;
		}

		public uint GetSize()
		{
			return Size;
		}
	}
}
