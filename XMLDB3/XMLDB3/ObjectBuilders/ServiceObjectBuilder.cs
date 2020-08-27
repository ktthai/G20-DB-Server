using Mabinogi.SQL;

namespace XMLDB3
{
	public class ServiceObjectBuilder
	{
		public static CharacterService Build(SimpleReader reader)
		{
			CharacterService characterService = new CharacterService();
			characterService.nsrespawncount = reader.GetByte(Mabinogi.SQL.Columns.Character.NsRespawnCount);
			characterService.nslastrespawnday = reader.GetInt32(Mabinogi.SQL.Columns.Character.NsLastRespawnDay);
			characterService.nsgiftreceiveday = reader.GetInt32(Mabinogi.SQL.Columns.Character.NsGiftReceiveDay);
			characterService.apgiftreceiveday = reader.GetInt32(Mabinogi.SQL.Columns.Character.ApGiftReceiveDay);
			characterService.nsbombcount = reader.GetByte(Mabinogi.SQL.Columns.Character.NsBombCount);
			characterService.nsbombday = reader.GetInt32(Mabinogi.SQL.Columns.Character.NsBombDay);
			return characterService;
		}
	}
}
