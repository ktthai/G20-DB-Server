using Mabinogi;

namespace XMLDB3
{
	public class PrivateFarmVisitorSerializer
	{
		public static PrivateFarmVisitor Serialize(Message _message)
		{
			PrivateFarmVisitor privateFarmVisitor = new PrivateFarmVisitor();
			privateFarmVisitor.charName = _message.ReadString();
			privateFarmVisitor.accountName = _message.ReadString();
			privateFarmVisitor.charId = _message.ReadS64();
			privateFarmVisitor.status = _message.ReadU8();
			return privateFarmVisitor;
		}

		public static void Deserialize(PrivateFarmVisitor _visitor, Message _message)
		{
			_message.WriteString(_visitor.charName);
			_message.WriteString(_visitor.accountName);
			_message.WriteS64(_visitor.charId);
			_message.WriteU8(_visitor.status);
		}
	}
}
