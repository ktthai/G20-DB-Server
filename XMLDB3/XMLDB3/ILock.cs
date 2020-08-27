namespace XMLDB3
{
	public interface ILock
	{
		IObjLockRegistHelper BeginHelper
		{
			get;
		}

		IObjLockRegistHelper EndHelper
		{
			get;
		}

		void Wait();

		void Close();

		void ForceUnregist();
	}
}
