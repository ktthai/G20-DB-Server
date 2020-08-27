namespace XMLDB3
{
	public class character_meta_row
	{
		private long m_charID;

		private string m_mcode;

		private string m_mtype;

		private string m_mdata;

		public long charID
		{
			get
			{
				return m_charID;
			}
			set
			{
				m_charID = value;
			}
		}

		public string mcode
		{
			get
			{
				return m_mcode;
			}
			set
			{
				m_mcode = value;
			}
		}

		public string mtype
		{
			get
			{
				return m_mtype;
			}
			set
			{
				m_mtype = value;
			}
		}

		public string mdata
		{
			get
			{
				return m_mdata;
			}
			set
			{
				m_mdata = value;
			}
		}

		public string ToMetaString()
		{
			return m_mcode + ":" + m_mtype + ":" + m_mdata;
		}
	}
}
