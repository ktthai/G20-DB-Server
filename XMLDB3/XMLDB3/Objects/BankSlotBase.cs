
using System.Collections.Generic;

public class BankSlotBase
{
	public BankSlotInfo slot;

	public List<BankItem> item;

	public BankSlotBase()
	{
		item = new List<BankItem>();
	}
}
