using System.Collections;

namespace XMLDB3
{
	public class CollectionItemComparer : IComparer
	{
		private IComparer internalComparer = new CaseInsensitiveComparer();

		int IComparer.Compare(object x, object y)
		{
			return internalComparer.Compare(x, y);
		}
	}
}
