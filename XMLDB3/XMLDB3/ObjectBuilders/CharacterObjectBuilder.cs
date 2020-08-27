using System;
using System.Data;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class CharacterObjectBuilder
	{
		
		public static bool BuildCharacter(SimpleReader reader, out CharacterInfo characterInfo)
		{
            if (reader.Read())
            {
                characterInfo = new CharacterInfo();

                characterInfo.Private = PrivateObjectBuilder.BuildCharacterPrivate(reader);

                characterInfo.id = reader.GetInt64(Mabinogi.SQL.Columns.Character.Id);
                characterInfo.name = reader.GetString(Mabinogi.SQL.Columns.Character.Name);

                characterInfo.inventoryHash = reader.GetString(Mabinogi.SQL.Columns.Character.CouponCode);
                characterInfo.updatetime = reader.GetDateTime(Mabinogi.SQL.Columns.Character.UpdateTime);

                characterInfo.appearance = AppearanceObjectBuilder.Build(reader);
                characterInfo.parameter = ParameterObjectBuilder.Build(reader);
                characterInfo.parameterEx = ParameterExObjectBuilder.Build(reader);
                characterInfo.data = DataObjectBuilder.BuildData(reader);
                characterInfo.memorys = MemoryObjectBuilder.Build(reader);
                characterInfo.conditions = ConditionObjectBuilder.Build(reader);
                characterInfo.arbeit = ArbeitObjectBuilder.Build(reader);

                characterInfo.titles = TitleObjectBuilder.Build(reader);
                characterInfo.service = ServiceObjectBuilder.Build(reader);

                characterInfo.marriage = MarriageObjectBuilder.Build(reader);

                characterInfo.farm = FarmObjectBuilder.Build(reader);
                characterInfo.heartSticker = HeartStickerObjectBuilder.Build(reader);
                characterInfo.joust = JoustObjectBuilder.Build(reader);
                characterInfo.macroChecker = MacroCheckerObjectBuilder.Build(reader);
                characterInfo.donation = DonationObjectBuilder.Build(reader);
                characterInfo.job = JobObjectBuilder.Build(reader);
                return true;
            }
            else
            {
                characterInfo = null;
                return false;
            }
        }

		public static void BuildSkills(SimpleReader reader, ref CharacterInfo characterInfo)
		{
            characterInfo.skills = SkillObjectBuilder.Build(reader);
        }

		public static void BuildCharacterQuests(SimpleReader reader, CharacterInfo characterInfo)
		{
            characterInfo.Private.registereds = PrivateObjectBuilder.BuildPrivateRegistered(reader);
        }

		public static void BuildLargeItems(SimpleReader reader, CharacterInfo characterInfo, ref InventoryHash inventoryHash)
		{
			InventoryObjectBuilder.BuildLargeItems(reader, characterInfo, ref inventoryHash);
		}

        public static void BuildSmallItems(SimpleReader reader, CharacterInfo characterInfo, ref InventoryHash inventoryHash)
        {
            InventoryObjectBuilder.BuildSmallItems(reader, characterInfo, ref inventoryHash);
        }

        public static void BuildHugeItems(SimpleReader reader, CharacterInfo characterInfo, ref InventoryHash inventoryHash)
        {
            InventoryObjectBuilder.BuildHugeItems(reader, characterInfo, ref inventoryHash);
        }

        public static void BuildQuestItems(SimpleReader reader, CharacterInfo characterInfo, ref InventoryHash inventoryHash)
        {
            InventoryObjectBuilder.BuildQuestItems(reader, characterInfo, ref inventoryHash);
        }

        public static void BuildEgoItems(SimpleReader reader, CharacterInfo characterInfo, ref InventoryHash inventoryHash)
        {
            InventoryObjectBuilder.BuildEgoItems(reader, characterInfo, ref inventoryHash);
        }


		public static void BuildCharacterAchievements(SimpleReader reader, CharacterInfo characterInfo)
		{
			characterInfo.achievements = AchievementObjectBuilder.Build(reader);
		}

		public static void BuildFavoritePrivateFarm(SimpleReader reader, CharacterInfo characterInfo)
		{
            characterInfo.prifateFarm = FavoritePrivateFarmObjectBuilder.Build(reader);
        }

		public static bool BuildDeeds(SimpleReader reader, CharacterInfo characterInfo)
		{
            characterInfo.deed = DeedObejctBuilder.Build(reader);
            return characterInfo.deed != null;
        }

        public static void BuildShape(SimpleReader reader, CharacterInfo characterInfo)
        {
            characterInfo.shape = ShapeObjectBuilder.Build(reader);
        }

		public static void BuildKeyword(SimpleReader reader, CharacterInfo characterInfo)
		{
			characterInfo.keyword = KeywordObjectBuilder.Build(reader);
		}

		public static void BuildPvP(SimpleReader reader, CharacterInfo characterInfo)
		{
			characterInfo.PVP = PVPObjectBuilder.Build(reader);
		}

		public static void BuildDivineKnight(SimpleReader reader, CharacterInfo characterInfo)
		{
			characterInfo.divineKnight = DivineKnightObjectBuilder.Build(reader);
		}

        public static void BuildSubSkill(SimpleReader reader, CharacterInfo characterInfo)
        {
			characterInfo.subSkill = SubSkillObjectBuilder.Build(reader);
        }

		public static void BuildMeta(SimpleReader reader, CharacterInfo characterInfo)
		{
			characterInfo.data.meta = DataObjectBuilder.BuildMeta(reader);
		}

		public static void BuildMyKnights(SimpleReader reader, CharacterInfo characterInfo)
		{
			characterInfo.myKnights = MyKnightsObjectBuilder.BuildCharacterMyKnights(reader);
		}
        public static void BuildMyKnightsMembers(SimpleReader reader, CharacterInfo characterInfo)
        {
            MyKnightsObjectBuilder.BuildMyKnightsMembers(reader, characterInfo.myKnights);
        }

        public static CharacterInfo Build(long id, SimpleConnection conn)
		{
            CharacterInfo characterInfo;

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Character))
            {
                cmd.Where(Mabinogi.SQL.Columns.Character.Id, id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (CharacterObjectBuilder.BuildCharacter(reader, out characterInfo) == false)
                        return null;
                }
            }

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterSkill))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharacterSkill.Id, id);
                using (var reader = cmd.ExecuteReader())
                {
                    CharacterObjectBuilder.BuildSkills(reader, ref characterInfo);
                }
            }

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterQuest))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharacterQuest.Id, id);
                using (var reader = cmd.ExecuteReader())
                {
                    CharacterObjectBuilder.BuildCharacterQuests(reader, characterInfo);
                }
            }

            InventoryHash inventoryHash;
            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharItemLarge))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharItem.Id, id);
                
                using (var reader = cmd.ExecuteReader())
                {
                    inventoryHash = new InventoryHash(characterInfo.id);
                    CharacterObjectBuilder.BuildLargeItems(reader, characterInfo, ref inventoryHash);
                }
            }

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharItemSmall))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharItem.Id, id);
                using (var reader = cmd.ExecuteReader())
                {
                    CharacterObjectBuilder.BuildSmallItems(reader, characterInfo, ref inventoryHash);
                }
            }

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharItemHuge))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharItem.Id, id);
                using (var reader = cmd.ExecuteReader())
                {
                    CharacterObjectBuilder.BuildHugeItems(reader, characterInfo, ref inventoryHash);
                }
            }

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharItemQuest))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharItem.Id, id);
                using (var reader = cmd.ExecuteReader())
                {
                    CharacterObjectBuilder.BuildQuestItems(reader, characterInfo, ref inventoryHash);
                }
            }

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharItemEgo))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharItem.Id, id);
                using (var reader = cmd.ExecuteReader())
                {
                    CharacterObjectBuilder.BuildEgoItems(reader, characterInfo, ref inventoryHash);
                }
            }

            if (inventoryHash != null)
                characterInfo.inventoryHash = inventoryHash.ToString();

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterAchievement))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharacterAchievement.Id, id);
                using (var reader = cmd.ExecuteReader())
                {
                    CharacterObjectBuilder.BuildCharacterAchievements(reader, characterInfo);
                }
            }

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.FavoritePrivateFarm))
            {
                cmd.Where(Mabinogi.SQL.Columns.FavoritePrivateFarm.CharId, id);
                using (var reader = cmd.ExecuteReader())
                {
                    CharacterObjectBuilder.BuildFavoritePrivateFarm(reader, characterInfo);
                }
            }

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterDeed))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharacterDeed.Id, id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (CharacterObjectBuilder.BuildDeeds(reader, characterInfo) == false)
                    {
                        reader.Close();
                        using (var cmd2 = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterDeed))
                        {
                            cmd2.Set(Mabinogi.SQL.Columns.CharacterDeed.Id, id);

                            cmd2.Execute();

                            characterInfo.deed = new CharacterDeed();
                        }
                    }
                }
            }

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterShape))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharacterShape.Id, id);
                using (var reader = cmd.ExecuteReader())
                {
                    CharacterObjectBuilder.BuildShape(reader, characterInfo);
                }
            }

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterKeyword))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharacterKeyword.CharId, id);
                using (var reader = cmd.ExecuteReader())
                {
                    CharacterObjectBuilder.BuildKeyword(reader, characterInfo);
                }
            }

            if (ConfigManager.IsPVPable)
            {
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterPvP))
                {
                    cmd.Where(Mabinogi.SQL.Columns.CharacterPvP.Id, id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        CharacterObjectBuilder.BuildPvP(reader, characterInfo);
                    }
                }
            }

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterDivineKnight))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharacterDivineKnight.Id, id);
                using (var reader = cmd.ExecuteReader())
                {
                    CharacterObjectBuilder.BuildDivineKnight(reader, characterInfo);
                }
            }

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterSubskill))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharacterSubskill.Id, id);
                using (var reader = cmd.ExecuteReader())
                {
                    CharacterObjectBuilder.BuildSubSkill(reader, characterInfo);
                }
            }

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterMyKnights))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharacterMyKnights.CharId, id);
                using (var reader = cmd.ExecuteReader())
                {
                    CharacterObjectBuilder.BuildMyKnights(reader, characterInfo);
                }
            }

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterMyKnightsMember))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharacterMyKnightsMember.CharId, id);
                using (var reader = cmd.ExecuteReader())
                {
                    CharacterObjectBuilder.BuildMyKnightsMembers(reader, characterInfo);
                }
            }

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterMeta))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharacterMeta.CharId, id);
                using (var reader = cmd.ExecuteReader())
                {
                    BuildMeta(reader, characterInfo);
                }
            }

            return characterInfo;
		}
	}
}
