namespace XMLDB3
{
	public interface IEncryptionMethod
	{
		void Init();

		void Close();

		byte[] EncryptColumn(string _input);

		string DecryptColumn(byte[] _inBuf);
	}
}
