using Mabinogi;

namespace Authenticator
{
	public class RemoteFunction_CharacterCardTranBegin : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			string account = base.Input.ReadString();
			long cardid = base.Input.ReadS64();
			long characterid = base.Input.ReadS64();
			string charactername = base.Input.ReadString();
			int rebirthcount = base.Input.ReadS32();
			string server = base.Input.ReadString();
			string comment = base.Input.ReadString();
			if (CharacterCard.Begin_UsingTransaction(account, cardid, characterid, charactername, rebirthcount, server, comment))
			{
				newReply.WriteU8(1);
			}
			else
			{
				newReply.WriteU8(0);
			}
			Reply(newReply);
			return newReply;
		}

		protected override string BuildName(Message _input)
		{
			string text = _input.ReadString();
			long num = _input.ReadS64();
			long num2 = _input.ReadS64();
			string text2 = _input.ReadString();
			_input.ReadS32();
			string text3 = _input.ReadString();
			_input.ReadString();
			return "Character Card Using Transaction Begin (card:" + num + "@" + text + ", target:" + text2 + "<" + num2 + ">@" + text3 + ")";
		}
	}
}
