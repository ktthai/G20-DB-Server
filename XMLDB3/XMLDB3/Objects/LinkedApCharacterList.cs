using System.Collections.Generic;
public class LinkedApCharacterList
{
	
	public Dictionary<long, LinkedApCharacter>  characterTable;

	public LinkedApCharacter[] linkedApCharacterList
	{
		get
		{
			if (characterTable != null)
			{
				LinkedApCharacter[] array = new LinkedApCharacter[characterTable.Values.Count];
				characterTable.Values.CopyTo(array, 0);
				return array;
			}
			return null;
		}
		set
		{
			characterTable = new Dictionary<long, LinkedApCharacter>(value.Length);
			foreach (LinkedApCharacter linkedApCharacter in value)
			{
				characterTable.Add(linkedApCharacter.charID, linkedApCharacter);
			}
		}
	}
}
