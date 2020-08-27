using Mabinogi;

namespace XMLDB3
{
	public class FamilyMemberSerializer
	{
		public static FamilyListFamilyMember Serialize(Message _message)
		{
			FamilyListFamilyMember familyListFamilyMember = new FamilyListFamilyMember();
			familyListFamilyMember.memberID = _message.ReadS64();
			familyListFamilyMember.memberName = _message.ReadString();
			familyListFamilyMember.memberClass = _message.ReadU16();
			return familyListFamilyMember;
		}

		public static void Deserialize(FamilyListFamilyMember _data, Message _message)
		{
			if (_data != null)
			{
				_message.WriteS64(_data.memberID);
				_message.WriteString(_data.memberName);
				_message.WriteU16(_data.memberClass);
			}
		}
	}
}
