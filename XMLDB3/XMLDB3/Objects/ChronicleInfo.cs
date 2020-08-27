public class ChronicleInfo : ChronicleInfoBase
{
	public bool IsRankingChronicle => sort != string.Empty;

	public bool ContentEquals(ChronicleInfo _info)
	{
		if (questName == _info.questName && keyword == _info.keyword && localtext == _info.localtext && sort == _info.sort && group == _info.group && source == _info.source && width == _info.width)
		{
			return height == _info.height;
		}
		return false;
	}
}
