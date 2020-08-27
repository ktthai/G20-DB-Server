using Mabinogi;
using System;

namespace XMLDB3
{
	public class PetSerializer
	{
		public static PetInfo Serialize(Message _message)
		{
			PetInfo petInfo = new PetInfo();
			petInfo.id = _message.ReadS64();
			petInfo.name = _message.ReadString();
			petInfo.appearance = ReadAppearanceFromMessage(_message);
			petInfo.parameter = ReadParameterFromMessage(_message);
			petInfo.parameterEx = ReadParameterExFromMessage(_message);
			petInfo.inventory = InventorySerializer.Serialize(_message);
			int num = _message.ReadS32();
			if (num > 0)
			{
				petInfo.skills = new PetSkill[num];
				for (int i = 0; i < num; i++)
				{
					petInfo.skills[i] = ReadSkillFromMessage(_message);
				}
			}
			else
			{
				petInfo.skills = null;
			}
			petInfo.conditions = ReadConditionFromMessage(_message);
			num = _message.ReadS32();
			if (num > 0)
			{
				petInfo.memorys = new PetMemory[num];
				for (int j = 0; j < num; j++)
				{
					petInfo.memorys[j] = ReadMemoryFromMessage(_message);
				}
			}
			else
			{
				petInfo.memorys = null;
			}
			petInfo.data = ReadDataFromMessage(_message);
			petInfo.@private = ReadQuestFromMessage(_message);
			petInfo.summon = ReadSummonFromMessage(_message);
			petInfo.macroChecker = ReadMacroCheckerFromMessage(_message);
			return petInfo;
		}

		private static PetAppearance ReadAppearanceFromMessage(Message _message)
		{
			PetAppearance petAppearance = new PetAppearance();
			petAppearance.type = _message.ReadS32();
			petAppearance.skin_color = _message.ReadU8();
			petAppearance.eye_type = _message.ReadS16();
			if (petAppearance.eye_type < 0)
			{
				petAppearance.eye_type = 0;
			}
			petAppearance.eye_color = _message.ReadU8();
			petAppearance.mouth_type = _message.ReadU8();
			petAppearance.status = _message.ReadS32();
			petAppearance.height = _message.ReadFloat();
			petAppearance.fatness = _message.ReadFloat();
			petAppearance.upper = _message.ReadFloat();
			petAppearance.lower = _message.ReadFloat();
			petAppearance.region = _message.ReadS32();
			petAppearance.x = _message.ReadS32();
			petAppearance.y = _message.ReadS32();
			petAppearance.direction = _message.ReadU8();
			petAppearance.battle_state = _message.ReadS32();
			petAppearance.extra_01 = _message.ReadS32();
			petAppearance.extra_02 = _message.ReadS32();
			petAppearance.extra_03 = _message.ReadS32();
			return petAppearance;
		}

		private static PetParameter ReadParameterFromMessage(Message _message)
		{
			PetParameter petParameter = new PetParameter();
			petParameter.life = _message.ReadFloat();
			petParameter.life_damage = _message.ReadFloat();
			petParameter.life_max = _message.ReadFloat();
			petParameter.mana = _message.ReadFloat();
			petParameter.mana_max = _message.ReadFloat();
			petParameter.stamina = _message.ReadFloat();
			petParameter.stamina_max = _message.ReadFloat();
			petParameter.food = _message.ReadFloat();
			petParameter.level = _message.ReadS16();
			petParameter.cumulatedlevel = _message.ReadS32();
			petParameter.maxlevel = _message.ReadS16();
			petParameter.rebirthcount = _message.ReadS16();
			petParameter.experience = _message.ReadS64();
			petParameter.age = _message.ReadS16();
			petParameter.strength = _message.ReadFloat();
			petParameter.dexterity = _message.ReadFloat();
			petParameter.intelligence = _message.ReadFloat();
			petParameter.will = _message.ReadFloat();
			petParameter.luck = _message.ReadFloat();
			petParameter.attack_min = _message.ReadS16();
			petParameter.attack_max = _message.ReadS16();
			petParameter.wattack_min = _message.ReadS16();
			petParameter.wattack_max = _message.ReadS16();
			petParameter.critical = _message.ReadFloat();
			petParameter.protect = _message.ReadFloat();
			petParameter.defense = _message.ReadS16();
			petParameter.rate = _message.ReadS16();
			return petParameter;
		}

		private static PetParameterEx ReadParameterExFromMessage(Message _message)
		{
			PetParameterEx petParameterEx = new PetParameterEx();
			petParameterEx.str_boost = _message.ReadU8();
			petParameterEx.dex_boost = _message.ReadU8();
			petParameterEx.int_boost = _message.ReadU8();
			petParameterEx.will_boost = _message.ReadU8();
			petParameterEx.luck_boost = _message.ReadU8();
			petParameterEx.height_boost = _message.ReadU8();
			petParameterEx.fatness_boost = _message.ReadU8();
			petParameterEx.upper_boost = _message.ReadU8();
			petParameterEx.lower_boost = _message.ReadU8();
			petParameterEx.life_boost = _message.ReadU8();
			petParameterEx.mana_boost = _message.ReadU8();
			petParameterEx.stamina_boost = _message.ReadU8();
			petParameterEx.toxic = _message.ReadFloat();
			petParameterEx.toxic_drunken_time = _message.ReadS64();
			petParameterEx.toxic_str = _message.ReadFloat();
			petParameterEx.toxic_int = _message.ReadFloat();
			petParameterEx.toxic_dex = _message.ReadFloat();
			petParameterEx.toxic_will = _message.ReadFloat();
			petParameterEx.toxic_luck = _message.ReadFloat();
			petParameterEx.lasttown = _message.ReadString();
			petParameterEx.lastdungeon = _message.ReadString();
			return petParameterEx;
		}

		private static PetSkill ReadSkillFromMessage(Message _message)
		{
			PetSkill petSkill = new PetSkill();
			petSkill.id = _message.ReadS16();
			petSkill.level = _message.ReadU8();
			petSkill.flag = _message.ReadS16();
			return petSkill;
		}

		private static PetCondition[] ReadConditionFromMessage(Message _message)
		{
			int num = _message.ReadS32();
			if (num > 0)
			{
				PetCondition[] array = new PetCondition[num];
				for (int i = 0; i < num; i++)
				{
					PetCondition petCondition = new PetCondition();
					petCondition.flag = _message.ReadS32();
					petCondition.timemode = _message.ReadS8();
					petCondition.time = _message.ReadS64();
					petCondition.meta = _message.ReadString();
					if (petCondition.meta.Length == 0)
					{
						petCondition.meta = null;
					}
					array[i] = petCondition;
				}
				return array;
			}
			return null;
		}

		private static PetMemory ReadMemoryFromMessage(Message _message)
		{
			PetMemory petMemory = new PetMemory();
			petMemory.target = _message.ReadString();
			petMemory.favor = _message.ReadU8();
			petMemory.memory = _message.ReadU8();
			petMemory.time_stamp = _message.ReadU8();
			return petMemory;
		}

		private static PetPrivate ReadQuestFromMessage(Message _message)
		{
			PetPrivate petPrivate = new PetPrivate();
			petPrivate.reserveds = ReadQuestReservedFromMessage(_message);
			petPrivate.registereds = ReadQuestRegisteredFromMessage(_message);
			return petPrivate;
		}

		private static PetPrivateReserved[] ReadQuestReservedFromMessage(Message _message)
		{
			int num = _message.ReadS32();
			if (num > 0)
			{
				PetPrivateReserved[] array = new PetPrivateReserved[num];
				for (int i = 0; i < num; i++)
				{
					PetPrivateReserved petPrivateReserved = new PetPrivateReserved();
					petPrivateReserved.id = _message.ReadS32();
					petPrivateReserved.rate = _message.ReadS32();
					array[i] = petPrivateReserved;
				}
				return array;
			}
			return null;
		}

		private static PetPrivateRegistered[] ReadQuestRegisteredFromMessage(Message _message)
		{
			int num = _message.ReadS32();
			if (num > 0)
			{
				PetPrivateRegistered[] array = new PetPrivateRegistered[num];
				for (int i = 0; i < num; i++)
				{
					PetPrivateRegistered petPrivateRegistered = new PetPrivateRegistered();
					petPrivateRegistered.id = _message.ReadS32();
					petPrivateRegistered.start = _message.ReadS64();
					petPrivateRegistered.end = _message.ReadS64();
					petPrivateRegistered.extra = _message.ReadS32();
					array[i] = petPrivateRegistered;
				}
				return array;
			}
			return null;
		}

		private static PetData ReadDataFromMessage(Message _message)
		{
			PetData petData = new PetData();
			petData.ui = _message.ReadString();
			petData.meta = _message.ReadString();
			petData.birthday = new DateTime(_message.ReadS64());
			petData.rebirthday = new DateTime(_message.ReadS64());
			petData.rebirthage = _message.ReadS16();
			petData.playtime = _message.ReadS32();
			petData.wealth = _message.ReadS32();
			petData.writeCounter = _message.ReadU8();
			return petData;
		}

		private static PetSummon ReadSummonFromMessage(Message _message)
		{
			PetSummon petSummon = new PetSummon();
			petSummon.remaintime = _message.ReadS32();
			petSummon.lasttime = _message.ReadS64();
			petSummon.expiretime = _message.ReadS64();
			petSummon.loyalty = _message.ReadU8();
			petSummon.favor = _message.ReadU8();
			return petSummon;
		}

		private static PetMacroChecker ReadMacroCheckerFromMessage(Message _message)
		{
			PetMacroChecker petMacroChecker = new PetMacroChecker();
			petMacroChecker.macroPoint = _message.ReadS32();
			return petMacroChecker;
		}

		public static void Deserialize(PetInfo _info, Message _message)
		{
			WritePetToMessage(_info, _message);
		}

		private static void WritePetToMessage(PetInfo _pet, Message _message)
		{
			_message.WriteS64(_pet.id);
			_message.WriteString(_pet.name);
			WriteAppearanceToMessage(_pet.appearance, _message);
			WriteParameterToMessage(_pet.parameter, _message);
			WriteParameterExToMessage(_pet.parameterEx, _message);
			InventorySerializer.Deserialize(_pet.inventory, _message);
			if (_pet.skills != null)
			{
				_message.WriteS32(_pet.skills.Length);
				PetSkill[] skills = _pet.skills;
				foreach (PetSkill skill in skills)
				{
					WriteSkillToMessage(skill, _message);
				}
			}
			else
			{
				_message.WriteU32(0u);
			}
			WriteConditionsToMessage(_pet.conditions, _message);
			if (_pet.memorys != null)
			{
				_message.WriteS32(_pet.memorys.Length);
				PetMemory[] memorys = _pet.memorys;
				foreach (PetMemory memory in memorys)
				{
					WriteMemoryToMessage(memory, _message);
				}
			}
			else
			{
				_message.WriteS32(0);
			}
			WriteDataToMessage(_pet.data, _message);
			WriteQuestToMessage(_pet.@private, _message);
			WriteSummonToMessage(_pet.summon, _message);
			WriteMacroCheckerToMessage(_pet.macroChecker, _message);
		}

		private static void WriteAppearanceToMessage(PetAppearance _appearance, Message _message)
		{
			if (_appearance == null)
			{
				_appearance = new PetAppearance();
			}
			_message.WriteS32(_appearance.type);
			_message.WriteU8(_appearance.skin_color);
			if (_appearance.eye_type < 0)
			{
				_appearance.eye_type = 0;
			}
			_message.WriteS16(_appearance.eye_type);
			_message.WriteU8(_appearance.eye_color);
			_message.WriteU8(_appearance.mouth_type);
			_message.WriteS32(_appearance.status);
			_message.WriteFloat(_appearance.height);
			_message.WriteFloat(_appearance.fatness);
			_message.WriteFloat(_appearance.upper);
			_message.WriteFloat(_appearance.lower);
			_message.WriteS32(_appearance.region);
			_message.WriteS32(_appearance.x);
			_message.WriteS32(_appearance.y);
			_message.WriteU8(_appearance.direction);
			_message.WriteS32(_appearance.battle_state);
			_message.WriteS32(_appearance.extra_01);
			_message.WriteS32(_appearance.extra_02);
			_message.WriteS32(_appearance.extra_03);
		}

		private static void WriteParameterToMessage(PetParameter _parameter, Message _message)
		{
			if (_parameter == null)
			{
				_parameter = new PetParameter();
			}
			_message.WriteFloat(_parameter.life);
			_message.WriteFloat(_parameter.life_damage);
			_message.WriteFloat(_parameter.life_max);
			_message.WriteFloat(_parameter.mana);
			_message.WriteFloat(_parameter.mana_max);
			_message.WriteFloat(_parameter.stamina);
			_message.WriteFloat(_parameter.stamina_max);
			_message.WriteFloat(_parameter.food);
			_message.WriteS16(_parameter.level);
			_message.WriteS32(_parameter.cumulatedlevel);
			_message.WriteS16(_parameter.maxlevel);
			_message.WriteS16(_parameter.rebirthcount);
			_message.WriteS64(_parameter.experience);
			_message.WriteS16(_parameter.age);
			_message.WriteFloat(_parameter.strength);
			_message.WriteFloat(_parameter.dexterity);
			_message.WriteFloat(_parameter.intelligence);
			_message.WriteFloat(_parameter.will);
			_message.WriteFloat(_parameter.luck);
			_message.WriteS16(_parameter.attack_min);
			_message.WriteS16(_parameter.attack_max);
			_message.WriteS16(_parameter.wattack_min);
			_message.WriteS16(_parameter.wattack_max);
			_message.WriteFloat(_parameter.critical);
			_message.WriteFloat(_parameter.protect);
			_message.WriteS16(_parameter.defense);
			_message.WriteS16(_parameter.rate);
		}

		private static void WriteParameterExToMessage(PetParameterEx _parameterEx, Message _message)
		{
			if (_parameterEx == null)
			{
				_parameterEx = new PetParameterEx();
			}
			_message.WriteU8(_parameterEx.str_boost);
			_message.WriteU8(_parameterEx.dex_boost);
			_message.WriteU8(_parameterEx.int_boost);
			_message.WriteU8(_parameterEx.will_boost);
			_message.WriteU8(_parameterEx.luck_boost);
			_message.WriteU8(_parameterEx.height_boost);
			_message.WriteU8(_parameterEx.fatness_boost);
			_message.WriteU8(_parameterEx.upper_boost);
			_message.WriteU8(_parameterEx.lower_boost);
			_message.WriteU8(_parameterEx.life_boost);
			_message.WriteU8(_parameterEx.mana_boost);
			_message.WriteU8(_parameterEx.stamina_boost);
			_message.WriteFloat(_parameterEx.toxic);
			_message.WriteS64(_parameterEx.toxic_drunken_time);
			_message.WriteFloat(_parameterEx.toxic_str);
			_message.WriteFloat(_parameterEx.toxic_int);
			_message.WriteFloat(_parameterEx.toxic_dex);
			_message.WriteFloat(_parameterEx.toxic_will);
			_message.WriteFloat(_parameterEx.toxic_luck);
			_message.WriteString(_parameterEx.lasttown);
			_message.WriteString(_parameterEx.lastdungeon);
		}

		private static void WriteSkillToMessage(PetSkill _skill, Message _message)
		{
			if (_skill == null)
			{
				_skill = new PetSkill();
			}
			_message.WriteS16(_skill.id);
			_message.WriteU8(_skill.level);
			_message.WriteS16(_skill.flag);
		}

		private static void WriteConditionsToMessage(PetCondition[] _condition, Message _message)
		{
			if (_condition == null)
			{
				_message.WriteU32(0u);
				return;
			}
			_message.WriteS32(_condition.Length);
			foreach (PetCondition petCondition in _condition)
			{
				if (petCondition.flag < 0)
				{
					petCondition.flag += 256;
				}
				_message.WriteS32(petCondition.flag);
				_message.WriteS8(petCondition.timemode);
				_message.WriteS64(petCondition.time);
				if (petCondition.meta != null && petCondition.meta.Length > 0)
				{
					_message.WriteString(petCondition.meta);
				}
				else
				{
					_message.WriteString("");
				}
			}
		}

		private static void WriteMemoryToMessage(PetMemory _memory, Message _message)
		{
			if (_memory == null)
			{
				_memory = new PetMemory();
			}
			_message.WriteString(_memory.target);
			_message.WriteU8(_memory.favor);
			_message.WriteU8(_memory.memory);
			_message.WriteU8(_memory.time_stamp);
		}

		private static void WriteDataToMessage(PetData _data, Message _message)
		{
			if (_data == null)
			{
				_data = new PetData();
			}
			_message.WriteString(_data.ui);
			_message.WriteString(_data.meta);
			_message.WriteS64(_data.birthday.Ticks);
			_message.WriteS64(_data.rebirthday.Ticks);
			_message.WriteS16(_data.rebirthage);
			_message.WriteS32(_data.playtime);
			_message.WriteS32(_data.wealth);
			_message.WriteU8(_data.writeCounter);
		}

		private static void WriteQuestToMessage(PetPrivate _private, Message _message)
		{
			if (_private == null)
			{
				_private = new PetPrivate();
			}
			WriteQuestReservedFromMessage(_private.reserveds, _message);
			WriteQuestRegisteredFromMessage(_private.registereds, _message);
		}

		private static void WriteQuestReservedFromMessage(PetPrivateReserved[] _reserveds, Message _message)
		{
			if (_reserveds == null)
			{
				_message.WriteU32(0u);
				return;
			}
			_message.WriteS32(_reserveds.Length);
			foreach (PetPrivateReserved petPrivateReserved in _reserveds)
			{
				PetPrivateReserved petPrivateReserved2 = petPrivateReserved;
				if (petPrivateReserved2 == null)
				{
					petPrivateReserved2 = new PetPrivateReserved();
				}
				_message.WriteS32(petPrivateReserved2.id);
				_message.WriteS32(petPrivateReserved2.rate);
			}
		}

		private static void WriteQuestRegisteredFromMessage(PetPrivateRegistered[] _registereds, Message _message)
		{
			if (_registereds == null)
			{
				_message.WriteU32(0u);
				return;
			}
			_message.WriteS32(_registereds.Length);
			foreach (PetPrivateRegistered petPrivateRegistered in _registereds)
			{
				PetPrivateRegistered petPrivateRegistered2 = petPrivateRegistered;
				if (petPrivateRegistered2 == null)
				{
					petPrivateRegistered2 = new PetPrivateRegistered();
				}
				_message.WriteS32(petPrivateRegistered2.id);
				_message.WriteS64(petPrivateRegistered2.start);
				_message.WriteS64(petPrivateRegistered2.end);
				_message.WriteS32(petPrivateRegistered2.extra);
			}
		}

		private static void WriteSummonToMessage(PetSummon _summon, Message _message)
		{
			if (_summon == null)
			{
				_summon = new PetSummon();
			}
			_message.WriteS32(_summon.remaintime);
			_message.WriteS64(_summon.lasttime);
			_message.WriteS64(_summon.expiretime);
			_message.WriteU8(_summon.loyalty);
			_message.WriteU8(_summon.favor);
		}

		private static void WriteMacroCheckerToMessage(PetMacroChecker _summon, Message _message)
		{
			if (_summon == null)
			{
				_summon = new PetMacroChecker();
			}
			_message.WriteS32(_summon.macroPoint);
		}
	}
}
