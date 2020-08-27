using Mabinogi;
using System.Collections.Generic;

namespace XMLDB3
{
	public class FamilySerializer
	{
		public static FamilyListFamily Serialize(Message _message)
		{
			FamilyListFamily familyListFamily = new FamilyListFamily();
			familyListFamily.familyID = _message.ReadS64();
			familyListFamily.familyName = _message.ReadString();
			familyListFamily.headID = _message.ReadS64();
			familyListFamily.state = _message.ReadU16();
			familyListFamily.tradition = _message.ReadU32();
			familyListFamily.meta = _message.ReadString();
			uint num = _message.ReadU32();
			if (num != 0)
			{
				familyListFamily.member = new List<FamilyListFamilyMember>((int)num);
				for (int i = 0; i < num; i++)
				{
					familyListFamily.member[i] = FamilyMemberSerializer.Serialize(_message);
				}
			}
			else
			{
				familyListFamily.member = null;
			}
			return familyListFamily;
		}

		public static void Deserialize(FamilyListFamily _data, Message _message)
		{
			if (_data == null)
			{
				return;
			}
			_message.WriteS64(_data.familyID);
			_message.WriteString(_data.familyName);
			_message.WriteS64(_data.headID);
			_message.WriteU16(_data.state);
			_message.WriteU32(_data.tradition);
			_message.WriteString(_data.meta);
			if (_data == null || _data.member == null || _data.member.Count == 0)
			{
				_message.WriteS32(0);
				return;
			}
			_message.WriteS32(_data.member.Count);

			foreach (FamilyListFamilyMember data in _data.member)
			{
				FamilyMemberSerializer.Deserialize(data, _message);
			}
		}
	}
}
