
public class CharacterKeywordSet
{
	public short keywordId;

	public bool IsSame(CharacterKeywordSet a)
	{
		if (keywordId == a.keywordId)
		{
			return true;
		}
		return false;
	}
}
