using Mabinogi;
using System.Threading;

namespace XMLDB3
{
	public class CacheStatistics
	{
		private string name = string.Empty;

		private int total;

		private int hit;

		private int size;

		public string Name => name;

		public int Total => total;

		public int Hit => hit;

		public int Size
		{
			get
			{
				return size;
			}
			set
			{
				size = value;
			}
		}

		public CacheStatistics(string _name)
		{
			name = _name;
		}

		public CacheStatistics(string _name, int _size)
		{
			name = _name;
			size = _size;
		}

		public void CacheMiss()
		{
			Interlocked.Increment(ref total);
		}

		public void CacheHit()
		{
			Interlocked.Increment(ref total);
			Interlocked.Increment(ref hit);
		}

		public void InitHitCount()
		{
			Interlocked.Exchange(ref total, 0);
			Interlocked.Exchange(ref hit, 0);
		}

		public Message ToMessage()
		{
			Message message = new Message(0u, 0uL);
			message.WriteString(name);
			message.WriteS32(size);
			message.WriteS32(total);
			message.WriteS32(hit);
			return message;
		}

		public Message FromMessage(Message _input)
		{
			name = _input.ReadString();
			size = _input.ReadS32();
			total = _input.ReadS32();
			hit = _input.ReadS32();
			return _input;
		}
	}
}
