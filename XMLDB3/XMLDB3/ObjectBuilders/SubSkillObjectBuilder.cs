using Mabinogi.SQL;
using System.Collections.Generic;

namespace XMLDB3
{
	internal class SubSkillObjectBuilder
	{
		public static CharacterSubSkillSet Build(SimpleReader reader)
		{
			CharacterSubSkillSet characterSubSkillSet = new CharacterSubSkillSet();
			if (reader.HasRows)
			{
				List<CharacterSubSkill> list = new List<CharacterSubSkill>();
				while(reader.Read())
				{
					CharacterSubSkill characterSubSkill = new CharacterSubSkill();
					characterSubSkill.id = (ushort)reader.GetInt32(Mabinogi.SQL.Columns.CharacterSubskill.Subskill);
					characterSubSkill.level = (uint)reader.GetInt32(Mabinogi.SQL.Columns.CharacterSubskill.Level);
					characterSubSkill.exp = (uint)reader.GetInt32(Mabinogi.SQL.Columns.CharacterSubskill.Experience);
					list.Add(characterSubSkill);
				}
				characterSubSkillSet.subSkillSet = list.ToArray();
			}
			return characterSubSkillSet;
		}
	}
}
