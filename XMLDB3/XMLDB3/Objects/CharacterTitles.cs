using System.Collections.Generic;

public class CharacterTitles
{
	public List<CharacterTitlesTitle> title { get; set; }

	public short selected { get; set; }

	public ulong appliedtime { get; set; }

	public short option { get; set; }

	public CharacterTitles()
	{
		title = new List<CharacterTitlesTitle>();
	}
}
