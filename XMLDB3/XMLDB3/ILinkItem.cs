namespace XMLDB3
{
	public interface ILinkItem : ICacheItem
	{
		ILinkItem Next
		{
			get;
		}

		ILinkItem Prev
		{
			get;
		}

		ISection Section
		{
			get;
		}
	}
}
