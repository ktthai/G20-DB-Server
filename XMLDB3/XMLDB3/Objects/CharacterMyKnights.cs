using System;

public class CharacterMyKnights
{
	public string name;

	public ushort level;

	public uint exp;

	public uint trainingPoint;

	public ushort dateBuffMember;

	public DateTime makeTime;

	public ushort addedSlotCount;

	public CharacterMyKnightsMember[] memberList;

	public bool IsSame(CharacterMyKnights other)
	{
		if (other != null && name == other.name && level == other.level && exp == other.exp && trainingPoint == other.trainingPoint && dateBuffMember == other.dateBuffMember && makeTime == other.makeTime && addedSlotCount == other.addedSlotCount)
		{
			return true;
		}
		return false;
	}
}
