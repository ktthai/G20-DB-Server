using Mabinogi.SQL;

namespace XMLDB3
{
	public class MarriageObjectBuilder
	{
		public static CharacterMarriage Build(SimpleReader reader)
		{
			CharacterMarriage characterMarriage = new CharacterMarriage();
			characterMarriage.mateid = reader.GetInt64(Mabinogi.SQL.Columns.Character.MateId);
			characterMarriage.matename = reader.GetString(Mabinogi.SQL.Columns.Character.MateName);
			characterMarriage.marriagetime = reader.GetInt32(Mabinogi.SQL.Columns.Character.MarriageTime);
			characterMarriage.marriagecount = reader.GetInt16(Mabinogi.SQL.Columns.Character.MarriageCount);
			return characterMarriage;
		}
	}
}
