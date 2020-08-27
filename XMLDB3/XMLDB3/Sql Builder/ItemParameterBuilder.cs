using System.Text.Json;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class ItemParameterBuilder
	{
		public static void BuildLargeItem(Item item, SimpleCommand cmd)
		{
            cmd.Set(Mabinogi.SQL.Columns.Item.Class, item.Class);

            cmd.Set(Mabinogi.SQL.Columns.Item.Color1, item.color_01);
            cmd.Set(Mabinogi.SQL.Columns.Item.Color2, item.color_02);
            cmd.Set(Mabinogi.SQL.Columns.Item.Color3, item.color_03);
            cmd.Set(Mabinogi.SQL.Columns.Item.Price, item.price);
            cmd.Set(Mabinogi.SQL.Columns.Item.SellingPrice, item.sellingprice);
            cmd.Set(Mabinogi.SQL.Columns.Item.Bundle, item.bundle);
            cmd.Set(Mabinogi.SQL.Columns.Item.LinkedPocket, item.linked_pocket);
            cmd.Set(Mabinogi.SQL.Columns.Item.Flag, item.flag);
            cmd.Set(Mabinogi.SQL.Columns.Item.Durability, item.durability);

            cmd.Set(Mabinogi.SQL.Columns.Item.Figure, item.figure);

            cmd.Set(Mabinogi.SQL.Columns.Item.DurabilityMax, item.durability_max);

            cmd.Set(Mabinogi.SQL.Columns.Item.OriginalDurabilityMax, item.origin_durability_max);
            cmd.Set(Mabinogi.SQL.Columns.Item.AttackMin, item.attack_min);
            cmd.Set(Mabinogi.SQL.Columns.Item.AttackMax, item.attack_max);
            cmd.Set(Mabinogi.SQL.Columns.Item.WAttackMin, item.wattack_min);
            cmd.Set(Mabinogi.SQL.Columns.Item.WAttackMax, item.wattack_max);
            cmd.Set(Mabinogi.SQL.Columns.Item.Balance, item.balance);
            cmd.Set(Mabinogi.SQL.Columns.Item.Critical, item.critical);

            cmd.Set(Mabinogi.SQL.Columns.Item.Defence, item.defence);
            cmd.Set(Mabinogi.SQL.Columns.Item.Protect, item.protect);
            cmd.Set(Mabinogi.SQL.Columns.Item.EffectiveRange, item.effective_range);
            cmd.Set(Mabinogi.SQL.Columns.Item.AttackSpeed, item.attack_speed);
            cmd.Set(Mabinogi.SQL.Columns.Item.DownHitCount, item.down_hit_count);
            cmd.Set(Mabinogi.SQL.Columns.Item.Experience, item.experience);
            cmd.Set(Mabinogi.SQL.Columns.Item.ExpPoint, item.exp_point);

            cmd.Set(Mabinogi.SQL.Columns.Item.Upgraded, item.upgraded);
            cmd.Set(Mabinogi.SQL.Columns.Item.UpgradeMax, item.upgrade_max);
            cmd.Set(Mabinogi.SQL.Columns.Item.Grade, item.grade);
            cmd.Set(Mabinogi.SQL.Columns.Item.Prefix, item.prefix);
            cmd.Set(Mabinogi.SQL.Columns.Item.Suffix, item.suffix);
            cmd.Set(Mabinogi.SQL.Columns.Item.Data, item.data);
            cmd.Set(Mabinogi.SQL.Columns.Item.Option, JsonSerializer.Serialize(item.options));
            cmd.Set(Mabinogi.SQL.Columns.Item.Expiration, item.expiration);
        }

		public static void BuildSmallItem(Item item, SimpleCommand cmd)
		{
            cmd.Set(Mabinogi.SQL.Columns.Item.Class, item.Class);
            cmd.Set(Mabinogi.SQL.Columns.Item.Color1, item.color_01);
            cmd.Set(Mabinogi.SQL.Columns.Item.Color2, item.color_02);
            cmd.Set(Mabinogi.SQL.Columns.Item.Color3, item.color_03);
            cmd.Set(Mabinogi.SQL.Columns.Item.Price, item.price);
            cmd.Set(Mabinogi.SQL.Columns.Item.SellingPrice, item.sellingprice);
            cmd.Set(Mabinogi.SQL.Columns.Item.Bundle, item.bundle);
            cmd.Set(Mabinogi.SQL.Columns.Item.LinkedPocket, item.linked_pocket);
            cmd.Set(Mabinogi.SQL.Columns.Item.Flag, item.flag);
            cmd.Set(Mabinogi.SQL.Columns.Item.Durability, item.durability);
            cmd.Set(Mabinogi.SQL.Columns.Item.Expiration, item.expiration);
        }

		public static void BuildHugeItem(Item item, SimpleCommand cmd)
		{
            cmd.Set(Mabinogi.SQL.Columns.Item.Class, item.Class);
            cmd.Set(Mabinogi.SQL.Columns.Item.Color1, item.color_01);
            cmd.Set(Mabinogi.SQL.Columns.Item.Color2, item.color_02);
            cmd.Set(Mabinogi.SQL.Columns.Item.Color3, item.color_03);
            cmd.Set(Mabinogi.SQL.Columns.Item.Price, item.price);
            cmd.Set(Mabinogi.SQL.Columns.Item.SellingPrice, item.sellingprice);
            cmd.Set(Mabinogi.SQL.Columns.Item.Bundle, item.bundle);
            cmd.Set(Mabinogi.SQL.Columns.Item.LinkedPocket, item.linked_pocket);
            cmd.Set(Mabinogi.SQL.Columns.Item.Flag, item.flag);
            cmd.Set(Mabinogi.SQL.Columns.Item.Durability, item.durability);
            cmd.Set(Mabinogi.SQL.Columns.Item.DurabilityMax, item.durability_max);

            cmd.Set(Mabinogi.SQL.Columns.Item.OriginalDurabilityMax, item.origin_durability_max);
            cmd.Set(Mabinogi.SQL.Columns.Item.Data, item.data);
            cmd.Set(Mabinogi.SQL.Columns.Item.Expiration, item.expiration);
        }

		public static void BuildQuestItem(Item item, SimpleCommand cmd)
		{
            cmd.Set(Mabinogi.SQL.Columns.Item.Class, item.Class);

            cmd.Set(Mabinogi.SQL.Columns.Item.Color1, item.color_01);
            cmd.Set(Mabinogi.SQL.Columns.Item.Color2, item.color_02);
            cmd.Set(Mabinogi.SQL.Columns.Item.Color3, item.color_03);
            cmd.Set(Mabinogi.SQL.Columns.Item.Price, item.price);
            cmd.Set(Mabinogi.SQL.Columns.Item.SellingPrice, item.sellingprice);
            cmd.Set(Mabinogi.SQL.Columns.Item.Bundle, item.bundle);
            cmd.Set(Mabinogi.SQL.Columns.Item.LinkedPocket, item.linked_pocket);
            cmd.Set(Mabinogi.SQL.Columns.Item.Flag, item.flag);
            cmd.Set(Mabinogi.SQL.Columns.Item.Durability, item.durability);

            cmd.Set(Mabinogi.SQL.Columns.Item.Objective, JsonSerializer.Serialize(item.quest.objectives));

            cmd.Set(Mabinogi.SQL.Columns.Item.Data, item.data);
            cmd.Set(Mabinogi.SQL.Columns.Item.Expiration, item.expiration);
            cmd.Set(Mabinogi.SQL.Columns.Item.Quest, item.quest.id);

            cmd.Set(Mabinogi.SQL.Columns.Item.Complete, item.quest.complete);
            cmd.Set(Mabinogi.SQL.Columns.Item.TemplateId, item.quest.templateid);
            cmd.Set(Mabinogi.SQL.Columns.Item.StartTime, item.quest.start_time);
        }

		public static void BuildEgoItem(Item item, SimpleCommand cmd)
		{
            cmd.Set(Mabinogi.SQL.Columns.Item.Class, item.Class);


            cmd.Set(Mabinogi.SQL.Columns.Item.Color1, item.color_01);
            cmd.Set(Mabinogi.SQL.Columns.Item.Color2, item.color_02);
            cmd.Set(Mabinogi.SQL.Columns.Item.Color3, item.color_03);
            cmd.Set(Mabinogi.SQL.Columns.Item.Price, item.price);
            cmd.Set(Mabinogi.SQL.Columns.Item.SellingPrice, item.sellingprice);
            cmd.Set(Mabinogi.SQL.Columns.Item.Bundle, item.bundle);
            cmd.Set(Mabinogi.SQL.Columns.Item.LinkedPocket, item.linked_pocket);
            cmd.Set(Mabinogi.SQL.Columns.Item.Flag, item.flag);
            cmd.Set(Mabinogi.SQL.Columns.Item.Durability, item.durability);


            cmd.Set(Mabinogi.SQL.Columns.Item.Figure, item.figure);

            cmd.Set(Mabinogi.SQL.Columns.Item.DurabilityMax, item.durability_max);

            cmd.Set(Mabinogi.SQL.Columns.Item.OriginalDurabilityMax, item.origin_durability_max);
            cmd.Set(Mabinogi.SQL.Columns.Item.AttackMin, item.attack_min);
            cmd.Set(Mabinogi.SQL.Columns.Item.AttackMax, item.attack_max);
            cmd.Set(Mabinogi.SQL.Columns.Item.WAttackMin, item.wattack_min);
            cmd.Set(Mabinogi.SQL.Columns.Item.WAttackMax, item.wattack_max);
            cmd.Set(Mabinogi.SQL.Columns.Item.Balance, item.balance);
            cmd.Set(Mabinogi.SQL.Columns.Item.Critical, item.critical);

            cmd.Set(Mabinogi.SQL.Columns.Item.Defence, item.defence);
            cmd.Set(Mabinogi.SQL.Columns.Item.Protect, item.protect);
            cmd.Set(Mabinogi.SQL.Columns.Item.EffectiveRange, item.effective_range);
            cmd.Set(Mabinogi.SQL.Columns.Item.AttackSpeed, item.attack_speed);
            cmd.Set(Mabinogi.SQL.Columns.Item.DownHitCount, item.down_hit_count);
            cmd.Set(Mabinogi.SQL.Columns.Item.Experience, item.experience);
            cmd.Set(Mabinogi.SQL.Columns.Item.ExpPoint, item.exp_point);

            cmd.Set(Mabinogi.SQL.Columns.Item.EgoName, item.ego.egoName);
            cmd.Set(Mabinogi.SQL.Columns.Item.EgoType, item.ego.egoType);
            cmd.Set(Mabinogi.SQL.Columns.Item.EgoDesire, item.ego.egoDesire);
            cmd.Set(Mabinogi.SQL.Columns.Item.EgoSocialLevel, item.ego.egoSocialLevel);
            cmd.Set(Mabinogi.SQL.Columns.Item.EgoSocialExp, item.ego.egoSocialExp);
            cmd.Set(Mabinogi.SQL.Columns.Item.EgoStrengthLevel, item.ego.egoStrLevel);
            cmd.Set(Mabinogi.SQL.Columns.Item.EgoStrengthExp, item.ego.egoStrExp);
            cmd.Set(Mabinogi.SQL.Columns.Item.EgoIntelligenceLevel, item.ego.egoIntLevel);

            cmd.Set(Mabinogi.SQL.Columns.Item.EgoIntelligenceExp, item.ego.egoIntExp);
            cmd.Set(Mabinogi.SQL.Columns.Item.EgoDexterityLevel, item.ego.egoDexLevel);
            cmd.Set(Mabinogi.SQL.Columns.Item.EgoDexterityExp, item.ego.egoDexExp);
            cmd.Set(Mabinogi.SQL.Columns.Item.EgoWillLevel, item.ego.egoWillLevel);
            cmd.Set(Mabinogi.SQL.Columns.Item.EgoWillExp, item.ego.egoWillExp);
            cmd.Set(Mabinogi.SQL.Columns.Item.EgoLuckLevel, item.ego.egoLuckLevel);
            cmd.Set(Mabinogi.SQL.Columns.Item.EgoLuckExp, item.ego.egoLuckExp);

            cmd.Set(Mabinogi.SQL.Columns.Item.EgoSkillCount, item.ego.egoSkillCount);
            cmd.Set(Mabinogi.SQL.Columns.Item.EgoSkillGauge, item.ego.egoSkillGauge);
            cmd.Set(Mabinogi.SQL.Columns.Item.EgoSkillCooldown, item.ego.egoSkillCoolTime);

            cmd.Set(Mabinogi.SQL.Columns.Item.Upgraded, item.upgraded);
            cmd.Set(Mabinogi.SQL.Columns.Item.UpgradeMax, item.upgrade_max);
            cmd.Set(Mabinogi.SQL.Columns.Item.Grade, item.grade);
            cmd.Set(Mabinogi.SQL.Columns.Item.Prefix, item.prefix);
            cmd.Set(Mabinogi.SQL.Columns.Item.Suffix, item.suffix);
            cmd.Set(Mabinogi.SQL.Columns.Item.Data, item.data);
            cmd.Set(Mabinogi.SQL.Columns.Item.Option, JsonSerializer.Serialize(item.options));
            cmd.Set(Mabinogi.SQL.Columns.Item.Expiration, item.expiration);
        }
	}
}
