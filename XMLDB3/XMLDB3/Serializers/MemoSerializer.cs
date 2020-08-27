using Mabinogi;

namespace XMLDB3
{
	public class MemoSerializer
	{
		public static Memo Serialize(Message _message)
		{
			Memo memo = new Memo();
			memo.sender = new MemoCharacter();
			memo.sender.account = _message.ReadString();
			memo.sender.name = _message.ReadString();
			memo.sender.server = _message.ReadString();
			int num = _message.ReadS32();
			if (num > 0)
			{
				memo.receipants = new MemoCharacter[num];
				for (int i = 0; i < num; i++)
				{
					memo.receipants[i] = new MemoCharacter();
					memo.receipants[i].account = _message.ReadString();
					memo.receipants[i].name = _message.ReadString();
					memo.receipants[i].server = _message.ReadString();
				}
			}
			else
			{
				memo.receipants = null;
			}
			memo.content = _message.ReadString();
			return memo;
		}
	}
}
