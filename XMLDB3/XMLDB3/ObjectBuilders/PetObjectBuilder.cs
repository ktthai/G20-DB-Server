using Mabinogi.SQL;
using System;

namespace XMLDB3
{
	public class PetObjectBuilder
	{

		public static PetInfo Build(long _id, SimpleConnection conn)
        {
			PetInfo petInfo;
			using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Pet))
            {
                cmd.Where(Mabinogi.SQL.Columns.Pet.Id, _id);
                using (var petReader = cmd.ExecuteReader())
                {
                    petInfo = PetObjectBuilder.BuildPetInfo(petReader);
                }
            }

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.PetSkill))
            {
                cmd.Where(Mabinogi.SQL.Columns.PetSkill.Id, _id);

                cmd.Set(Mabinogi.SQL.Columns.PetSkill.Skill, 0);
                cmd.Set(Mabinogi.SQL.Columns.PetSkill.Level, 0);
                cmd.Set(Mabinogi.SQL.Columns.PetSkill.Flag, 0);

                using (var petSkillReader = cmd.ExecuteReader())
                {
                    petInfo.skills = PetSkillObjectBuilder.Build(petSkillReader);
                }
            }

            InventoryHash inventoryHash = new InventoryHash(petInfo.id);
            petInfo.inventory = new System.Collections.Generic.Dictionary<long, Item>();

            petInfo.strToHash = inventoryHash.ToString();
            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharItemLarge))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharItem.Id, _id);

                using (var largeItemReader = cmd.ExecuteReader())
                    PetInventoryObjectBuilder.BuildLargeItems(inventoryHash, petInfo.inventory, largeItemReader);
            }

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharItemSmall))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharItem.Id, _id);

                using (var smallItemReader = cmd.ExecuteReader())
                    PetInventoryObjectBuilder.BuildSmallItems(inventoryHash, petInfo.inventory, smallItemReader);
            }

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharItemHuge))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharItem.Id, _id);

                using (var hugeItemReader = cmd.ExecuteReader())
                    PetInventoryObjectBuilder.BuildHugeItems(inventoryHash, petInfo.inventory, hugeItemReader);
            }

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharItemQuest))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharItem.Id, _id);

                using (var questItemReader = cmd.ExecuteReader())
                    PetInventoryObjectBuilder.BuildQuestItems(inventoryHash, petInfo.inventory, questItemReader);
            }
			return petInfo;
        }

		public static PetInfo BuildPetInfo(SimpleReader petReader)
		{
			try
			{
				if (petReader == null)
				{
					throw new Exception("펫 테이블이 없습니다.");
				}
				if (petReader.Read() == false)
				{
					throw new Exception("캐릭터 테이블 열이 하나 이상입니다.");
				}
				PetInfo petInfo = new PetInfo();
				petInfo.id = petReader.GetInt64(Mabinogi.SQL.Columns.Pet.Id);
				petInfo.name = petReader.GetString(Mabinogi.SQL.Columns.Pet.Name);
				
				string couponCode = petReader.GetString(Mabinogi.SQL.Columns.Pet.CouponCode);
				if (couponCode == null)
				{
					petInfo.inventoryHash = string.Empty;
				}
				else
				{
					petInfo.inventoryHash = couponCode;
				}

				DateTime updateTime;
				if (petReader.GetDateTimeSafe(Mabinogi.SQL.Columns.Pet.UpdateTime, out updateTime))
				{
					 petInfo.updatetime = updateTime;
				}
				else
				{
					petInfo.updatetime = DateTime.Now;
				}

				petInfo.appearance = PetAppearanceObjectBuilder.Build(petReader);
				petInfo.parameter = PetParameterObjectBuilder.Build(petReader);
				petInfo.parameterEx = PetParameterExObjectBuilder.Build(petReader);
				petInfo.data = PetDataObjectBuilder.Build(petReader);
				petInfo.memorys = PetMemoryObjectBuilder.Build(petReader);
				petInfo.conditions = PetConditionObjectBuilder.Build(petReader);
				petInfo.@private = PetPrivateObjectBuilder.Build(petReader);
				petInfo.summon = PetSummonObjectBuilder.Build(petReader);
				petInfo.macroChecker = PetMacroCheckerObjectBuilder.Build(petReader);
				return petInfo;
			}
			catch (Exception ex)
			{
				ExceptionMonitor.ExceptionRaised(ex);
				return null;
			}
		}
	}
}
