using System.Collections;

namespace XMLDB3
{
	public class BankSlotComparer : IComparer
	{
		private IComparer internalComparer = new CaseInsensitiveComparer();

		int IComparer.Compare(object x, object y)
		{
			return internalComparer.Compare(((BankSlot)x).Name, ((BankSlot)y).Name);
		}
	}
}
