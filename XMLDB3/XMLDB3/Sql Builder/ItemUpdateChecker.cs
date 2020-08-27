namespace XMLDB3
{
	public class ItemUpdateChecker
	{
		public static bool CheckLargeItem(Item _new, Item _old)
		{
			if (_new.pocket != _old.pocket)
			{
				return true;
			}
			if (_new.Class != _old.Class)
			{
				return true;
			}
			if (_new.pos_x != _old.pos_x)
			{
				return true;
			}
			if (_new.pos_y != _old.pos_y)
			{
				return true;
			}
			if (_new.varint != _old.varint)
			{
				return true;
			}
			if (_new.color_01 != _old.color_01)
			{
				return true;
			}
			if (_new.color_02 != _old.color_02)
			{
				return true;
			}
			if (_new.color_03 != _old.color_03)
			{
				return true;
			}
			if (_new.price != _old.price)
			{
				return true;
			}
			if (_new.sellingprice != _old.sellingprice)
			{
				return true;
			}
			if (_new.bundle != _old.bundle)
			{
				return true;
			}
			if (_new.linked_pocket != _old.linked_pocket)
			{
				return true;
			}
			if (_new.figure != _old.figure)
			{
				return true;
			}
			if (_new.flag != _old.flag)
			{
				return true;
			}
			if (_new.durability != _old.durability)
			{
				return true;
			}
			if (_new.durability_max != _old.durability_max)
			{
				return true;
			}
			if (_new.origin_durability_max != _old.origin_durability_max)
			{
				return true;
			}
			if (_new.attack_min != _old.attack_min)
			{
				return true;
			}
			if (_new.attack_max != _old.attack_max)
			{
				return true;
			}
			if (_new.wattack_min != _old.wattack_min)
			{
				return true;
			}
			if (_new.wattack_max != _old.wattack_max)
			{
				return true;
			}
			if (_new.balance != _old.balance)
			{
				return true;
			}
			if (_new.critical != _old.critical)
			{
				return true;
			}
			if (_new.defence != _old.defence)
			{
				return true;
			}
			if (_new.protect != _old.protect)
			{
				return true;
			}
			if (_new.effective_range != _old.effective_range)
			{
				return true;
			}
			if (_new.attack_speed != _old.attack_speed)
			{
				return true;
			}
			if (_new.down_hit_count != _old.down_hit_count)
			{
				return true;
			}
			if (_new.experience != _old.experience)
			{
				return true;
			}
			if (_new.exp_point != _old.exp_point)
			{
				return true;
			}
			if (_new.upgraded != _old.upgraded)
			{
				return true;
			}
			if (_new.upgrade_max != _old.upgrade_max)
			{
				return true;
			}
			if (_new.grade != _old.grade)
			{
				return true;
			}
			if (_new.prefix != _old.prefix)
			{
				return true;
			}
			if (_new.suffix != _old.suffix)
			{
				return true;
			}
			if (_new.data != _old.data)
			{
				return true;
			}
			string a = ItemXmlFieldHelper.BuildItemOptionXml(_old.options);
			string b = ItemXmlFieldHelper.BuildItemOptionXml(_new.options);
			if (a != b)
			{
				return true;
			}
			if (_new.expiration != _old.expiration)
			{
				return true;
			}
			return false;
		}

		public static bool CheckSmallItem(Item _new, Item _old)
		{
			if (_new.pocket != _old.pocket)
			{
				return true;
			}
			if (_new.Class != _old.Class)
			{
				return true;
			}
			if (_new.pos_x != _old.pos_x)
			{
				return true;
			}
			if (_new.pos_y != _old.pos_y)
			{
				return true;
			}
			if (_new.varint != _old.varint)
			{
				return true;
			}
			if (_new.color_01 != _old.color_01)
			{
				return true;
			}
			if (_new.color_02 != _old.color_02)
			{
				return true;
			}
			if (_new.color_03 != _old.color_03)
			{
				return true;
			}
			if (_new.price != _old.price)
			{
				return true;
			}
			if (_new.sellingprice != _old.sellingprice)
			{
				return true;
			}
			if (_new.bundle != _old.bundle)
			{
				return true;
			}
			if (_new.linked_pocket != _old.linked_pocket)
			{
				return true;
			}
			if (_new.flag != _old.flag)
			{
				return true;
			}
			if (_new.durability != _old.durability)
			{
				return true;
			}
			if (_new.expiration != _old.expiration)
			{
				return true;
			}
			return false;
		}

		public static bool CheckHugeItem(Item _new, Item _old)
		{
			if (_new.pocket != _old.pocket)
			{
				return true;
			}
			if (_new.Class != _old.Class)
			{
				return true;
			}
			if (_new.pos_x != _old.pos_x)
			{
				return true;
			}
			if (_new.pos_y != _old.pos_y)
			{
				return true;
			}
			if (_new.varint != _old.varint)
			{
				return true;
			}
			if (_new.color_01 != _old.color_01)
			{
				return true;
			}
			if (_new.color_02 != _old.color_02)
			{
				return true;
			}
			if (_new.color_03 != _old.color_03)
			{
				return true;
			}
			if (_new.price != _old.price)
			{
				return true;
			}
			if (_new.sellingprice != _old.sellingprice)
			{
				return true;
			}
			if (_new.bundle != _old.bundle)
			{
				return true;
			}
			if (_new.linked_pocket != _old.linked_pocket)
			{
				return true;
			}
			if (_new.flag != _old.flag)
			{
				return true;
			}
			if (_new.durability != _old.durability)
			{
				return true;
			}
			if (_new.durability_max != _old.durability_max)
			{
				return true;
			}
			if (_new.origin_durability_max != _old.origin_durability_max)
			{
				return true;
			}
			if (_new.data != _old.data)
			{
				return true;
			}
			if (_new.expiration != _old.expiration)
			{
				return true;
			}
			return false;
		}

		public static bool CheckQuestItem(Item _new, Item _old)
		{
			if (_new.pocket != _old.pocket)
			{
				return true;
			}
			if (_new.Class != _old.Class)
			{
				return true;
			}
			if (_new.pos_x != _old.pos_x)
			{
				return true;
			}
			if (_new.pos_y != _old.pos_y)
			{
				return true;
			}
			if (_new.varint != _old.varint)
			{
				return true;
			}
			if (_new.color_01 != _old.color_01)
			{
				return true;
			}
			if (_new.color_02 != _old.color_02)
			{
				return true;
			}
			if (_new.color_03 != _old.color_03)
			{
				return true;
			}
			if (_new.price != _old.price)
			{
				return true;
			}
			if (_new.sellingprice != _old.sellingprice)
			{
				return true;
			}
			if (_new.bundle != _old.bundle)
			{
				return true;
			}
			if (_new.linked_pocket != _old.linked_pocket)
			{
				return true;
			}
			if (_new.flag != _old.flag)
			{
				return true;
			}
			if (_new.durability != _old.durability)
			{
				return true;
			}
			if (_new.expiration != _old.expiration)
			{
				return true;
			}
			if (_new.quest.id != _old.quest.id)
			{
				return true;
			}
			if (_new.quest.templateid != _old.quest.templateid)
			{
				return true;
			}
			if (_new.quest.complete != _old.quest.complete)
			{
				return true;
			}
			if (_new.quest.start_time != _old.quest.start_time)
			{
				return true;
			}
			if (_new.quest.data != _old.quest.data)
			{
				return true;
			}
			string a = ItemXmlFieldHelper.BuildQuestObjectiveXml(_new.quest.objectives);
			string b = ItemXmlFieldHelper.BuildQuestObjectiveXml(_old.quest.objectives);
			if (a != b)
			{
				return true;
			}
			return false;
		}

		public static bool CheckEgoItem(Item _new, Item _old)
		{
			if (_new.pocket != _old.pocket)
			{
				return true;
			}
			if (_new.Class != _old.Class)
			{
				return true;
			}
			if (_new.pos_x != _old.pos_x)
			{
				return true;
			}
			if (_new.pos_y != _old.pos_y)
			{
				return true;
			}
			if (_new.varint != _old.varint)
			{
				return true;
			}
			if (_new.color_01 != _old.color_01)
			{
				return true;
			}
			if (_new.color_02 != _old.color_02)
			{
				return true;
			}
			if (_new.color_03 != _old.color_03)
			{
				return true;
			}
			if (_new.price != _old.price)
			{
				return true;
			}
			if (_new.sellingprice != _old.sellingprice)
			{
				return true;
			}
			if (_new.bundle != _old.bundle)
			{
				return true;
			}
			if (_new.linked_pocket != _old.linked_pocket)
			{
				return true;
			}
			if (_new.figure != _old.figure)
			{
				return true;
			}
			if (_new.flag != _old.flag)
			{
				return true;
			}
			if (_new.durability != _old.durability)
			{
				return true;
			}
			if (_new.durability_max != _old.durability_max)
			{
				return true;
			}
			if (_new.origin_durability_max != _old.origin_durability_max)
			{
				return true;
			}
			if (_new.attack_min != _old.attack_min)
			{
				return true;
			}
			if (_new.attack_max != _old.attack_max)
			{
				return true;
			}
			if (_new.wattack_min != _old.wattack_min)
			{
				return true;
			}
			if (_new.wattack_max != _old.wattack_max)
			{
				return true;
			}
			if (_new.balance != _old.balance)
			{
				return true;
			}
			if (_new.critical != _old.critical)
			{
				return true;
			}
			if (_new.defence != _old.defence)
			{
				return true;
			}
			if (_new.protect != _old.protect)
			{
				return true;
			}
			if (_new.effective_range != _old.effective_range)
			{
				return true;
			}
			if (_new.attack_speed != _old.attack_speed)
			{
				return true;
			}
			if (_new.down_hit_count != _old.down_hit_count)
			{
				return true;
			}
			if (_new.experience != _old.experience)
			{
				return true;
			}
			if (_new.exp_point != _old.exp_point)
			{
				return true;
			}
			if (_new.upgraded != _old.upgraded)
			{
				return true;
			}
			if (_new.upgrade_max != _old.upgrade_max)
			{
				return true;
			}
			if (_new.grade != _old.grade)
			{
				return true;
			}
			if (_new.prefix != _old.prefix)
			{
				return true;
			}
			if (_new.suffix != _old.suffix)
			{
				return true;
			}
			if (_new.data != _old.data)
			{
				return true;
			}
			string a = ItemXmlFieldHelper.BuildItemOptionXml(_old.options);
			string b = ItemXmlFieldHelper.BuildItemOptionXml(_new.options);
			if (a != b)
			{
				return true;
			}
			if (_new.expiration != _old.expiration)
			{
				return true;
			}
			if (_new.ego.egoName != _old.ego.egoName)
			{
				return true;
			}
			if (_new.ego.egoType != _old.ego.egoType)
			{
				return true;
			}
			if (_new.ego.egoDesire != _old.ego.egoDesire)
			{
				return true;
			}
			if (_new.ego.egoSocialLevel != _old.ego.egoSocialLevel)
			{
				return true;
			}
			if (_new.ego.egoSocialExp != _old.ego.egoSocialExp)
			{
				return true;
			}
			if (_new.ego.egoStrLevel != _old.ego.egoStrLevel)
			{
				return true;
			}
			if (_new.ego.egoStrExp != _old.ego.egoStrExp)
			{
				return true;
			}
			if (_new.ego.egoIntLevel != _old.ego.egoIntLevel)
			{
				return true;
			}
			if (_new.ego.egoIntExp != _old.ego.egoIntExp)
			{
				return true;
			}
			if (_new.ego.egoDexLevel != _old.ego.egoDexLevel)
			{
				return true;
			}
			if (_new.ego.egoDexExp != _old.ego.egoDexExp)
			{
				return true;
			}
			if (_new.ego.egoWillLevel != _old.ego.egoWillLevel)
			{
				return true;
			}
			if (_new.ego.egoWillExp != _old.ego.egoWillExp)
			{
				return true;
			}
			if (_new.ego.egoLuckLevel != _old.ego.egoLuckLevel)
			{
				return true;
			}
			if (_new.ego.egoLuckExp != _old.ego.egoLuckExp)
			{
				return true;
			}
			if (_new.ego.egoSkillCount != _old.ego.egoSkillCount)
			{
				return true;
			}
			if (_new.ego.egoSkillGauge != _old.ego.egoSkillGauge)
			{
				return true;
			}
			if (_new.ego.egoSkillCoolTime != _old.ego.egoSkillCoolTime)
			{
				return true;
			}
			return false;
		}
	}
}
