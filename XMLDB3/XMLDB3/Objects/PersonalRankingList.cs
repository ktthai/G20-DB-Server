using System;

public class PersonalRankingList
{
	private PersonalRanking[] personalRankingField;

	public PersonalRanking[] Items
	{
		get
		{
			return personalRankingField;
		}
		set
		{
			personalRankingField = value;
		}
	}
}
