using System;

namespace Mabinogi.Network
{
	public class NetworkBuffer
	{
		private const int InitBufferSize = 1024;

		private byte[] m_pBuffer;

		private int m_iBufSize;

		private int m_iTrimCount;

		public NetworkBuffer()
		{
			m_pBuffer = new byte[1024];
			m_iBufSize = 0;
			m_iTrimCount = 0;
		}

		public void AddBuffer(byte[] _pIn, int _iSize)
		{
			if (_pIn == null || _iSize <= 0)
			{
				return;
			}
			if (m_iBufSize + _iSize <= m_pBuffer.Length)
			{
				Array.Copy(_pIn, 0, m_pBuffer, m_iBufSize, _iSize);
			}
			else
			{
				int num;
				for (num = m_pBuffer.Length; num < m_iBufSize + _iSize; num *= 2)
				{
				}
				byte[] array = new byte[num];
				Array.Copy(m_pBuffer, 0, array, 0, m_iBufSize);
				Array.Copy(_pIn, 0, array, m_iBufSize, _iSize);
				m_pBuffer = array;
				m_iTrimCount = 0;
			}
			m_iBufSize += _iSize;
		}

		public bool PopBuffer(int _iSize)
		{
			if (_iSize > m_iBufSize)
			{
				return false;
			}
			if (m_pBuffer.Length >= m_iBufSize * 2 && m_pBuffer.Length > 1024)
			{
				m_iTrimCount++;
			}
			else
			{
				m_iTrimCount = 0;
			}
			if (m_iTrimCount == 3)
			{
				m_iTrimCount = 0;
				byte[] array = new byte[m_pBuffer.Length / 2];
				Array.Copy(m_pBuffer, _iSize, array, 0, m_iBufSize - _iSize);
				m_pBuffer = array;
			}
			else
			{
				Array.Copy(m_pBuffer, _iSize, m_pBuffer, 0, m_iBufSize - _iSize);
			}
			m_iBufSize -= _iSize;
			return true;
		}

		public byte[] GetBuffer()
		{
			return m_pBuffer;
		}

		public int GetBufSize()
		{
			return m_iBufSize;
		}
	}
}
