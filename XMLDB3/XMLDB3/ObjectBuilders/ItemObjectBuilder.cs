using Mabinogi.SQL;

namespace XMLDB3
{
	public class ItemObjectBuilder
	{
		public static Item BuildLargeItem(SimpleReader reader)
		{
			Item item = new Item();
			item.storedtype = 1;
			item.id = reader.GetInt64(Mabinogi.SQL.Columns.Item.ItemId);
			
			item.Class = reader.GetInt32(Mabinogi.SQL.Columns.Item.Class);
			item.color_01 = reader.GetInt32(Mabinogi.SQL.Columns.Item.Color1);
			item.color_02 = reader.GetInt32(Mabinogi.SQL.Columns.Item.Color2);
			item.color_03 = reader.GetInt32(Mabinogi.SQL.Columns.Item.Color3);
			item.price = reader.GetInt32(Mabinogi.SQL.Columns.Item.Price);
			item.bundle = reader.GetInt16(Mabinogi.SQL.Columns.Item.Bundle);
            item.linked_pocket = (byte)reader.GetInt32(Mabinogi.SQL.Columns.Item.LinkedPocket);
            item.figure = reader.GetInt32(Mabinogi.SQL.Columns.Item.Figure);
			item.flag = reader.GetByte(Mabinogi.SQL.Columns.Item.Flag);
			item.durability = reader.GetInt32(Mabinogi.SQL.Columns.Item.Durability);
			item.durability_max = reader.GetInt32(Mabinogi.SQL.Columns.Item.DurabilityMax);
			item.origin_durability_max = reader.GetInt32(Mabinogi.SQL.Columns.Item.OriginalDurabilityMax);
			item.attack_min = reader.GetInt16(Mabinogi.SQL.Columns.Item.AttackMin);
			item.attack_max = reader.GetInt16(Mabinogi.SQL.Columns.Item.AttackMax);
			item.wattack_min = reader.GetInt16(Mabinogi.SQL.Columns.Item.WAttackMin);
			item.wattack_max = reader.GetInt16(Mabinogi.SQL.Columns.Item.WAttackMax);
			item.balance = reader.GetByte(Mabinogi.SQL.Columns.Item.Balance);
			item.critical = reader.GetByte(Mabinogi.SQL.Columns.Item.Critical);
			item.defence = reader.GetInt32(Mabinogi.SQL.Columns.Item.Defence);
			item.protect = reader.GetInt16(Mabinogi.SQL.Columns.Item.Protect);
			item.effective_range = reader.GetInt16(Mabinogi.SQL.Columns.Item.EffectiveRange);
			item.attack_speed = reader.GetByte(Mabinogi.SQL.Columns.Item.AttackSpeed);
			item.down_hit_count = reader.GetByte(Mabinogi.SQL.Columns.Item.DownHitCount);
			item.experience = reader.GetInt16(Mabinogi.SQL.Columns.Item.Experience);
			item.exp_point = reader.GetByte(Mabinogi.SQL.Columns.Item.ExpPoint);
			item.upgraded = reader.GetByte(Mabinogi.SQL.Columns.Item.Upgraded);
			item.upgrade_max = reader.GetByte(Mabinogi.SQL.Columns.Item.UpgradeMax);
			item.grade = reader.GetByte(Mabinogi.SQL.Columns.Item.Grade);
			item.prefix = reader.GetInt16(Mabinogi.SQL.Columns.Item.Prefix);
			item.suffix = reader.GetInt16(Mabinogi.SQL.Columns.Item.Suffix);
			item.data = reader.GetString(Mabinogi.SQL.Columns.Item.Data);
			item.options = ItemXmlFieldHelper.BuildItemOption(reader.GetString(Mabinogi.SQL.Columns.Item.Option));
			item.sellingprice = reader.GetInt32(Mabinogi.SQL.Columns.Item.SellingPrice);
			item.expiration = reader.GetInt32(Mabinogi.SQL.Columns.Item.Expiration);
			
			
			return item;
		}

		public static Item BuildSmallItem(SimpleReader reader)
		{
			Item item = new Item();
			item.storedtype = 2;
			item.id = reader.GetInt64(Mabinogi.SQL.Columns.Item.ItemId);
            item.Class = reader.GetInt32(Mabinogi.SQL.Columns.Item.Class);
            item.color_01 = reader.GetInt32(Mabinogi.SQL.Columns.Item.Color1);
            item.color_02 = reader.GetInt32(Mabinogi.SQL.Columns.Item.Color2);
            item.color_03 = reader.GetInt32(Mabinogi.SQL.Columns.Item.Color3);
            item.price = reader.GetInt32(Mabinogi.SQL.Columns.Item.Price);
            item.bundle = reader.GetInt16(Mabinogi.SQL.Columns.Item.Bundle);
            item.linked_pocket = (byte)reader.GetInt32(Mabinogi.SQL.Columns.Item.LinkedPocket);
            item.flag = reader.GetByte(Mabinogi.SQL.Columns.Item.Flag);
            item.durability = reader.GetInt32(Mabinogi.SQL.Columns.Item.Durability);
			item.sellingprice = reader.GetInt32(Mabinogi.SQL.Columns.Item.SellingPrice);
            item.expiration = reader.GetInt32(Mabinogi.SQL.Columns.Item.Expiration);

            
            item.data = string.Empty;
			return item;
		}

		public static Item BuildHugeItem(SimpleReader reader)
		{
			Item item = new Item();
			item.storedtype = 3;
            item.id = reader.GetInt64(Mabinogi.SQL.Columns.Item.ItemId);
            item.Class = reader.GetInt32(Mabinogi.SQL.Columns.Item.Class);
            item.color_01 = reader.GetInt32(Mabinogi.SQL.Columns.Item.Color1);
            item.color_02 = reader.GetInt32(Mabinogi.SQL.Columns.Item.Color2);
            item.color_03 = reader.GetInt32(Mabinogi.SQL.Columns.Item.Color3);
            item.price = reader.GetInt32(Mabinogi.SQL.Columns.Item.Price);
            item.bundle = reader.GetInt16(Mabinogi.SQL.Columns.Item.Bundle);
            item.linked_pocket = (byte)reader.GetInt32(Mabinogi.SQL.Columns.Item.LinkedPocket);
			item.flag = reader.GetByte(Mabinogi.SQL.Columns.Item.Flag);
            item.durability = reader.GetInt32(Mabinogi.SQL.Columns.Item.Durability);
            item.durability_max = reader.GetInt32(Mabinogi.SQL.Columns.Item.DurabilityMax);
            item.origin_durability_max = reader.GetInt32(Mabinogi.SQL.Columns.Item.OriginalDurabilityMax);
            item.sellingprice = reader.GetInt32(Mabinogi.SQL.Columns.Item.SellingPrice);
			item.data = reader.GetString(Mabinogi.SQL.Columns.Item.Data);
			item.expiration = reader.GetInt32(Mabinogi.SQL.Columns.Item.Expiration);

            return item;
		}

		public static Item BuildQuestItem(SimpleReader reader)
		{
			Item item = new Item();
			item.quest = new Quest();
			item.storedtype = 4;
			item.id = reader.GetInt64(Mabinogi.SQL.Columns.Item.ItemId);
			item.quest.id = reader.GetInt64(Mabinogi.SQL.Columns.Item.Quest);
            item.Class = reader.GetInt32(Mabinogi.SQL.Columns.Item.Class);
            item.color_01 = reader.GetInt32(Mabinogi.SQL.Columns.Item.Color1);
            item.color_02 = reader.GetInt32(Mabinogi.SQL.Columns.Item.Color2);
            item.color_03 = reader.GetInt32(Mabinogi.SQL.Columns.Item.Color3);
            item.price = reader.GetInt32(Mabinogi.SQL.Columns.Item.Price);
            item.bundle = reader.GetInt16(Mabinogi.SQL.Columns.Item.Bundle);
            item.linked_pocket = (byte)reader.GetInt32(Mabinogi.SQL.Columns.Item.LinkedPocket);
			item.flag = reader.GetByte(Mabinogi.SQL.Columns.Item.Flag);
            item.durability = reader.GetInt32(Mabinogi.SQL.Columns.Item.Durability);
			item.sellingprice = reader.GetInt32(Mabinogi.SQL.Columns.Item.SellingPrice);
			item.quest.templateid = reader.GetInt32(Mabinogi.SQL.Columns.Item.TemplateId);
			item.quest.complete = reader.GetByte(Mabinogi.SQL.Columns.Item.Complete);
			item.quest.start_time = reader.GetInt64(Mabinogi.SQL.Columns.Item.StartTime);
			item.quest.data = reader.GetString(Mabinogi.SQL.Columns.Item.Data);
			item.quest.objectives = ItemXmlFieldHelper.BuildQuestObjective(reader.GetString(Mabinogi.SQL.Columns.Item.Objective));
            item.expiration = reader.GetInt32(Mabinogi.SQL.Columns.Item.Expiration);
            item.data = string.Empty;
			return item;
		}

		public static Item BuildEgoItem(SimpleReader reader)
		{
			Item item = new Item();
			item.ego = new Ego();
			item.storedtype = 5;
            item.id = reader.GetInt64(Mabinogi.SQL.Columns.Item.ItemId);
            item.pocket = (byte)reader.GetInt32(Mabinogi.SQL.Columns.Item.PocketId);
            item.Class = reader.GetInt32(Mabinogi.SQL.Columns.Item.Class);
            item.color_01 = reader.GetInt32(Mabinogi.SQL.Columns.Item.Color1);
            item.color_02 = reader.GetInt32(Mabinogi.SQL.Columns.Item.Color2);
            item.color_03 = reader.GetInt32(Mabinogi.SQL.Columns.Item.Color3);
            item.price = reader.GetInt32(Mabinogi.SQL.Columns.Item.Price);
            item.bundle = reader.GetInt16(Mabinogi.SQL.Columns.Item.Bundle);
            item.linked_pocket = (byte)reader.GetInt32(Mabinogi.SQL.Columns.Item.LinkedPocket);
            item.figure = reader.GetInt32(Mabinogi.SQL.Columns.Item.Figure);
            item.flag = reader.GetByte(Mabinogi.SQL.Columns.Item.Flag);
			item.durability = reader.GetInt32(Mabinogi.SQL.Columns.Item.Durability);
            item.durability_max = reader.GetInt32(Mabinogi.SQL.Columns.Item.DurabilityMax);
            item.origin_durability_max = reader.GetInt32(Mabinogi.SQL.Columns.Item.OriginalDurabilityMax);
            item.attack_min = reader.GetInt16(Mabinogi.SQL.Columns.Item.AttackMin);
            item.attack_max = reader.GetInt16(Mabinogi.SQL.Columns.Item.AttackMax);
            item.wattack_min = reader.GetInt16(Mabinogi.SQL.Columns.Item.WAttackMin);
            item.wattack_max = reader.GetInt16(Mabinogi.SQL.Columns.Item.WAttackMax);
            item.balance = reader.GetByte(Mabinogi.SQL.Columns.Item.Balance);
            item.critical = reader.GetByte(Mabinogi.SQL.Columns.Item.Critical);
            item.defence = reader.GetInt32(Mabinogi.SQL.Columns.Item.Defence);
            item.protect = reader.GetInt16(Mabinogi.SQL.Columns.Item.Protect);
            item.effective_range = reader.GetInt16(Mabinogi.SQL.Columns.Item.EffectiveRange);
            item.attack_speed = reader.GetByte(Mabinogi.SQL.Columns.Item.AttackSpeed);
            item.down_hit_count = reader.GetByte(Mabinogi.SQL.Columns.Item.DownHitCount);
            item.experience = reader.GetInt16(Mabinogi.SQL.Columns.Item.Experience);
            item.exp_point = reader.GetByte(Mabinogi.SQL.Columns.Item.ExpPoint);
            item.upgraded = reader.GetByte(Mabinogi.SQL.Columns.Item.Upgraded);
            item.upgrade_max = reader.GetByte(Mabinogi.SQL.Columns.Item.UpgradeMax);
            item.grade = reader.GetByte(Mabinogi.SQL.Columns.Item.Grade);
            item.prefix = reader.GetInt16(Mabinogi.SQL.Columns.Item.Prefix);
            item.suffix = reader.GetInt16(Mabinogi.SQL.Columns.Item.Suffix);
            item.data = reader.GetString(Mabinogi.SQL.Columns.Item.Data);
            item.options = ItemXmlFieldHelper.BuildItemOption(reader.GetString(Mabinogi.SQL.Columns.Item.Option));
            item.sellingprice = reader.GetInt32(Mabinogi.SQL.Columns.Item.SellingPrice);
            item.expiration = reader.GetInt32(Mabinogi.SQL.Columns.Item.Expiration);
            item.ego.egoName = reader.GetString(Mabinogi.SQL.Columns.Item.EgoName);
			item.ego.egoType = reader.GetByte(Mabinogi.SQL.Columns.Item.EgoType);
			item.ego.egoDesire = reader.GetByte(Mabinogi.SQL.Columns.Item.EgoDesire);
			item.ego.egoSocialLevel = reader.GetByte(Mabinogi.SQL.Columns.Item.EgoStrengthLevel);
			item.ego.egoSocialExp = reader.GetInt32(Mabinogi.SQL.Columns.Item.EgoSocialExp);
			item.ego.egoStrLevel = reader.GetByte(Mabinogi.SQL.Columns.Item.EgoStrengthLevel);
			item.ego.egoStrExp = reader.GetInt32(Mabinogi.SQL.Columns.Item.EgoStrengthExp);
			item.ego.egoIntLevel = reader.GetByte(Mabinogi.SQL.Columns.Item.EgoDexterityLevel);
			item.ego.egoIntExp = reader.GetInt32(Mabinogi.SQL.Columns.Item.EgoIntelligenceExp);
			item.ego.egoDexLevel = reader.GetByte(Mabinogi.SQL.Columns.Item.EgoDexterityLevel);
			item.ego.egoDexExp = reader.GetInt32(Mabinogi.SQL.Columns.Item.EgoDexterityExp);
			item.ego.egoWillLevel = reader.GetByte(Mabinogi.SQL.Columns.Item.EgoWillLevel);
			item.ego.egoWillExp = reader.GetInt32(Mabinogi.SQL.Columns.Item.EgoWillExp);
			item.ego.egoLuckLevel = reader.GetByte(Mabinogi.SQL.Columns.Item.EgoLuckLevel);
			item.ego.egoLuckExp = reader.GetInt32(Mabinogi.SQL.Columns.Item.EgoLuckExp);
			item.ego.egoSkillCount = reader.GetByte(Mabinogi.SQL.Columns.Item.EgoSkillCount);
			item.ego.egoSkillGauge = reader.GetInt32(Mabinogi.SQL.Columns.Item.EgoSkillGauge);
			item.ego.egoSkillCoolTime = reader.GetInt64(Mabinogi.SQL.Columns.Item.EgoSkillCooldown);
			item.varint = reader.GetInt32(Mabinogi.SQL.Columns.CharItem.VarInt);
			return item;
		}
	}
}
