using Mabinogi.SQL;
using System.Collections.Generic;

namespace XMLDB3
{
	public class PetSkillObjectBuilder
	{
		public static PetSkill[] Build(SimpleReader reader)
		{
			if (reader != null && reader.HasRows)
			{
				List<PetSkill> arrayList = new List<PetSkill>();
				PetSkill petSkill;
				while (reader.Read())
				{
					petSkill = new PetSkill();
					petSkill.id = reader.GetInt16(Mabinogi.SQL.Columns.PetSkill.Skill);
					petSkill.level = reader.GetByte(Mabinogi.SQL.Columns.PetSkill.Level);
					petSkill.flag = reader.GetInt16(Mabinogi.SQL.Columns.PetSkill.Flag);
					arrayList.Add(petSkill);
				}
				return arrayList.ToArray();
			}
			return null;
		}
	}
}
