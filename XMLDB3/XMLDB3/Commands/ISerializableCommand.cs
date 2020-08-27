namespace XMLDB3
{
	public interface ISerializableCommand
	{
		void OnSerialize(IObjLockRegistHelper _helper, bool bBegin);
	}
}
