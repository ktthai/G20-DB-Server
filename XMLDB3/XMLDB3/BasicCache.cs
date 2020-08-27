using System.Collections;

namespace XMLDB3
{
	public class BasicCache
	{
		private const int DefaultTableInitSize = 50000;

		private int m_TableInitSize;

		private int m_ElevationCutline;

		private Hashtable m_Objects;

		private Hashtable m_ObjectsLv2;

		private bool m_AutoElevation = true;

		protected string m_Name;

		private CacheStatistics m_statistics;

		public bool AutoElevation
		{
			get
			{
				return m_AutoElevation;
			}
			set
			{
				m_AutoElevation = value;
			}
		}

		public CacheStatistics Statistics
		{
			get
			{
				m_statistics.Size = m_Objects.Count + m_ObjectsLv2.Count;
				return m_statistics;
			}
		}

		public BasicCache()
			: this(50000, "DefaultCache")
		{
			m_Name = "DefaultCache";
		}

		public BasicCache(int _size, string _name)
		{
			m_TableInitSize = _size;
			m_ElevationCutline = _size / 2;
			m_Name = _name;
			m_Objects = new Hashtable(_size);
			m_ObjectsLv2 = new Hashtable(0);
			m_statistics = new CacheStatistics(m_Name);
		}

		public void Elevation()
		{
			m_ObjectsLv2 = m_Objects;
			m_Objects = new Hashtable(m_TableInitSize);
			WorkSession.WriteStatus("BasicCache.Elevation() : 캐쉬 에레베이션이 일어났습니다");
		}

		public object Find(object _Key)
		{
			lock (this)
			{
				object obj = m_Objects[_Key];
				if (obj == null)
				{
					obj = m_ObjectsLv2[_Key];
				}
				return obj;
			}
		}

		public bool Push(object _Id, object _Data)
		{
			if (_Data == null)
			{
				return false;
			}
			lock (this)
			{
				if (m_AutoElevation && m_Objects.Count >= m_ElevationCutline)
				{
					Elevation();
				}
				m_Objects[_Id] = _Data;
				return true;
			}
		}

		public void Pop(object _Key)
		{
			lock (this)
			{
				m_Objects.Remove(_Key);
				m_ObjectsLv2.Remove(_Key);
			}
		}

		public object Extract(object _Key)
		{
			lock (this)
			{
				object obj = m_Objects[_Key];
				if (obj == null)
				{
					obj = m_ObjectsLv2[_Key];
					if (obj != null)
					{
						m_ObjectsLv2.Remove(_Key);
						m_statistics.CacheHit();
					}
					else
					{
						m_statistics.CacheMiss();
					}
				}
				else
				{
					m_Objects.Remove(_Key);
					m_statistics.CacheHit();
				}
				return obj;
			}
		}
	}
}
