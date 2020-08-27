using System;
using System.Text.Json;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class CharacterCreateBuilder
    {
        public static void Build(CharacterInfo _new, SimpleConnection conn, SimpleTransaction transaction)
        {
            if (_new != null)
            {
                BuildGameId(_new.id, _new.name, conn, transaction);
                InventoryCreateBuilder.Build(_new.id, _new.inventory, out _new.strToHash, conn, transaction);
                BuildCharacter(_new, conn, transaction);
            }
            else
                throw new ArgumentNullException("CharacterInfo", "캐릭터 데이터가 없습니다.");
        }

        private static void BuildGameId(long _characterId, string _charactername, SimpleConnection conn, SimpleTransaction transaction)
        {
            var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.GameId, transaction);

            cmd.Set(Mabinogi.SQL.Columns.GameId.Id, _characterId);
            cmd.Set(Mabinogi.SQL.Columns.GameId.Name, _charactername);
            cmd.Set(Mabinogi.SQL.Columns.GameId.Flag, 1);

            cmd.Execute();
        }

        private static void BuildCharacter(CharacterInfo _new, SimpleConnection conn, SimpleTransaction transaction)
        {
            var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.Character, transaction);

            cmd.Set(Mabinogi.SQL.Columns.Character.Id, _new.id);
            cmd.Set(Mabinogi.SQL.Columns.Character.Name, _new.name);
            AppearanceInsertBuilder(_new, cmd);
            ParameterInsertBuilder(_new, cmd);
            ParameterExInsertBuilder(_new, cmd);
            UserDataInsertBuilder(_new, cmd);

            if (_new.titles == null)
                cmd.Set(Mabinogi.SQL.Columns.Character.Title, JsonSerializer.Serialize(new CharacterTitles()));
            else
                cmd.Set(Mabinogi.SQL.Columns.Character.Title, JsonSerializer.Serialize(_new.titles));

            if (_new.conditions == null)
                cmd.Set(Mabinogi.SQL.Columns.Character.Condition, JsonSerializer.Serialize(new CharacterCondition[0]));
            else
                cmd.Set(Mabinogi.SQL.Columns.Character.Condition, JsonSerializer.Serialize(_new.conditions));

            if (_new.arbeit != null && _new.arbeit.collection != null)
                cmd.Set(Mabinogi.SQL.Columns.Character.Collection, JsonSerializer.Serialize(_new.arbeit.collection));
            else
                cmd.Set(Mabinogi.SQL.Columns.Character.Collection, JsonSerializer.Serialize(new CharacterArbeitInfo[0]));
            
                

            if (_new.arbeit != null && _new.arbeit.history != null)
                cmd.Set(Mabinogi.SQL.Columns.Character.History, JsonSerializer.Serialize(_new.arbeit.history));
            else
                cmd.Set(Mabinogi.SQL.Columns.Character.History, JsonSerializer.Serialize(new CharacterArbeitDay[0]));

            if (_new.memorys == null)
                cmd.Set(Mabinogi.SQL.Columns.Character.Memory, JsonSerializer.Serialize(new CharacterMemory[0]));
            else
                cmd.Set(Mabinogi.SQL.Columns.Character.Memory, JsonSerializer.Serialize( _new.memorys ));

            if (_new.Private != null && _new.Private.reserveds != null)
                cmd.Set(Mabinogi.SQL.Columns.Character.Reserved, JsonSerializer.Serialize(_new.Private.reserveds));
            else 
                cmd.Set(Mabinogi.SQL.Columns.Character.Reserved, JsonSerializer.Serialize(new CharacterPrivateReserved[0]));


            if (_new.Private != null && _new.Private.books != null)
                cmd.Set(Mabinogi.SQL.Columns.Character.Book, JsonSerializer.Serialize(_new.Private.books));
            else
                cmd.Set(Mabinogi.SQL.Columns.Character.Book, JsonSerializer.Serialize(new CharacterPrivateBook[0]));
            


            cmd.Set(Mabinogi.SQL.Columns.Character.UpdateTime, DateTime.MinValue);
            cmd.Set(Mabinogi.SQL.Columns.Character.DeleteTime, DateTime.MinValue);
            ServiceInsertBuilder(_new, cmd);
            MarriageInsertBuilder(_new, cmd);

            cmd.Set(Mabinogi.SQL.Columns.Character.FarmId, _new.farm.farmID);

            HeartStickerInsertBuilder(_new, cmd);
            JoustInsertBuilder(_new, cmd);

            cmd.Set(Mabinogi.SQL.Columns.Character.MacroPoint, _new.macroChecker.macroPoint);

            cmd.Set(Mabinogi.SQL.Columns.Character.DonationValue, _new.donation.donationValue);
            cmd.Set(Mabinogi.SQL.Columns.Character.DonationUpdate, _new.donation.donationUpdate);

            cmd.Set(Mabinogi.SQL.Columns.Character.JobId, _new.job.jobId);

            cmd.Set(Mabinogi.SQL.Columns.Character.CouponCode, InventoryHashUtility.ComputeHash(_new.strToHash));
            cmd.Execute();


            cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterAchievement, transaction);
            cmd.Set(Mabinogi.SQL.Columns.CharacterAchievement.Id, _new.id);

            if (_new.achievements == null)
                cmd.Set(Mabinogi.SQL.Columns.CharacterAchievement.TotalScore, 0);
            else
                cmd.Set(Mabinogi.SQL.Columns.CharacterAchievement.TotalScore, _new.achievements.totalscore);


            if (_new.achievements != null && _new.achievements.achievement != null)
                cmd.Set(Mabinogi.SQL.Columns.CharacterAchievement.Achievement, JsonSerializer.Serialize(_new.achievements.achievement));
            else
                cmd.Set(Mabinogi.SQL.Columns.CharacterAchievement.Achievement, JsonSerializer.Serialize(new CharacterAchievementsAchievement[0]));
                

            cmd.Execute();
        }

        private static void AppearanceInsertBuilder(Character character, SimpleCommand cmd)
        {
            cmd.Set(Mabinogi.SQL.Columns.Character.Type, character.appearance.type);
            cmd.Set(Mabinogi.SQL.Columns.Character.SkinColor, character.appearance.skin_color);
            cmd.Set(Mabinogi.SQL.Columns.Character.EyeType, character.appearance.eye_type);
            cmd.Set(Mabinogi.SQL.Columns.Character.EyeColor, character.appearance.eye_color);
            cmd.Set(Mabinogi.SQL.Columns.Character.MouthType, character.appearance.mouth_type);
            cmd.Set(Mabinogi.SQL.Columns.Character.Status, character.appearance.status);
            cmd.Set(Mabinogi.SQL.Columns.Character.Height, character.appearance.height);
            cmd.Set(Mabinogi.SQL.Columns.Character.Fatness, character.appearance.fatness);
            cmd.Set(Mabinogi.SQL.Columns.Character.Upper, character.appearance.upper);
            cmd.Set(Mabinogi.SQL.Columns.Character.Lower, character.appearance.lower);
            cmd.Set(Mabinogi.SQL.Columns.Character.Region, character.appearance.region);
            cmd.Set(Mabinogi.SQL.Columns.Character.X, character.appearance.x);
            cmd.Set(Mabinogi.SQL.Columns.Character.Y, character.appearance.y);
            cmd.Set(Mabinogi.SQL.Columns.Character.Direction, character.appearance.direction);
            cmd.Set(Mabinogi.SQL.Columns.Character.BattleState, character.appearance.battle_state);
            cmd.Set(Mabinogi.SQL.Columns.Character.WeaponSet, character.appearance.weapon_set);
        }

        private static void ParameterInsertBuilder(Character character, SimpleCommand cmd)
        {
            cmd.Set(Mabinogi.SQL.Columns.Character.Life, character.parameter.life);
            cmd.Set(Mabinogi.SQL.Columns.Character.LifeDamage, character.parameter.life_damage);
            cmd.Set(Mabinogi.SQL.Columns.Character.LifeMax, character.parameter.life_max);
            cmd.Set(Mabinogi.SQL.Columns.Character.Mana, character.parameter.mana);
            cmd.Set(Mabinogi.SQL.Columns.Character.ManaMax, character.parameter.mana_max);

            cmd.Set(Mabinogi.SQL.Columns.Character.Stamina, character.parameter.stamina);
            cmd.Set(Mabinogi.SQL.Columns.Character.StaminaMax, character.parameter.stamina_max);
            cmd.Set(Mabinogi.SQL.Columns.Character.Food, character.parameter.food);
            cmd.Set(Mabinogi.SQL.Columns.Character.Level, character.parameter.level);
            cmd.Set(Mabinogi.SQL.Columns.Character.CumulatedLevel, character.parameter.cumulatedlevel);

            cmd.Set(Mabinogi.SQL.Columns.Character.MaxLevel, character.parameter.maxlevel);
            cmd.Set(Mabinogi.SQL.Columns.Character.RebirthCount, character.parameter.rebirthcount);
            cmd.Set(Mabinogi.SQL.Columns.Character.LifetimeSkill, character.parameter.lifetimeskill);
            cmd.Set(Mabinogi.SQL.Columns.Character.Experience, character.parameter.experience);
            cmd.Set(Mabinogi.SQL.Columns.Character.Age, character.parameter.age);

            cmd.Set(Mabinogi.SQL.Columns.Character.Strength, character.parameter.strength);
            cmd.Set(Mabinogi.SQL.Columns.Character.Dexterity, character.parameter.dexterity);
            cmd.Set(Mabinogi.SQL.Columns.Character.Intelligence, character.parameter.intelligence);
            cmd.Set(Mabinogi.SQL.Columns.Character.Will, character.parameter.will);
            cmd.Set(Mabinogi.SQL.Columns.Character.Luck, character.parameter.luck);

            cmd.Set(Mabinogi.SQL.Columns.Character.LifeMaxByFood, character.parameter.life_max_by_food);
            cmd.Set(Mabinogi.SQL.Columns.Character.ManaMaxByFood, character.parameter.mana_max_by_food);
            cmd.Set(Mabinogi.SQL.Columns.Character.StaminaMaxByFood, character.parameter.stamina_max_by_food);
            cmd.Set(Mabinogi.SQL.Columns.Character.StrengthByFood, character.parameter.strength_by_food);
            cmd.Set(Mabinogi.SQL.Columns.Character.DexterityByFood, character.parameter.dexterity_by_food);

            cmd.Set(Mabinogi.SQL.Columns.Character.IntelligenceByFood, character.parameter.intelligence_by_food);
            cmd.Set(Mabinogi.SQL.Columns.Character.WillByFood, character.parameter.will_by_food);
            cmd.Set(Mabinogi.SQL.Columns.Character.LuckByFood, character.parameter.luck_by_food);
            cmd.Set(Mabinogi.SQL.Columns.Character.AbilityRemain, character.parameter.ability_remain);
            cmd.Set(Mabinogi.SQL.Columns.Character.AttackMin, character.parameter.attack_min);

            cmd.Set(Mabinogi.SQL.Columns.Character.AttackMax, character.parameter.attack_max);
            cmd.Set(Mabinogi.SQL.Columns.Character.WAttackMin, character.parameter.wattack_min);
            cmd.Set(Mabinogi.SQL.Columns.Character.WAttackMax, character.parameter.wattack_max);
            cmd.Set(Mabinogi.SQL.Columns.Character.Critical, character.parameter.critical);
            cmd.Set(Mabinogi.SQL.Columns.Character.Protect, character.parameter.protect);

            cmd.Set(Mabinogi.SQL.Columns.Character.Defense, character.parameter.defense);
            cmd.Set(Mabinogi.SQL.Columns.Character.Rate, character.parameter.rate);
            cmd.Set(Mabinogi.SQL.Columns.Character.Rank1, character.parameter.rank1);
            cmd.Set(Mabinogi.SQL.Columns.Character.Rank2, character.parameter.rank2);
            cmd.Set(Mabinogi.SQL.Columns.Character.Score, character.parameter.score);
        }

        private static void ParameterExInsertBuilder(Character character, SimpleCommand cmd)
        {
            cmd.Set(Mabinogi.SQL.Columns.Character.StrengthBoost, character.parameterEx.str_boost);
            cmd.Set(Mabinogi.SQL.Columns.Character.DexterityBoost, character.parameterEx.dex_boost);
            cmd.Set(Mabinogi.SQL.Columns.Character.IntelligenceBoost, character.parameterEx.int_boost);
            cmd.Set(Mabinogi.SQL.Columns.Character.WillBoost, character.parameterEx.will_boost);
            cmd.Set(Mabinogi.SQL.Columns.Character.LuckBoost, character.parameterEx.luck_boost);

            cmd.Set(Mabinogi.SQL.Columns.Character.HeightBoost, character.parameterEx.height_boost);
            cmd.Set(Mabinogi.SQL.Columns.Character.FatnessBoost, character.parameterEx.fatness_boost);
            cmd.Set(Mabinogi.SQL.Columns.Character.UpperBoost, character.parameterEx.upper_boost);
            cmd.Set(Mabinogi.SQL.Columns.Character.LowerBoost, character.parameterEx.lower_boost);
            cmd.Set(Mabinogi.SQL.Columns.Character.LifeBoost, character.parameterEx.life_boost);

            cmd.Set(Mabinogi.SQL.Columns.Character.ManaBoost, character.parameterEx.mana_boost);
            cmd.Set(Mabinogi.SQL.Columns.Character.StaminaBoost, character.parameterEx.stamina_boost);
            cmd.Set(Mabinogi.SQL.Columns.Character.Toxic, character.parameterEx.toxic);
            cmd.Set(Mabinogi.SQL.Columns.Character.ToxicDrunkenTime, character.parameterEx.toxic_drunken_time);
            cmd.Set(Mabinogi.SQL.Columns.Character.ToxicStrength, character.parameterEx.toxic_str);

            cmd.Set(Mabinogi.SQL.Columns.Character.ToxicIntelligence, character.parameterEx.toxic_int);
            cmd.Set(Mabinogi.SQL.Columns.Character.ToxicDexterity, character.parameterEx.toxic_dex);
            cmd.Set(Mabinogi.SQL.Columns.Character.ToxicWill, character.parameterEx.toxic_will);
            cmd.Set(Mabinogi.SQL.Columns.Character.ToxicLuck, character.parameterEx.toxic_luck);
            cmd.Set(Mabinogi.SQL.Columns.Character.LastTown, character.parameterEx.lasttown);

            cmd.Set(Mabinogi.SQL.Columns.Character.LastDungeon, character.parameterEx.lastdungeon);
            cmd.Set(Mabinogi.SQL.Columns.Character.ExploLevel, character.parameterEx.exploLevel);
            cmd.Set(Mabinogi.SQL.Columns.Character.ExploMaxKeyLevel, character.parameterEx.exploMaxKeyLevel);
            cmd.Set(Mabinogi.SQL.Columns.Character.ExploCumLevel, character.parameterEx.exploCumLevel);
            cmd.Set(Mabinogi.SQL.Columns.Character.ExploExp, character.parameterEx.exploExp);

            cmd.Set(Mabinogi.SQL.Columns.Character.DiscoverCount, character.parameterEx.discoverCount);
        }

        private static void UserDataInsertBuilder(Character character, SimpleCommand cmd)
        {
            cmd.Set(Mabinogi.SQL.Columns.Character.NaoFavor, character.data.nao_favor);
            cmd.Set(Mabinogi.SQL.Columns.Character.NaoMemory, character.data.nao_memory);
            cmd.Set(Mabinogi.SQL.Columns.Character.NaoStyle, character.data.nao_style);
            cmd.Set(Mabinogi.SQL.Columns.Character.Playtime, character.data.playtime);
            cmd.Set(Mabinogi.SQL.Columns.Character.Birthday, character.data.birthday);
            cmd.Set(Mabinogi.SQL.Columns.Character.RebirthAge, character.data.rebirthage);
            cmd.Set(Mabinogi.SQL.Columns.Character.RebirthDay, character.data.rebirthday);
            cmd.Set(Mabinogi.SQL.Columns.Character.Wealth, character.data.wealth);
            cmd.Set(Mabinogi.SQL.Columns.Character.WriteCounter, character.data.writeCounter);
        }

        private static void ServiceInsertBuilder(Character character, SimpleCommand cmd)
        {
            cmd.Set(Mabinogi.SQL.Columns.Character.NsRespawnCount, character.service.nsrespawncount);
            cmd.Set(Mabinogi.SQL.Columns.Character.NsLastRespawnDay, character.service.nslastrespawnday);
            cmd.Set(Mabinogi.SQL.Columns.Character.NsGiftReceiveDay, character.service.nsgiftreceiveday);
            cmd.Set(Mabinogi.SQL.Columns.Character.ApGiftReceiveDay, character.service.apgiftreceiveday);
            cmd.Set(Mabinogi.SQL.Columns.Character.NsBombCount, character.service.nsbombcount);
            cmd.Set(Mabinogi.SQL.Columns.Character.NsBombDay, character.service.nsbombday);
        }

        private static void MarriageInsertBuilder(Character character, SimpleCommand cmd)
        {
            cmd.Set(Mabinogi.SQL.Columns.Character.MateId, character.marriage.mateid);
            cmd.Set(Mabinogi.SQL.Columns.Character.MateName, character.marriage.matename);
            cmd.Set(Mabinogi.SQL.Columns.Character.MarriageTime, character.marriage.marriagetime);
            cmd.Set(Mabinogi.SQL.Columns.Character.MarriageCount, character.marriage.marriagecount);
        }

        private static void HeartStickerInsertBuilder(Character character, SimpleCommand cmd)
        {
            cmd.Set(Mabinogi.SQL.Columns.Character.HeartUpdateTime, character.heartSticker.heartUpdateTime);
            cmd.Set(Mabinogi.SQL.Columns.Character.HeartPoint, character.heartSticker.heartPoint);
            cmd.Set(Mabinogi.SQL.Columns.Character.HeartTotalPoint, character.heartSticker.heartTotalPoint);
        }

        private static void JoustInsertBuilder(Character character, SimpleCommand cmd)
        {
            cmd.Set(Mabinogi.SQL.Columns.Character.JoustPoint, character.joust.joustPoint);
            cmd.Set(Mabinogi.SQL.Columns.Character.JoustLastWinYear, character.joust.joustLastWinYear);
            cmd.Set(Mabinogi.SQL.Columns.Character.JoustLastWinWeek, character.joust.joustLastWinWeek);
            cmd.Set(Mabinogi.SQL.Columns.Character.JoustWeekWinCount, character.joust.joustWeekWinCount);
            cmd.Set(Mabinogi.SQL.Columns.Character.JoustDailyWinCount, character.joust.joustDailyWinCount);
            cmd.Set(Mabinogi.SQL.Columns.Character.JoustDailyLoseCount, character.joust.joustDailyLoseCount);
            cmd.Set(Mabinogi.SQL.Columns.Character.JoustServerWinCount, character.joust.joustServerWinCount);
            cmd.Set(Mabinogi.SQL.Columns.Character.JoustServerLoseCount, character.joust.joustServerLoseCount);
        }
    }
}
