using Mabinogi;
using System;
using System.Collections.Generic;

namespace XMLDB3
{
	public class CharacterSerializer
	{
		public static CharacterInfo Serialize(Message _message)
		{
			return ReadCharacterFromMessage(_message);
		}

		private static CharacterInfo ReadCharacterFromMessage(Message _message)
		{
			CharacterInfo characterInfo = new CharacterInfo();
			characterInfo.id = _message.ReadS64();
			characterInfo.name = _message.ReadString();
			characterInfo.appearance = ReadAppearanceFromMessage(_message);
			characterInfo.parameter = ReadParameterFromMessage(_message);
			characterInfo.parameterEx = ReadParameterExFromMessage(_message);
			characterInfo.titles = ReadTitleFromMessage(_message);
			characterInfo.marriage = ReadMarriageFromMessage(_message);
			characterInfo.job = ReadJobFromMessage(_message);
			characterInfo.inventory = InventorySerializer.Serialize(_message);
			characterInfo.keyword = ReadKeywordFromMessage(_message);
			int num = _message.ReadS32();
			if (num > 0)
			{
				characterInfo.skills = new CharacterSkill[num];
				for (int i = 0; i < num; i++)
				{
					characterInfo.skills[i] = ReadSkillFromMessage(_message);
				}
			}
			else
			{
				characterInfo.skills = null;
			}
			characterInfo.arbeit = ReadArbeitFromMessage(_message);
			characterInfo.conditions = ReadConditionFromMessage(_message);
			num = _message.ReadS32();
			if (num > 0)
			{
				characterInfo.memorys = new CharacterMemory[num];
				for (int j = 0; j < num; j++)
				{
					characterInfo.memorys[j] = ReadMemoryFromMessage(_message);
				}
			}
			else
			{
				characterInfo.memorys = null;
			}
			characterInfo.data = ReadUIFromMessage(_message);
			characterInfo.Private = ReadCharQuestFromMessage(_message);
			characterInfo.deed = ReadCharDeedFromMessage(_message);
			characterInfo.service = ReadServiceFromMessage(_message);
			characterInfo.PVP = ReadPVPFromMessage(_message);
			characterInfo.farm = ReadFarmFromMessage(_message);
			characterInfo.heartSticker = ReadHeartStickerFromMessage(_message);
			characterInfo.joust = ReadJoustFromMessage(_message);
			characterInfo.achievements = ReadAchievementFromMessage(_message);
			characterInfo.macroChecker = ReadMacroCheckerFromMessage(_message);
			characterInfo.donation = ReadDonationFromMessage(_message);
			characterInfo.prifateFarm = ReadPrivateFarmFromMessage(_message);
			characterInfo.shape = ReadCharShapeFromMessage(_message);
			characterInfo.subSkill = ReadSubSkillsFromMessage(_message);
			characterInfo.divineKnight = ReadDivineKnightFromMessage(_message);
			characterInfo.myKnights = ReadMyKnightsFromMessage(_message);
			return characterInfo;
		}

		private static CharacterAppearance ReadAppearanceFromMessage(Message _message)
		{
			CharacterAppearance characterAppearance = new CharacterAppearance();
			_message.ReadString();
			_message.ReadString();
			characterAppearance.type = _message.ReadS32();
			characterAppearance.skin_color = _message.ReadU8();
			characterAppearance.eye_type = _message.ReadS16();
			if (characterAppearance.eye_type < 0)
			{
				characterAppearance.eye_type = 0;
			}
			characterAppearance.eye_color = _message.ReadU8();
			characterAppearance.mouth_type = _message.ReadU8();
			characterAppearance.status = _message.ReadS32();
			characterAppearance.height = _message.ReadFloat();
			characterAppearance.fatness = _message.ReadFloat();
			characterAppearance.upper = _message.ReadFloat();
			characterAppearance.lower = _message.ReadFloat();
			characterAppearance.region = _message.ReadS32();
			characterAppearance.x = _message.ReadS32();
			characterAppearance.y = _message.ReadS32();
			characterAppearance.direction = _message.ReadU8();
			characterAppearance.battle_state = _message.ReadS32();
			characterAppearance.weapon_set = _message.ReadU8();
			return characterAppearance;
		}

		private static CharacterParameter ReadParameterFromMessage(Message _message)
		{
			CharacterParameter characterParameter = new CharacterParameter();
			characterParameter.life = _message.ReadFloat();
			characterParameter.life_damage = _message.ReadFloat();
			characterParameter.life_max = _message.ReadFloat();
			characterParameter.mana = _message.ReadFloat();
			characterParameter.mana_max = _message.ReadFloat();
			characterParameter.stamina = _message.ReadFloat();
			characterParameter.stamina_max = _message.ReadFloat();
			characterParameter.food = _message.ReadFloat();
			characterParameter.level = _message.ReadS16();
			characterParameter.cumulatedlevel = _message.ReadS32();
			characterParameter.maxlevel = _message.ReadS16();
			characterParameter.rebirthcount = _message.ReadS16();
			characterParameter.lifetimeskill = _message.ReadS16();
			characterParameter.experience = _message.ReadS64();
			characterParameter.age = _message.ReadS16();
			characterParameter.strength = _message.ReadFloat();
			characterParameter.dexterity = _message.ReadFloat();
			characterParameter.intelligence = _message.ReadFloat();
			characterParameter.will = _message.ReadFloat();
			characterParameter.luck = _message.ReadFloat();
			characterParameter.life_max_by_food = _message.ReadFloat();
			characterParameter.mana_max_by_food = _message.ReadFloat();
			characterParameter.stamina_max_by_food = _message.ReadFloat();
			characterParameter.strength_by_food = _message.ReadFloat();
			characterParameter.dexterity_by_food = _message.ReadFloat();
			characterParameter.intelligence_by_food = _message.ReadFloat();
			characterParameter.will_by_food = _message.ReadFloat();
			characterParameter.luck_by_food = _message.ReadFloat();
			characterParameter.ability_remain = _message.ReadS32();
			characterParameter.attack_min = _message.ReadS16();
			characterParameter.attack_max = _message.ReadS16();
			characterParameter.wattack_min = _message.ReadS16();
			characterParameter.wattack_max = _message.ReadS16();
			characterParameter.critical = _message.ReadFloat();
			characterParameter.protect = _message.ReadFloat();
			characterParameter.defense = _message.ReadS16();
			characterParameter.rate = _message.ReadS16();
			characterParameter.rank1 = _message.ReadS16();
			characterParameter.rank2 = _message.ReadS16();
			characterParameter.score = _message.ReadS64();
			return characterParameter;
		}

		private static CharacterParameterEx ReadParameterExFromMessage(Message _message)
		{
			CharacterParameterEx characterParameterEx = new CharacterParameterEx();
			characterParameterEx.str_boost = _message.ReadU8();
			characterParameterEx.dex_boost = _message.ReadU8();
			characterParameterEx.int_boost = _message.ReadU8();
			characterParameterEx.will_boost = _message.ReadU8();
			characterParameterEx.luck_boost = _message.ReadU8();
			characterParameterEx.height_boost = _message.ReadU8();
			characterParameterEx.fatness_boost = _message.ReadU8();
			characterParameterEx.upper_boost = _message.ReadU8();
			characterParameterEx.lower_boost = _message.ReadU8();
			characterParameterEx.life_boost = _message.ReadU8();
			characterParameterEx.mana_boost = _message.ReadU8();
			characterParameterEx.stamina_boost = _message.ReadU8();
			characterParameterEx.toxic = _message.ReadFloat();
			characterParameterEx.toxic_drunken_time = _message.ReadS64();
			characterParameterEx.toxic_str = _message.ReadFloat();
			characterParameterEx.toxic_int = _message.ReadFloat();
			characterParameterEx.toxic_dex = _message.ReadFloat();
			characterParameterEx.toxic_will = _message.ReadFloat();
			characterParameterEx.toxic_luck = _message.ReadFloat();
			characterParameterEx.lasttown = _message.ReadString();
			characterParameterEx.lastdungeon = _message.ReadString();
			characterParameterEx.exploLevel = _message.ReadS16();
			characterParameterEx.exploMaxKeyLevel = _message.ReadS16();
			characterParameterEx.exploCumLevel = _message.ReadS32();
			characterParameterEx.exploExp = _message.ReadS64();
			characterParameterEx.discoverCount = _message.ReadS32();
			return characterParameterEx;
		}

		private static CharacterTitles ReadTitleFromMessage(Message _message)
		{
			CharacterTitles characterTitles = new CharacterTitles();
			characterTitles.selected = _message.ReadS16();
			characterTitles.appliedtime = _message.ReadU64();
			ushort num = _message.ReadU16();
			if (num > 0)
			{
				characterTitles.title = new List<CharacterTitlesTitle>(num);
				CharacterTitlesTitle characterTitlesTitle;
				for (ushort num2 = 0; num2 < num; num2 = (ushort)(num2 + 1))
				{
					characterTitlesTitle = new CharacterTitlesTitle();
					characterTitlesTitle.id = _message.ReadS16();
					characterTitlesTitle.state = _message.ReadU8();
					characterTitlesTitle.validtime = _message.ReadU64();
					characterTitles.title.Add(characterTitlesTitle);
				}
			}
			else
			{
				characterTitles.title = null;
			}
			characterTitles.option = _message.ReadS16();
			return characterTitles;
		}

		private static CharacterMarriage ReadMarriageFromMessage(Message _message)
		{
			CharacterMarriage characterMarriage = new CharacterMarriage();
			characterMarriage.mateid = _message.ReadS64();
			characterMarriage.matename = _message.ReadString();
			characterMarriage.marriagetime = _message.ReadS32();
			characterMarriage.marriagecount = _message.ReadS16();
			return characterMarriage;
		}

		private static CharacterJob ReadJobFromMessage(Message _message)
		{
			CharacterJob characterJob = new CharacterJob();
			if (_message.CurrentFieldType == Message.MessageFieldType.VT_BYTE)
			{
				characterJob.jobId = _message.ReadU8();
			}
			return characterJob;
		}

		private static CharacterKeyword ReadKeywordFromMessage(Message _message)
		{
			CharacterKeyword characterKeyword = new CharacterKeyword();
			int num = _message.ReadS32();
			if (num > 0)
			{
				characterKeyword.keywordSet = new CharacterKeywordSet[num];
				for (int i = 0; i < num; i++)
				{
					CharacterKeywordSet characterKeywordSet = new CharacterKeywordSet();
					characterKeywordSet.keywordId = _message.ReadS16();
					characterKeyword.keywordSet[i] = characterKeywordSet;
				}
			}
			else
			{
				characterKeyword.keywordSet = null;
			}
			return characterKeyword;
		}

		private static CharacterSkill ReadSkillFromMessage(Message _message)
		{
			CharacterSkill characterSkill = new CharacterSkill();
			characterSkill.id = _message.ReadS16();
			characterSkill.version = _message.ReadS16();
			characterSkill.level = _message.ReadU8();
			characterSkill.maxlevel = _message.ReadU8();
			characterSkill.experience = _message.ReadS32();
			characterSkill.count = _message.ReadS16();
			characterSkill.flag = _message.ReadS16();
			characterSkill.subflag1 = _message.ReadS16();
			characterSkill.subflag2 = _message.ReadS16();
			characterSkill.subflag3 = _message.ReadS16();
			characterSkill.subflag4 = _message.ReadS16();
			characterSkill.subflag5 = _message.ReadS16();
			characterSkill.subflag6 = _message.ReadS16();
			characterSkill.subflag7 = _message.ReadS16();
			characterSkill.subflag8 = _message.ReadS16();
			characterSkill.subflag9 = _message.ReadS16();
			characterSkill.lastPromotionTime = _message.ReadS64();
			characterSkill.promotionConditionCount = _message.ReadS16();
			characterSkill.promotionExperience = _message.ReadS32();
			return characterSkill;
		}

		private static CharacterSubSkillSet ReadSubSkillsFromMessage(Message _message)
		{
			CharacterSubSkillSet characterSubSkillSet = new CharacterSubSkillSet();
			dynamic val = _message.ReadTypeOf(CharacterSubSkillSet.SubSkillSize);
			if (val > 0)
			{
				characterSubSkillSet.subSkillSet = new CharacterSubSkill[(int)val];
				for (int i = 0; i < val; i++)
				{
					characterSubSkillSet.subSkillSet[i] = ReadSubSkillFromMessage(_message);
				}
			}
			else
			{
				characterSubSkillSet.subSkillSet = null;
			}
			return characterSubSkillSet;
		}

		private static CharacterSubSkill ReadSubSkillFromMessage(Message _message)
		{
			CharacterSubSkill characterSubSkill = new CharacterSubSkill();
			characterSubSkill.id = _message.ReadTypeOf(characterSubSkill.id);
			characterSubSkill.level = _message.ReadTypeOf(characterSubSkill.level);
			characterSubSkill.exp = _message.ReadTypeOf(characterSubSkill.exp);
			return characterSubSkill;
		}

		private static CharacterDivineKnight ReadDivineKnightFromMessage(Message _message)
		{
			CharacterDivineKnight characterDivineKnight = new CharacterDivineKnight();
			characterDivineKnight.groupLimit = _message.ReadTypeOf(characterDivineKnight.groupLimit);
			characterDivineKnight.groupSelected = _message.ReadTypeOf(characterDivineKnight.groupSelected);
			characterDivineKnight.exp = _message.ReadTypeOf(characterDivineKnight.exp);
			return characterDivineKnight;
		}

		private static CharacterMyKnightsMember[] ReadMyKnightsMemberListFromMessage(Message _message)
		{
			int num = _message.ReadS32();
			CharacterMyKnightsMember[] array = new CharacterMyKnightsMember[num];
			for (int i = 0; i < num; i++)
			{
				CharacterMyKnightsMember characterMyKnightsMember = new CharacterMyKnightsMember();
				characterMyKnightsMember.id = _message.ReadTypeOf(characterMyKnightsMember.id);
				characterMyKnightsMember.isRecruited = _message.ReadTypeOf(characterMyKnightsMember.isRecruited);
				characterMyKnightsMember.holy = _message.ReadTypeOf(characterMyKnightsMember.holy);
				characterMyKnightsMember.strength = _message.ReadTypeOf(characterMyKnightsMember.strength);
				characterMyKnightsMember.intelligence = _message.ReadTypeOf(characterMyKnightsMember.intelligence);
				characterMyKnightsMember.dexterity = _message.ReadTypeOf(characterMyKnightsMember.dexterity);
				characterMyKnightsMember.will = _message.ReadTypeOf(characterMyKnightsMember.will);
				characterMyKnightsMember.luck = _message.ReadTypeOf(characterMyKnightsMember.luck);
				characterMyKnightsMember.favorLevel = _message.ReadTypeOf(characterMyKnightsMember.favorLevel);
				characterMyKnightsMember.favorExp = _message.ReadTypeOf(characterMyKnightsMember.favorExp);
				characterMyKnightsMember.stress = _message.ReadTypeOf(characterMyKnightsMember.stress);
				characterMyKnightsMember.woundTime = _message.ReadTypeOf(characterMyKnightsMember.woundTime);
				characterMyKnightsMember.isSelfCured = _message.ReadTypeOf(characterMyKnightsMember.isSelfCured);
				characterMyKnightsMember.curTraining = _message.ReadTypeOf(characterMyKnightsMember.curTraining);
				characterMyKnightsMember.trainingStartTime = _message.ReadTypeOf(characterMyKnightsMember.trainingStartTime);
				characterMyKnightsMember.curTask = _message.ReadTypeOf(characterMyKnightsMember.curTask);
				characterMyKnightsMember.curTaskTemplate = _message.ReadTypeOf(characterMyKnightsMember.curTaskTemplate);
				characterMyKnightsMember.taskEndTime = _message.ReadTypeOf(characterMyKnightsMember.taskEndTime);
				characterMyKnightsMember.restStartTime = _message.ReadTypeOf(characterMyKnightsMember.restStartTime);
				characterMyKnightsMember.lastDateTime = _message.ReadTypeOf(characterMyKnightsMember.lastDateTime);
				characterMyKnightsMember.firstRecruitTime = _message.ReadTypeOf(characterMyKnightsMember.firstRecruitTime);
				characterMyKnightsMember.lastRecruitTime = _message.ReadTypeOf(characterMyKnightsMember.lastRecruitTime);
				characterMyKnightsMember.lastDismissTime = _message.ReadTypeOf(characterMyKnightsMember.lastDismissTime);
				characterMyKnightsMember.dismissCount = _message.ReadTypeOf(characterMyKnightsMember.dismissCount);
				characterMyKnightsMember.taskTryCount = _message.ReadTypeOf(characterMyKnightsMember.taskTryCount);
				characterMyKnightsMember.taskSuccessCount = _message.ReadTypeOf(characterMyKnightsMember.taskSuccessCount);
				characterMyKnightsMember.favorTalkCount = _message.ReadTypeOf(characterMyKnightsMember.favorTalkCount);
				characterMyKnightsMember.latestDateList = _message.ReadTypeOf(characterMyKnightsMember.latestDateList);
				array[i] = characterMyKnightsMember;
			}
			return array;
		}

		private static CharacterMyKnights ReadMyKnightsFromMessage(Message _message)
		{
			CharacterMyKnights characterMyKnights = new CharacterMyKnights();
			characterMyKnights.name = _message.ReadTypeOf(characterMyKnights.name);
			characterMyKnights.level = _message.ReadTypeOf(characterMyKnights.level);
			characterMyKnights.exp = _message.ReadTypeOf(characterMyKnights.exp);
			characterMyKnights.trainingPoint = _message.ReadTypeOf(characterMyKnights.trainingPoint);
			characterMyKnights.dateBuffMember = _message.ReadTypeOf(characterMyKnights.dateBuffMember);
			characterMyKnights.makeTime = new DateTime(_message.ReadS64());
			characterMyKnights.addedSlotCount = _message.ReadTypeOf(characterMyKnights.addedSlotCount);
			characterMyKnights.memberList = ReadMyKnightsMemberListFromMessage(_message);
			return characterMyKnights;
		}

		private static CharacterMemory ReadMemoryFromMessage(Message _message)
		{
			CharacterMemory characterMemory = new CharacterMemory();
			characterMemory.target = _message.ReadString();
			characterMemory.favor = _message.ReadU8();
			characterMemory.memory = _message.ReadU8();
			characterMemory.time_stamp = _message.ReadU8();
			return characterMemory;
		}

		private static CharacterArbeit ReadArbeitFromMessage(Message _message)
		{
			CharacterArbeit characterArbeit = new CharacterArbeit();
			characterArbeit.history = ReadArbeitHistoryFromMessage(_message);
			characterArbeit.collection = ReadArbeitCollectionFromMessage(_message);
			return characterArbeit;
		}

		private static CharacterArbeitInfo[] ReadArbeitCollectionFromMessage(Message _message)
		{
			uint num = _message.ReadU32();
			CharacterArbeitInfo[] array = new CharacterArbeitInfo[num];
			for (uint num2 = 0u; num2 < num; num2++)
			{
				CharacterArbeitInfo characterArbeitInfo = new CharacterArbeitInfo();
				characterArbeitInfo.category = _message.ReadS16();
				characterArbeitInfo.total = _message.ReadS16();
				characterArbeitInfo.success = _message.ReadS16();
				array[num2] = characterArbeitInfo;
			}
			return array;
		}

		private static CharacterArbeitDay[] ReadArbeitHistoryFromMessage(Message _message)
		{
			uint num = _message.ReadU32();
			CharacterArbeitDay[] array = new CharacterArbeitDay[num];
			for (uint num2 = 0u; num2 < num; num2++)
			{
				CharacterArbeitDay characterArbeitDay = new CharacterArbeitDay();
				characterArbeitDay.daycount = _message.ReadS32();
				uint num3 = _message.ReadU32();
				characterArbeitDay.info = new CharacterArbeitDayInfo[num3];
				for (uint num4 = 0u; num4 < num3; num4++)
				{
					CharacterArbeitDayInfo characterArbeitDayInfo = new CharacterArbeitDayInfo();
					characterArbeitDayInfo.category = _message.ReadS16();
					characterArbeitDay.info[num4] = characterArbeitDayInfo;
				}
				array[num2] = characterArbeitDay;
			}
			return array;
		}

		private static CharacterCondition[] ReadConditionFromMessage(Message _message)
		{
			uint num = _message.ReadU32();
			CharacterCondition[] array = new CharacterCondition[num];
			for (uint num2 = 0u; num2 < num; num2++)
			{
				CharacterCondition characterCondition = new CharacterCondition();
				characterCondition.flag = _message.ReadS32();
				characterCondition.timemode = _message.ReadS8();
				characterCondition.time = _message.ReadS64();
				characterCondition.meta = _message.ReadString();
				if (characterCondition.meta.Length == 0)
				{
					characterCondition.meta = null;
				}
				array[num2] = characterCondition;
			}
			return array;
		}

		private static CharacterData ReadUIFromMessage(Message _message)
		{
			CharacterData characterData = new CharacterData();
			_message.ReadString();
			characterData.meta = _message.ReadString();
			characterData.nao_memory = _message.ReadS16();
			characterData.nao_favor = _message.ReadS16();
			characterData.nao_style = _message.ReadU8();
			characterData.birthday = new DateTime(_message.ReadS64());
			characterData.rebirthday = new DateTime(_message.ReadS64());
			characterData.rebirthage = _message.ReadS16();
			characterData.playtime = _message.ReadS32();
			characterData.wealth = _message.ReadS32();
			characterData.writeCounter = _message.ReadU8();
			return characterData;
		}

		private static CharacterPrivate ReadCharQuestFromMessage(Message _message)
		{
			CharacterPrivate characterPrivate = new CharacterPrivate();
			characterPrivate.reserveds = ReadQuestReservedFromMessage(_message);
			characterPrivate.registereds = ReadQuestRegisteredFromMessage(_message);
			characterPrivate.books = ReadQuestBookFromMessage(_message);
			return characterPrivate;
		}

		private static CharacterPrivateReserved[] ReadQuestReservedFromMessage(Message _message)
		{
			uint num = _message.ReadU32();
			CharacterPrivateReserved[] array = new CharacterPrivateReserved[num];
			for (uint num2 = 0u; num2 < num; num2++)
			{
				CharacterPrivateReserved characterPrivateReserved = new CharacterPrivateReserved();
				characterPrivateReserved.id = _message.ReadS32();
				characterPrivateReserved.rate = _message.ReadS32();
				array[num2] = characterPrivateReserved;
			}
			return array;
		}

		private static CharacterPrivateRegistered[] ReadQuestRegisteredFromMessage(Message _message)
		{
			uint num = _message.ReadU32();
			CharacterPrivateRegistered[] array = new CharacterPrivateRegistered[num];
			for (uint num2 = 0u; num2 < num; num2++)
			{
				CharacterPrivateRegistered characterPrivateRegistered = new CharacterPrivateRegistered();
				characterPrivateRegistered.id = _message.ReadS32();
				characterPrivateRegistered.start = _message.ReadS64();
				characterPrivateRegistered.end = _message.ReadS64();
				characterPrivateRegistered.extra = _message.ReadS32();
				array[num2] = characterPrivateRegistered;
			}
			return array;
		}

		private static CharacterPrivateBook[] ReadQuestBookFromMessage(Message _message)
		{
			uint num = _message.ReadU32();
			CharacterPrivateBook[] array = new CharacterPrivateBook[num];
			for (uint num2 = 0u; num2 < num; num2++)
			{
				CharacterPrivateBook characterPrivateBook = new CharacterPrivateBook();
				characterPrivateBook.id = _message.ReadS32();
				array[num2] = characterPrivateBook;
			}
			return array;
		}

		private static CharacterDeed ReadCharDeedFromMessage(Message _message)
		{
			CharacterDeed characterDeed = new CharacterDeed();
			int num = _message.ReadS32();
			if (num > 0)
			{
				characterDeed.deedSet = new CharacterDeedSet[num];
				for (int i = 0; i < num; i++)
				{
					CharacterDeedSet characterDeedSet = new CharacterDeedSet();
					characterDeedSet.deedBitFlag = _message.ReadS64();
					characterDeedSet.deedUpdateTime = _message.ReadS32();
					characterDeed.deedSet[i] = characterDeedSet;
				}
			}
			else
			{
				characterDeed.deedSet = null;
			}
			return characterDeed;
		}

		private static CharacterShape ReadCharShapeFromMessage(Message _message)
		{
			CharacterShape characterShape = new CharacterShape();
			int num = _message.ReadS32();
			if (num > 0)
			{
				characterShape.shapeSet = new CharacterShapeSet[num];
				for (int i = 0; i < num; i++)
				{
					CharacterShapeSet characterShapeSet = new CharacterShapeSet();
					characterShapeSet.shapeId = _message.ReadS32();
					characterShapeSet.shapeCount = _message.ReadU8();
					characterShapeSet.collectBitFlag = _message.ReadU8();
					characterShape.shapeSet[i] = characterShapeSet;
				}
			}
			else
			{
				characterShape.shapeSet = null;
			}
			return characterShape;
		}

		private static CharacterService ReadServiceFromMessage(Message _message)
		{
			CharacterService characterService = new CharacterService();
			characterService.nsrespawncount = _message.ReadU8();
			characterService.nslastrespawnday = _message.ReadS32();
			characterService.nsbombcount = _message.ReadU8();
			characterService.nsbombday = _message.ReadS32();
			characterService.nsgiftreceiveday = _message.ReadS32();
			characterService.apgiftreceiveday = _message.ReadS32();
			return characterService;
		}

		private static CharacterPVP ReadPVPFromMessage(Message _message)
		{
			CharacterPVP characterPVP = new CharacterPVP();
			characterPVP.winCnt = _message.ReadS64();
			characterPVP.loseCnt = _message.ReadS64();
			characterPVP.penaltyPoint = _message.ReadS32();
			return characterPVP;
		}

		private static CharacterFarm ReadFarmFromMessage(Message _message)
		{
			CharacterFarm characterFarm = new CharacterFarm();
			characterFarm.farmID = _message.ReadS64();
			return characterFarm;
		}

		private static CharacterHeartSticker ReadHeartStickerFromMessage(Message _message)
		{
			CharacterHeartSticker characterHeartSticker = new CharacterHeartSticker();
			characterHeartSticker.heartUpdateTime = _message.ReadS64();
			characterHeartSticker.heartPoint = _message.ReadS16();
			characterHeartSticker.heartTotalPoint = _message.ReadS16();
			return characterHeartSticker;
		}

		private static CharacterJoust ReadJoustFromMessage(Message _message)
		{
			CharacterJoust characterJoust = new CharacterJoust();
			characterJoust.joustPoint = _message.ReadS32();
			characterJoust.joustLastWinYear = _message.ReadU8();
			characterJoust.joustLastWinWeek = _message.ReadU8();
			characterJoust.joustWeekWinCount = _message.ReadU8();
			characterJoust.joustDailyWinCount = _message.ReadS16();
			characterJoust.joustDailyLoseCount = _message.ReadS16();
			characterJoust.joustServerWinCount = _message.ReadS16();
			characterJoust.joustServerLoseCount = _message.ReadS16();
			return characterJoust;
		}

		private static CharacterAchievements ReadAchievementFromMessage(Message _message)
		{
			CharacterAchievements characterAchievements = new CharacterAchievements();
			characterAchievements.totalscore = _message.ReadS32();
			int num = _message.ReadS32();
			if (num > 0)
			{
				characterAchievements.achievement = new CharacterAchievementsAchievement[num];
				for (int i = 0; i < num; i++)
				{
					CharacterAchievementsAchievement characterAchievementsAchievement = new CharacterAchievementsAchievement();
					characterAchievementsAchievement.setid = _message.ReadS16();
					characterAchievementsAchievement.bitflag = _message.ReadS32();
					characterAchievements.achievement[i] = characterAchievementsAchievement;
				}
			}
			else
			{
				characterAchievements.achievement = null;
			}
			return characterAchievements;
		}

		private static CharacterPrivateFarm ReadPrivateFarmFromMessage(Message _message)
		{
			CharacterPrivateFarm characterPrivateFarm = new CharacterPrivateFarm();
			int num = _message.ReadS32();
			if (num > 0)
			{
				characterPrivateFarm.favoriteFarm = new CharacterFavoritePrivateFarm[num];
				for (int i = 0; i < num; i++)
				{
					CharacterFavoritePrivateFarm characterFavoritePrivateFarm = new CharacterFavoritePrivateFarm();
					characterFavoritePrivateFarm.privateFarmId = _message.ReadS64();
					characterFavoritePrivateFarm.themeId = _message.ReadS32();
					characterFavoritePrivateFarm.posX = _message.ReadU16();
					characterFavoritePrivateFarm.posY = _message.ReadU16();
					characterFavoritePrivateFarm.farmName = _message.ReadString();
					characterFavoritePrivateFarm.ownerName = _message.ReadString();
					characterPrivateFarm.favoriteFarm[i] = characterFavoritePrivateFarm;
				}
			}
			else
			{
				characterPrivateFarm.favoriteFarm = null;
			}
			return characterPrivateFarm;
		}

		private static CharacterMacroChecker ReadMacroCheckerFromMessage(Message _message)
		{
			CharacterMacroChecker characterMacroChecker = new CharacterMacroChecker();
			characterMacroChecker.macroPoint = _message.ReadS32();
			return characterMacroChecker;
		}

		private static CharacterDonation ReadDonationFromMessage(Message _message)
		{
			CharacterDonation characterDonation = new CharacterDonation();
			characterDonation.donationValue = _message.ReadS32();
			characterDonation.donationUpdate = _message.ReadS64();
			return characterDonation;
		}

		public static void Deserialize(CharacterInfo _character, Message _message)
		{
			_message.WriteS64(_character.id);
			_message.WriteString(_character.name);
			WriteAppearanceToMessage(_character.appearance, _message);
			WriteParameterToMessage(_character.parameter, _message);
			WriteParameterExToMessage(_character.parameterEx, _message);
			WriteTitleToMessage(_character.titles, _message);
			WriteMarriageToMessage(_character.marriage, _message);
			WriteJobToMessage(_character.job, _message);
			InventorySerializer.Deserialize(_character.inventory, _message);
			WriteKeywordToMessage(_character.keyword, _message);
			if (_character.skills != null && _character.skills.Length > 0)
			{
				_message.WriteS32(_character.skills.Length);
				CharacterSkill[] skills = _character.skills;
				foreach (CharacterSkill skill in skills)
				{
					WriteSkillToMessage(skill, _message);
				}
			}
			else
			{
				_message.WriteS32(0);
			}
			WriteArbeitToMessage(_character.arbeit, _message);
			WriteConditionsToMessage(_character.conditions, _message);
			if (_character.memorys != null && _character.memorys.Length > 0)
			{
				_message.WriteS32(_character.memorys.Length);
				CharacterMemory[] memorys = _character.memorys;
				foreach (CharacterMemory memory in memorys)
				{
					WriteMemoryToMessage(memory, _message);
				}
			}
			else
			{
				_message.WriteS32(0);
			}
			WriteUIToMessage(_character.data, _message);
			WriteCharQuestFromMessage(_character.Private, _message);
			WriteCharDeedFromMessage(_character.deed, _message);
			WriteServiceToMessage(_character.service, _message);
			WritePVPToMessage(_character.PVP, _message);
			WriteFarmToMessage(_character.farm, _message);
			WriteHeartStickerToMessage(_character.heartSticker, _message);
			WriteJoustToMessage(_character.joust, _message);
			WriteAchievementToMessage(_character.achievements, _message);
			WriteMacroCheckerToMessage(_character.macroChecker, _message);
			WriteDonationToMessage(_character.donation, _message);
			WritePrivateFarmToMessage(_character.prifateFarm, _message);
			WriteCharShapeFromMessage(_character.shape, _message);
			WriteSubSkillsToMessage(_character.subSkill, _message);
			WriteDivineKnightToMessage(_character.divineKnight, _message);
			WriteMyKnightsToMessage(_character.myKnights, _message);
		}

		private static void WriteAppearanceToMessage(CharacterAppearance _appearance, Message _message)
		{
			if (_appearance == null)
			{
				_appearance = new CharacterAppearance();
			}
			_message.WriteString(string.Empty);
			_message.WriteString(string.Empty);
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
			_message.WriteU8(_appearance.weapon_set);
		}

		private static void WriteParameterToMessage(CharacterParameter _parameter, Message _message)
		{
			if (_parameter == null)
			{
				_parameter = new CharacterParameter();
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
			_message.WriteS16(_parameter.lifetimeskill);
			_message.WriteS64(_parameter.experience);
			_message.WriteS16(_parameter.age);
			_message.WriteFloat(_parameter.strength);
			_message.WriteFloat(_parameter.dexterity);
			_message.WriteFloat(_parameter.intelligence);
			_message.WriteFloat(_parameter.will);
			_message.WriteFloat(_parameter.luck);
			_message.WriteFloat(_parameter.life_max_by_food);
			_message.WriteFloat(_parameter.mana_max_by_food);
			_message.WriteFloat(_parameter.stamina_max_by_food);
			_message.WriteFloat(_parameter.strength_by_food);
			_message.WriteFloat(_parameter.dexterity_by_food);
			_message.WriteFloat(_parameter.intelligence_by_food);
			_message.WriteFloat(_parameter.will_by_food);
			_message.WriteFloat(_parameter.luck_by_food);
			_message.WriteS32(_parameter.ability_remain);
			_message.WriteS16(_parameter.attack_min);
			_message.WriteS16(_parameter.attack_max);
			_message.WriteS16(_parameter.wattack_min);
			_message.WriteS16(_parameter.wattack_max);
			_message.WriteFloat(_parameter.critical);
			_message.WriteFloat(_parameter.protect);
			_message.WriteS16(_parameter.defense);
			_message.WriteS16(_parameter.rate);
			_message.WriteS16(_parameter.rank1);
			_message.WriteS16(_parameter.rank2);
			_message.WriteS64(_parameter.score);
		}

		private static void WriteParameterExToMessage(CharacterParameterEx _parameter, Message _message)
		{
			if (_parameter == null)
			{
				_parameter = new CharacterParameterEx();
			}
			_message.WriteU8(_parameter.str_boost);
			_message.WriteU8(_parameter.dex_boost);
			_message.WriteU8(_parameter.int_boost);
			_message.WriteU8(_parameter.will_boost);
			_message.WriteU8(_parameter.luck_boost);
			_message.WriteU8(_parameter.height_boost);
			_message.WriteU8(_parameter.fatness_boost);
			_message.WriteU8(_parameter.upper_boost);
			_message.WriteU8(_parameter.lower_boost);
			_message.WriteU8(_parameter.life_boost);
			_message.WriteU8(_parameter.mana_boost);
			_message.WriteU8(_parameter.stamina_boost);
			_message.WriteFloat(_parameter.toxic);
			_message.WriteS64(_parameter.toxic_drunken_time);
			_message.WriteFloat(_parameter.toxic_str);
			_message.WriteFloat(_parameter.toxic_int);
			_message.WriteFloat(_parameter.toxic_dex);
			_message.WriteFloat(_parameter.toxic_will);
			_message.WriteFloat(_parameter.toxic_luck);
			_message.WriteString(_parameter.lasttown);
			_message.WriteString(_parameter.lastdungeon);
			_message.WriteS16(_parameter.exploLevel);
			_message.WriteS16(_parameter.exploMaxKeyLevel);
			_message.WriteS32(_parameter.exploCumLevel);
			_message.WriteS64(_parameter.exploExp);
			_message.WriteS32(_parameter.discoverCount);
		}

		private static void WriteTitleToMessage(CharacterTitles _title, Message _message)
		{
			if (_title == null)
			{
				_title = new CharacterTitles();
			}
			_message.WriteS16(_title.selected);
			_message.WriteU64(_title.appliedtime);
			if (_title.title != null && _title.title.Count > 0)
			{
				_message.WriteU16((ushort)_title.title.Count);

				foreach (CharacterTitlesTitle characterTitlesTitle in _title.title)
				{
					if (characterTitlesTitle != null)
					{
						_message.WriteS16(characterTitlesTitle.id);
						_message.WriteU8(characterTitlesTitle.state);
						_message.WriteU64(characterTitlesTitle.validtime);
					}
					else
					{
						_message.WriteS16(0);
						_message.WriteU8(0);
						_message.WriteU64(0uL);
					}
				}
			}
			else
			{
				_message.WriteU16(0);
			}
			_message.WriteS16(_title.option);
		}

		private static void WriteMarriageToMessage(CharacterMarriage _marriage, Message _message)
		{
			if (_marriage == null)
			{
				_marriage = new CharacterMarriage();
				_marriage.matename = string.Empty;
			}
			_message.WriteS64(_marriage.mateid);
			_message.WriteString(_marriage.matename);
			_message.WriteS32(_marriage.marriagetime);
			_message.WriteS16(_marriage.marriagecount);
		}

		private static void WriteJobToMessage(CharacterJob _job, Message _message)
		{
			if (_job == null)
			{
				_job = new CharacterJob();
				_job.jobId = 0;
			}
			_message.WriteU8(_job.jobId);
		}

		private static void WriteKeywordToMessage(CharacterKeyword _keyword, Message _message)
		{
			if (_keyword == null)
			{
				_keyword = new CharacterKeyword();
			}
			if (_keyword.keywordSet != null && _keyword.keywordSet.Length > 0)
			{
				_message.WriteS32(_keyword.keywordSet.Length);
				CharacterKeywordSet[] keywordSet = _keyword.keywordSet;
				foreach (CharacterKeywordSet characterKeywordSet in keywordSet)
				{
					if (characterKeywordSet != null)
					{
						_message.WriteS16(characterKeywordSet.keywordId);
					}
					else
					{
						_message.WriteS16(0);
					}
				}
			}
			else
			{
				_message.WriteS32(0);
			}
		}

		private static void WriteSkillToMessage(CharacterSkill _skill, Message _message)
		{
			if (_skill == null)
			{
				_skill = new CharacterSkill();
			}
			_message.WriteS16(_skill.id);
			_message.WriteS16(_skill.version);
			_message.WriteU8(_skill.level);
			_message.WriteU8(_skill.maxlevel);
			_message.WriteS32(_skill.experience);
			_message.WriteS16(_skill.count);
			_message.WriteS16(_skill.flag);
			_message.WriteS16(_skill.subflag1);
			_message.WriteS16(_skill.subflag2);
			_message.WriteS16(_skill.subflag3);
			_message.WriteS16(_skill.subflag4);
			_message.WriteS16(_skill.subflag5);
			_message.WriteS16(_skill.subflag6);
			_message.WriteS16(_skill.subflag7);
			_message.WriteS16(_skill.subflag8);
			_message.WriteS16(_skill.subflag9);
			_message.WriteS64(_skill.lastPromotionTime);
			_message.WriteS16(_skill.promotionConditionCount);
			_message.WriteS32(_skill.promotionExperience);
		}

		private static void WriteSubSkillsToMessage(CharacterSubSkillSet _subSkills, Message _message)
		{
			if (_subSkills == null)
			{
				_subSkills = new CharacterSubSkillSet();
			}
			if (_subSkills.subSkillSet != null && _subSkills.subSkillSet.Length > 0)
			{
				_message.WriteU16((ushort)_subSkills.subSkillSet.Length);
				CharacterSubSkill[] subSkillSet = _subSkills.subSkillSet;
				foreach (CharacterSubSkill subSkill in subSkillSet)
				{
					WriteSubSkillToMessage(subSkill, _message);
				}
			}
			else
			{
				_message.WriteU16(0);
			}
		}

		private static void WriteSubSkillToMessage(CharacterSubSkill _subSkill, Message _message)
		{
			if (_subSkill == null)
			{
				_subSkill = new CharacterSubSkill();
			}
			_message.WriteTypeOf(_subSkill.id);
			_message.WriteTypeOf(_subSkill.level);
			_message.WriteTypeOf(_subSkill.exp);
		}

		private static void WriteDivineKnightToMessage(CharacterDivineKnight _divineKnight, Message _message)
		{
			if (_divineKnight == null)
			{
				_divineKnight = new CharacterDivineKnight();
			}
			_message.WriteTypeOf(_divineKnight.groupLimit);
			_message.WriteTypeOf(_divineKnight.groupSelected);
			_message.WriteTypeOf(_divineKnight.exp);
		}

		private static void WriteMyKnightsMemberToMessage(CharacterMyKnightsMember _member, Message _message)
		{
			if (_member == null)
			{
				_member = new CharacterMyKnightsMember();
				_member.latestDateList = string.Empty;
			}
			_message.WriteTypeOf(_member.id);
			_message.WriteTypeOf(_member.isRecruited);
			_message.WriteTypeOf(_member.holy);
			_message.WriteTypeOf(_member.strength);
			_message.WriteTypeOf(_member.intelligence);
			_message.WriteTypeOf(_member.dexterity);
			_message.WriteTypeOf(_member.will);
			_message.WriteTypeOf(_member.luck);
			_message.WriteTypeOf(_member.favorLevel);
			_message.WriteTypeOf(_member.favorExp);
			_message.WriteTypeOf(_member.stress);
			_message.WriteTypeOf(_member.woundTime);
			_message.WriteTypeOf(_member.isSelfCured);
			_message.WriteTypeOf(_member.curTraining);
			_message.WriteTypeOf(_member.trainingStartTime);
			_message.WriteTypeOf(_member.curTask);
			_message.WriteTypeOf(_member.curTaskTemplate);
			_message.WriteTypeOf(_member.taskEndTime);
			_message.WriteTypeOf(_member.restStartTime);
			_message.WriteTypeOf(_member.lastDateTime);
			_message.WriteTypeOf(_member.firstRecruitTime);
			_message.WriteTypeOf(_member.lastRecruitTime);
			_message.WriteTypeOf(_member.lastDismissTime);
			_message.WriteTypeOf(_member.dismissCount);
			_message.WriteTypeOf(_member.taskTryCount);
			_message.WriteTypeOf(_member.taskSuccessCount);
			_message.WriteTypeOf(_member.favorTalkCount);
			_message.WriteTypeOf(_member.latestDateList);
		}

		private static void WriteMyKnightsToMessage(CharacterMyKnights _myKnights, Message _message)
		{
			if (_myKnights == null)
			{
				_myKnights = new CharacterMyKnights();
				_myKnights.name = string.Empty;
				_myKnights.memberList = new CharacterMyKnightsMember[0];
			}
			_message.WriteTypeOf(_myKnights.name);
			_message.WriteTypeOf(_myKnights.level);
			_message.WriteTypeOf(_myKnights.exp);
			_message.WriteTypeOf(_myKnights.trainingPoint);
			_message.WriteTypeOf(_myKnights.dateBuffMember);
			_message.WriteS64(_myKnights.makeTime.Ticks);
			_message.WriteTypeOf(_myKnights.addedSlotCount);
			if (_myKnights.memberList == null)
			{
				_message.WriteS32(0);
				return;
			}
			_message.WriteTypeOf(_myKnights.memberList.Length);
			CharacterMyKnightsMember[] memberList = _myKnights.memberList;
			foreach (CharacterMyKnightsMember member in memberList)
			{
				WriteMyKnightsMemberToMessage(member, _message);
			}
		}

		private static void WriteMemoryToMessage(CharacterMemory _memory, Message _message)
		{
			if (_memory == null)
			{
				_memory = new CharacterMemory();
			}
			_message.WriteString(_memory.target);
			_message.WriteU8(_memory.favor);
			_message.WriteU8(_memory.memory);
			_message.WriteU8(_memory.time_stamp);
		}

		private static void WriteArbeitToMessage(CharacterArbeit _arbeit, Message _message)
		{
			if (_arbeit == null)
			{
				_arbeit = new CharacterArbeit();
			}
			WriteArbeitHistoryToMessage(_arbeit.history, _message);
			WriteArbeitCollectionToMessage(_arbeit.collection, _message);
		}

		private static void WriteArbeitCollectionToMessage(CharacterArbeitInfo[] _arbeitInfo, Message _message)
		{
			if (_arbeitInfo == null || _arbeitInfo.Length == 0)
			{
				_message.WriteU32(0u);
				return;
			}
			_message.WriteS32(_arbeitInfo.Length);
			foreach (CharacterArbeitInfo characterArbeitInfo in _arbeitInfo)
			{
				CharacterArbeitInfo characterArbeitInfo2 = characterArbeitInfo;
				if (characterArbeitInfo2 == null)
				{
					characterArbeitInfo2 = new CharacterArbeitInfo();
				}
				_message.WriteS16(characterArbeitInfo2.category);
				_message.WriteS16(characterArbeitInfo2.total);
				_message.WriteS16(characterArbeitInfo2.success);
			}
		}

		private static void WriteArbeitHistoryToMessage(CharacterArbeitDay[] _arbeitHistory, Message _message)
		{
			if (_arbeitHistory == null || _arbeitHistory.Length == 0)
			{
				_message.WriteU32(0u);
				return;
			}
			_message.WriteS32(_arbeitHistory.Length);
			foreach (CharacterArbeitDay characterArbeitDay in _arbeitHistory)
			{
				CharacterArbeitDay characterArbeitDay2 = characterArbeitDay;
				if (characterArbeitDay2 == null)
				{
					characterArbeitDay2 = new CharacterArbeitDay();
				}
				_message.WriteS32(characterArbeitDay2.daycount);
				WriteArbeitDayInfoToMessage(characterArbeitDay2.info, _message);
			}
		}

		private static void WriteArbeitDayInfoToMessage(CharacterArbeitDayInfo[] _arbeitDayInfo, Message _message)
		{
			if (_arbeitDayInfo == null || _arbeitDayInfo.Length == 0)
			{
				_message.WriteS32(0);
				return;
			}
			_message.WriteS32(_arbeitDayInfo.Length);
			foreach (CharacterArbeitDayInfo characterArbeitDayInfo in _arbeitDayInfo)
			{
				CharacterArbeitDayInfo characterArbeitDayInfo2 = characterArbeitDayInfo;
				if (characterArbeitDayInfo2 == null)
				{
					characterArbeitDayInfo2 = new CharacterArbeitDayInfo();
				}
				_message.WriteS16(characterArbeitDayInfo2.category);
			}
		}

		private static void WriteConditionsToMessage(CharacterCondition[] _condition, Message _message)
		{
			if (_condition == null || _condition.Length == 0)
			{
				_message.WriteS32(0);
				return;
			}
			_message.WriteS32(_condition.Length);
			foreach (CharacterCondition characterCondition in _condition)
			{
				CharacterCondition characterCondition2 = characterCondition;
				if (characterCondition2 == null)
				{
					characterCondition2 = new CharacterCondition();
				}
				if (characterCondition2.flag < 0)
				{
					characterCondition2.flag += 256;
				}
				_message.WriteS32(characterCondition2.flag);
				_message.WriteS8(characterCondition2.timemode);
				_message.WriteS64(characterCondition2.time);
				if (characterCondition2.meta != null && characterCondition2.meta.Length > 0)
				{
					_message.WriteString(characterCondition2.meta);
				}
				else
				{
					_message.WriteString("");
				}
			}
		}

		private static void WriteUIToMessage(CharacterData _data, Message _message)
		{
			if (_data == null)
			{
				_data = new CharacterData();
				_data.meta = string.Empty;
			}
			_message.WriteString(string.Empty);
			_message.WriteString(_data.meta);
			_message.WriteS16(_data.nao_memory);
			_message.WriteS16(_data.nao_favor);
			_message.WriteU8(_data.nao_style);
			_message.WriteS64(_data.birthday.Ticks);
			_message.WriteS64(_data.rebirthday.Ticks);
			_message.WriteS16(_data.rebirthage);
			_message.WriteS32(_data.playtime);
			_message.WriteS32(_data.wealth);
			_message.WriteU8(_data.writeCounter);
		}

		private static void WriteCharQuestFromMessage(CharacterPrivate _private, Message _message)
		{
			WriteQuestReservedFromMessage(_private.reserveds, _message);
			WriteQuestRegisteredFromMessage(_private.registereds, _message);
			WriteQuestBookFromMessage(_private.books, _message);
		}

		private static void WriteQuestReservedFromMessage(CharacterPrivateReserved[] _reserved, Message _message)
		{
			if (_reserved == null || _reserved.Length == 0)
			{
				_message.WriteS32(0);
				return;
			}
			_message.WriteS32(_reserved.Length);
			foreach (CharacterPrivateReserved characterPrivateReserved in _reserved)
			{
				CharacterPrivateReserved characterPrivateReserved2 = characterPrivateReserved;
				if (characterPrivateReserved2 == null)
				{
					characterPrivateReserved2 = new CharacterPrivateReserved();
				}
				_message.WriteS32(characterPrivateReserved2.id);
				_message.WriteS32(characterPrivateReserved2.rate);
			}
		}

		private static void WriteQuestRegisteredFromMessage(CharacterPrivateRegistered[] _registered, Message _message)
		{
			if (_registered == null || _registered.Length == 0)
			{
				_message.WriteS32(0);
				return;
			}
			_message.WriteS32(_registered.Length);
			foreach (CharacterPrivateRegistered characterPrivateRegistered in _registered)
			{
				CharacterPrivateRegistered characterPrivateRegistered2 = characterPrivateRegistered;
				if (characterPrivateRegistered2 == null)
				{
					characterPrivateRegistered2 = new CharacterPrivateRegistered();
				}
				_message.WriteS32(characterPrivateRegistered2.id);
				_message.WriteS64(characterPrivateRegistered2.start);
				_message.WriteS64(characterPrivateRegistered2.end);
				_message.WriteS32(characterPrivateRegistered2.extra);
			}
		}

		private static void WriteQuestBookFromMessage(CharacterPrivateBook[] _book, Message _message)
		{
			if (_book == null || _book.Length == 0)
			{
				_message.WriteS32(0);
				return;
			}

			_message.WriteS32(_book.Length);
			foreach (CharacterPrivateBook characterPrivateBook in _book)
			{
				if (characterPrivateBook == null)
				{
					_message.WriteS32(0);
				}
				else
				{
					_message.WriteS32(characterPrivateBook.id);
				}
			}
		}

		private static void WriteCharDeedFromMessage(CharacterDeed _deed, Message _message)
		{
			if (_deed == null)
			{
				_deed = new CharacterDeed();
			}
			if (_deed.deedSet != null && _deed.deedSet.Length > 0)
			{
				_message.WriteS32(_deed.deedSet.Length);
				CharacterDeedSet[] deedSet = _deed.deedSet;
				foreach (CharacterDeedSet characterDeedSet in deedSet)
				{
					if (characterDeedSet != null)
					{
						_message.WriteS64(characterDeedSet.deedBitFlag);
						_message.WriteS32(characterDeedSet.deedUpdateTime);
					}
					else
					{
						_message.WriteS64(0L);
						_message.WriteS32(0);
					}
				}
			}
			else
			{
				_message.WriteS32(0);
			}
		}

		private static void WriteCharShapeFromMessage(CharacterShape _shape, Message _message)
		{
			if (_shape == null)
			{
				_shape = new CharacterShape();
			}
			if (_shape.shapeSet != null && _shape.shapeSet.Length > 0)
			{
				_message.WriteS32(_shape.shapeSet.Length);
				CharacterShapeSet[] shapeSet = _shape.shapeSet;
				foreach (CharacterShapeSet characterShapeSet in shapeSet)
				{
					if (characterShapeSet != null)
					{
						_message.WriteS32(characterShapeSet.shapeId);
						_message.WriteU8(characterShapeSet.shapeCount);
						_message.WriteU8(characterShapeSet.collectBitFlag);
					}
					else
					{
						_message.WriteS32(0);
						_message.WriteU8(0);
						_message.WriteU8(0);
					}
				}
			}
			else
			{
				_message.WriteS32(0);
			}
		}

		private static void WriteServiceToMessage(CharacterService _service, Message _message)
		{
			if (_service == null)
			{
				_service = new CharacterService();
			}
			_message.WriteU8(_service.nsrespawncount);
			_message.WriteS32(_service.nslastrespawnday);
			_message.WriteU8(_service.nsbombcount);
			_message.WriteS32(_service.nsbombday);
			_message.WriteS32(_service.nsgiftreceiveday);
			_message.WriteS32(_service.apgiftreceiveday);
		}

		private static void WritePVPToMessage(CharacterPVP _pvp, Message _message)
		{
			if (_pvp == null)
			{
				_pvp = new CharacterPVP();
			}
			_message.WriteS64(_pvp.winCnt);
			_message.WriteS64(_pvp.loseCnt);
			_message.WriteS32(_pvp.penaltyPoint);
		}

		private static void WriteFarmToMessage(CharacterFarm _farm, Message _message)
		{
			if (_farm == null)
			{
				_farm = new CharacterFarm();
			}
			_message.WriteS64(_farm.farmID);
		}

		private static void WriteHeartStickerToMessage(CharacterHeartSticker _heartSticker, Message _message)
		{
			if (_heartSticker == null)
			{
				_heartSticker = new CharacterHeartSticker();
			}
			_message.WriteS64(_heartSticker.heartUpdateTime);
			_message.WriteS16(_heartSticker.heartPoint);
			_message.WriteS16(_heartSticker.heartTotalPoint);
		}

		private static void WriteJoustToMessage(CharacterJoust _joust, Message _message)
		{
			if (_joust == null)
			{
				_joust = new CharacterJoust();
			}
			_message.WriteS32(_joust.joustPoint);
			_message.WriteU8(_joust.joustLastWinYear);
			_message.WriteU8(_joust.joustLastWinWeek);
			_message.WriteU8(_joust.joustWeekWinCount);
			_message.WriteS16(_joust.joustDailyWinCount);
			_message.WriteS16(_joust.joustDailyLoseCount);
			_message.WriteS16(_joust.joustServerWinCount);
			_message.WriteS16(_joust.joustServerLoseCount);
		}

		private static void WriteAchievementToMessage(CharacterAchievements _achievement, Message _message)
		{
			if (_achievement == null)
			{
				_achievement = new CharacterAchievements();
			}
			_message.WriteS32(_achievement.totalscore);
			if (_achievement.achievement != null && _achievement.achievement.Length > 0)
			{
				_message.WriteS32(_achievement.achievement.Length);
				CharacterAchievementsAchievement[] achievement = _achievement.achievement;
				foreach (CharacterAchievementsAchievement characterAchievementsAchievement in achievement)
				{
					if (characterAchievementsAchievement != null)
					{
						_message.WriteS16(characterAchievementsAchievement.setid);
						_message.WriteS32(characterAchievementsAchievement.bitflag);
					}
					else
					{
						_message.WriteS16(0);
						_message.WriteS32(0);
					}
				}
			}
			else
			{
				_message.WriteS32(0);
			}
		}

		private static void WritePrivateFarmToMessage(CharacterPrivateFarm _privateFarm, Message _message)
		{
			if (_privateFarm == null)
			{
				_privateFarm = new CharacterPrivateFarm();
			}
			if (_privateFarm.favoriteFarm != null && _privateFarm.favoriteFarm.Length > 0)
			{
				_message.WriteS32(_privateFarm.favoriteFarm.Length);
				CharacterFavoritePrivateFarm[] favoriteFarm = _privateFarm.favoriteFarm;
				foreach (CharacterFavoritePrivateFarm characterFavoritePrivateFarm in favoriteFarm)
				{
					if (characterFavoritePrivateFarm != null)
					{
						_message.WriteS64(characterFavoritePrivateFarm.privateFarmId);
						_message.WriteS32(characterFavoritePrivateFarm.themeId);
						_message.WriteU16(characterFavoritePrivateFarm.posX);
						_message.WriteU16(characterFavoritePrivateFarm.posY);
						_message.WriteString(characterFavoritePrivateFarm.farmName);
						_message.WriteString(characterFavoritePrivateFarm.ownerName);
					}
					else
					{
						_message.WriteS64(0L);
						_message.WriteS32(0);
						_message.WriteU16(0);
						_message.WriteU16(0);
						_message.WriteString(string.Empty);
						_message.WriteString(string.Empty);
					}
				}
			}
			else
			{
				_message.WriteS32(0);
			}
		}

		private static void WriteMacroCheckerToMessage(CharacterMacroChecker _macro, Message _message)
		{
			if (_macro == null)
			{
				_macro = new CharacterMacroChecker();
			}
			_message.WriteS32(_macro.macroPoint);
		}

		private static void WriteDonationToMessage(CharacterDonation _donation, Message _message)
		{
			if (_donation == null)
			{
				_donation = new CharacterDonation();
			}
			_message.WriteS32(_donation.donationValue);
			_message.WriteS64(_donation.donationUpdate);
		}
	}
}
