
public class CharacterMyKnightsMember
{
	public ushort id;

	public byte isRecruited;

	public ushort holy;

	public ushort strength;

	public ushort intelligence;

	public ushort dexterity;

	public ushort will;

	public ushort luck;

	public ushort favorLevel;

	public uint favorExp;

	public ushort stress;

	public ulong woundTime;

	public byte isSelfCured;

	public ushort curTraining;

	public ulong trainingStartTime;

	public ulong curTask;

	public uint curTaskTemplate;

	public ulong taskEndTime;

	public ulong restStartTime;

	public ulong lastDateTime;

	public ulong firstRecruitTime;

	public ulong lastRecruitTime;

	public ulong lastDismissTime;

	public ushort dismissCount;

	public uint taskTryCount;

	public uint taskSuccessCount;

	public ulong favorTalkCount;

	public string latestDateList;

	public bool IsSame(CharacterMyKnightsMember other)
	{
		if (other != null && id == other.id && isRecruited == other.isRecruited && holy == other.holy && strength == other.strength && intelligence == other.intelligence && dexterity == other.dexterity && will == other.will && luck == other.luck && favorLevel == other.favorLevel && favorExp == other.favorExp && stress == other.stress && woundTime == other.woundTime && isSelfCured == other.isSelfCured && curTraining == other.curTraining && trainingStartTime == other.trainingStartTime && curTask == other.curTask && curTaskTemplate == other.curTaskTemplate && taskEndTime == other.taskEndTime && restStartTime == other.restStartTime && lastDateTime == other.lastDateTime && firstRecruitTime == other.firstRecruitTime && lastRecruitTime == other.lastRecruitTime && lastDismissTime == other.lastDismissTime && dismissCount == other.dismissCount && taskTryCount == other.taskTryCount && taskSuccessCount == other.taskSuccessCount && favorTalkCount == other.favorTalkCount && latestDateList == other.latestDateList)
		{
			return true;
		}
		return false;
	}
}
