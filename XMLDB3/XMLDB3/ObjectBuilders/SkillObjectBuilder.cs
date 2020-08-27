using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class SkillObjectBuilder
	{
		public static CharacterSkill[] Build(SimpleReader reader)
		{
			if (reader.HasRows)
			{
				List<CharacterSkill> arrayList = new List<CharacterSkill>();
				CharacterSkill characterSkill;
				while (reader.Read())
				{
					characterSkill = new CharacterSkill();
					characterSkill.id = reader.GetInt16(Mabinogi.SQL.Columns.CharacterSkill.Skill);
					characterSkill.version = (short)reader.GetInt32(Mabinogi.SQL.Columns.CharacterSkill.Version);
					characterSkill.level = reader.GetByte(Mabinogi.SQL.Columns.CharacterSkill.Level);
					characterSkill.maxlevel = reader.GetByte(Mabinogi.SQL.Columns.CharacterSkill.MaxLevel);
					characterSkill.experience = reader.GetInt32(Mabinogi.SQL.Columns.CharacterSkill.Experience);
					characterSkill.count = reader.GetInt16(Mabinogi.SQL.Columns.CharacterSkill.Count);
					characterSkill.flag = reader.GetInt16(Mabinogi.SQL.Columns.CharacterSkill.Flag);
					characterSkill.subflag1 = reader.GetInt16(Mabinogi.SQL.Columns.CharacterSkill.SubFlag1);
					characterSkill.subflag2 = reader.GetInt16(Mabinogi.SQL.Columns.CharacterSkill.SubFlag2);
					characterSkill.subflag3 = reader.GetInt16(Mabinogi.SQL.Columns.CharacterSkill.SubFlag3);
					characterSkill.subflag4 = reader.GetInt16(Mabinogi.SQL.Columns.CharacterSkill.SubFlag4);
					characterSkill.subflag5 = reader.GetInt16(Mabinogi.SQL.Columns.CharacterSkill.SubFlag5);
					characterSkill.subflag6 = reader.GetInt16(Mabinogi.SQL.Columns.CharacterSkill.SubFlag6);
					characterSkill.subflag7 = reader.GetInt16(Mabinogi.SQL.Columns.CharacterSkill.SubFlag7);
					characterSkill.subflag8 = reader.GetInt16(Mabinogi.SQL.Columns.CharacterSkill.SubFlag8);
					characterSkill.subflag9 = reader.GetInt16(Mabinogi.SQL.Columns.CharacterSkill.SubFlag9);
					characterSkill.lastPromotionTime = reader.GetInt64(Mabinogi.SQL.Columns.CharacterSkill.LastPromotionTime);
					characterSkill.promotionConditionCount = reader.GetInt16(Mabinogi.SQL.Columns.CharacterSkill.PromotionConditionCount);
					characterSkill.promotionExperience = reader.GetInt32(Mabinogi.SQL.Columns.CharacterSkill.PromotionExperience);
					arrayList.Add(characterSkill);
				}
				return arrayList.ToArray();
			}
			return new CharacterSkill[0];
		}
	}
}
