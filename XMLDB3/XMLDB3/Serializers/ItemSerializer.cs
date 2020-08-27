using Mabinogi;
using System;
using System.Text.Json;

namespace XMLDB3
{
	public class ItemSerializer
	{
		public static Item Serialize(Message _message)
		{
			Item item = new Item();
			item.id = _message.ReadS64();
			item.storedtype = _message.ReadU8();
			if (item.storedtype <= 0 || item.storedtype >= 6)
			{
				MailSender.Send("Item 의 storedtype이 잘못되었습니다", JsonSerializer.Serialize(item));
				throw new Exception("Invalid item storedtype\n");
			}
			item.pocket = _message.ReadU8();
			item.Class = _message.ReadS32();
			item.pos_x = _message.ReadS32();
			item.pos_y = _message.ReadS32();
			item.color_01 = _message.ReadS32();
			item.color_02 = _message.ReadS32();
			item.color_03 = _message.ReadS32();
			item.price = _message.ReadS32();
			item.sellingprice = _message.ReadS32();
			item.bundle = _message.ReadS16();
			item.linked_pocket = _message.ReadU8();
			item.figure = _message.ReadS32();
			item.flag = _message.ReadU8();
			item.durability = _message.ReadS32();
			item.durability_max = _message.ReadS32();
			item.origin_durability_max = _message.ReadS32();
			item.attack_min = _message.ReadS16();
			item.attack_max = _message.ReadS16();
			item.wattack_min = _message.ReadS16();
			item.wattack_max = _message.ReadS16();
			item.balance = _message.ReadU8();
			item.critical = _message.ReadU8();
			item.defence = _message.ReadS32();
			item.protect = _message.ReadS16();
			item.effective_range = _message.ReadS16();
			item.attack_speed = _message.ReadU8();
			item.down_hit_count = _message.ReadU8();
			item.experience = _message.ReadS16();
			item.exp_point = _message.ReadU8();
			item.upgraded = _message.ReadU8();
			item.upgrade_max = _message.ReadU8();
			item.grade = _message.ReadU8();
			item.prefix = _message.ReadS16();
			item.suffix = _message.ReadS16();
			item.data = _message.ReadString();
			uint num = _message.ReadU32();
			item.options = new ItemOption[num];
			for (int i = 0; i < num; i++)
			{
				ItemOption itemOption = item.options[i] = new ItemOption();
				itemOption.type = _message.ReadU8();
				itemOption.flag = _message.ReadS32();
				itemOption.execute = _message.ReadS16();
				itemOption.execdata = _message.ReadS64();
				itemOption.open = _message.ReadU8();
				itemOption.opendata = _message.ReadS32();
				itemOption.enable = _message.ReadU8();
				itemOption.enabledata = _message.ReadS32();
			}
			item.expiration = _message.ReadS32();
			item.varint = _message.ReadS32();
			if (item.Type == Item.StoredType.IstEgo)
			{
				item.ego = ReadEgoFromMessage(_message);
			}
			else
			{
				item.ego = null;
			}
			item.quest = QuestSerializer.Serialize(_message);
			return item;
		}

		private static Ego ReadEgoFromMessage(Message _message)
		{
			Ego ego = new Ego();
			ego.egoName = _message.ReadString();
			ego.egoType = _message.ReadU8();
			ego.egoDesire = _message.ReadU8();
			ego.egoSocialLevel = _message.ReadU8();
			ego.egoSocialExp = _message.ReadS32();
			ego.egoStrLevel = _message.ReadU8();
			ego.egoStrExp = _message.ReadS32();
			ego.egoIntLevel = _message.ReadU8();
			ego.egoIntExp = _message.ReadS32();
			ego.egoDexLevel = _message.ReadU8();
			ego.egoDexExp = _message.ReadS32();
			ego.egoWillLevel = _message.ReadU8();
			ego.egoWillExp = _message.ReadS32();
			ego.egoLuckLevel = _message.ReadU8();
			ego.egoLuckExp = _message.ReadS32();
			ego.egoSkillCount = _message.ReadU8();
			ego.egoSkillGauge = _message.ReadS32();
			ego.egoSkillCoolTime = _message.ReadS64();
			return ego;
		}

		public static Message Deserialize(Item _item, Message _message)
		{
			if (_item == null)
			{
				_item = new Item();
			}
			_message.WriteS64(_item.id);
			_message.WriteU8(_item.storedtype);
			_message.WriteU8(_item.pocket);
			_message.WriteS32(_item.Class);
			_message.WriteS32(_item.pos_x);
			_message.WriteS32(_item.pos_y);
			_message.WriteS32(_item.color_01);
			_message.WriteS32(_item.color_02);
			_message.WriteS32(_item.color_03);
			_message.WriteS32(_item.price);
			_message.WriteS32(_item.sellingprice);
			_message.WriteS16(_item.bundle);
			_message.WriteU8(_item.linked_pocket);
			_message.WriteS32(_item.figure);
			_message.WriteU8(_item.flag);
			_message.WriteS32(_item.durability);
			_message.WriteS32(_item.durability_max);
			_message.WriteS32(_item.origin_durability_max);
			_message.WriteS16(_item.attack_min);
			_message.WriteS16(_item.attack_max);
			_message.WriteS16(_item.wattack_min);
			_message.WriteS16(_item.wattack_max);
			_message.WriteU8(_item.balance);
			_message.WriteU8(_item.critical);
			_message.WriteS32(_item.defence);
			_message.WriteS16(_item.protect);
			_message.WriteS16(_item.effective_range);
			_message.WriteU8(_item.attack_speed);
			_message.WriteU8(_item.down_hit_count);
			_message.WriteS16(_item.experience);
			_message.WriteU8(_item.exp_point);
			_message.WriteU8(_item.upgraded);
			_message.WriteU8(_item.upgrade_max);
			_message.WriteU8(_item.grade);
			_message.WriteS16(_item.prefix);
			_message.WriteS16(_item.suffix);
			_message.WriteString(_item.data);
			if (_item.options != null)
			{
				_message.WriteU32((uint)_item.options.Length);
				ItemOption[] options = _item.options;
				foreach (ItemOption itemOption in options)
				{
					_message.WriteU8(itemOption.type);
					_message.WriteS32(itemOption.flag);
					_message.WriteS16(itemOption.execute);
					_message.WriteS64(itemOption.execdata);
					_message.WriteU8(itemOption.open);
					_message.WriteS32(itemOption.opendata);
					_message.WriteU8(itemOption.enable);
					_message.WriteS32(itemOption.enabledata);
				}
			}
			else
			{
				_message.WriteU32(0u);
			}
			_message.WriteS32(_item.expiration);
			_message.WriteS32(_item.varint);
			if (_item.Type == Item.StoredType.IstEgo)
			{
				WriteEgoToMessage(_item.ego, _message);
			}
			QuestSerializer.Deserialize(_item.quest, _message);
			return _message;
		}

		private static void WriteEgoToMessage(Ego _item, Message _message)
		{
			if (_item == null)
			{
				_item = new Ego();
			}
			_message.WriteString(_item.egoName);
			_message.WriteU8(_item.egoType);
			_message.WriteU8(_item.egoDesire);
			_message.WriteU8(_item.egoSocialLevel);
			_message.WriteS32(_item.egoSocialExp);
			_message.WriteU8(_item.egoStrLevel);
			_message.WriteS32(_item.egoStrExp);
			_message.WriteU8(_item.egoIntLevel);
			_message.WriteS32(_item.egoIntExp);
			_message.WriteU8(_item.egoDexLevel);
			_message.WriteS32(_item.egoDexExp);
			_message.WriteU8(_item.egoWillLevel);
			_message.WriteS32(_item.egoWillExp);
			_message.WriteU8(_item.egoLuckLevel);
			_message.WriteS32(_item.egoLuckExp);
			_message.WriteU8(_item.egoSkillCount);
			_message.WriteS32(_item.egoSkillGauge);
			_message.WriteS64(_item.egoSkillCoolTime);
		}
	}
}
