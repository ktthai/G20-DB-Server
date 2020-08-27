using System;
using System.Text.Json;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class PetCreateBuilder
    {
        public static void Build(PetInfo _new, SimpleConnection conn, SimpleTransaction transaction)
        {
            if (_new != null)
            {
                BuildGameId(_new.id, _new.name, conn, transaction);
                InventoryCreateBuilder.Build(_new.id, _new.inventory, out _new.strToHash, conn, transaction);
                BuildPet(_new, conn, transaction);
            }
            else
            {
                throw new ArgumentNullException("PetInfo", "팻 데이터가 없습니다.");
            }
        }

        private static void BuildGameId(long _petid, string _petname, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.GameId, transaction))
            {
                cmd.Set(Mabinogi.SQL.Columns.GameId.Id, _petid);
                cmd.Set(Mabinogi.SQL.Columns.GameId.Name, _petname);
                cmd.Set(Mabinogi.SQL.Columns.GameId.Flag, 2);
                cmd.Execute();
            }
        }

        private static void BuildPet(PetInfo _new, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.Pet, transaction))
            {
                cmd.Set(Mabinogi.SQL.Columns.Pet.Id, _new.id);
                cmd.Set(Mabinogi.SQL.Columns.Pet.Name, _new.name);
                BuildAppearance(_new, cmd);
                BuildParameter(_new, cmd);
                BuildParameterEx(_new, cmd);
                BuildData(_new, cmd);
                BuildCondition(_new, cmd);
                BuildMemory(_new, cmd);
                BuildPrivate(_new, cmd);
                BuildSummon(_new, cmd);

                cmd.Set(Mabinogi.SQL.Columns.Pet.UpdateTime, null);
                cmd.Set(Mabinogi.SQL.Columns.Pet.DeleteTime, null);
                cmd.Set(Mabinogi.SQL.Columns.Pet.MacroPoint, _new.macroChecker.macroPoint);
                cmd.Set(Mabinogi.SQL.Columns.Pet.CouponCode, InventoryHashUtility.ComputeHash(_new.strToHash));

                cmd.Execute();
            }
        }

        private static void BuildAppearance(Pet _new, SimpleCommand cmd)
        {
            cmd.Set(Mabinogi.SQL.Columns.Pet.Type, _new.appearance.type);
            cmd.Set(Mabinogi.SQL.Columns.Pet.SkinColor, _new.appearance.skin_color);
            cmd.Set(Mabinogi.SQL.Columns.Pet.EyeColor, _new.appearance.eye_color);
            cmd.Set(Mabinogi.SQL.Columns.Pet.EyeType, _new.appearance.eye_type);
            cmd.Set(Mabinogi.SQL.Columns.Pet.MouthType, _new.appearance.mouth_type);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Status, _new.appearance.status);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Height, _new.appearance.height);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Fatness, _new.appearance.fatness);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Upper, _new.appearance.upper);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Lower, _new.appearance.lower);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Region, _new.appearance.region);
            cmd.Set(Mabinogi.SQL.Columns.Pet.X, _new.appearance.x);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Y, _new.appearance.y);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Direction, _new.appearance.direction);
            cmd.Set(Mabinogi.SQL.Columns.Pet.BattleState, _new.appearance.battle_state);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Extra1, _new.appearance.extra_01);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Extra2, _new.appearance.extra_02);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Extra3, _new.appearance.extra_03);
        }

        private static void BuildParameter(Pet _new, SimpleCommand cmd)
        {
            cmd.Set(Mabinogi.SQL.Columns.Pet.Life, _new.parameter.life);
            cmd.Set(Mabinogi.SQL.Columns.Pet.LifeDamage, _new.parameter.life_damage);
            cmd.Set(Mabinogi.SQL.Columns.Pet.LifeMax, _new.parameter.life_max);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Mana, _new.parameter.mana);
            cmd.Set(Mabinogi.SQL.Columns.Pet.ManaMax, _new.parameter.mana_max);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Stamina, _new.parameter.stamina);
            cmd.Set(Mabinogi.SQL.Columns.Pet.StaminaMax, _new.parameter.stamina_max);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Food, _new.parameter.food);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Level, _new.parameter.level);
            cmd.Set(Mabinogi.SQL.Columns.Pet.CumulatedLevel, _new.parameter.cumulatedlevel);
            cmd.Set(Mabinogi.SQL.Columns.Pet.MaxLevel, _new.parameter.maxlevel);
            cmd.Set(Mabinogi.SQL.Columns.Pet.RebirthCount, _new.parameter.rebirthcount);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Experience, _new.parameter.experience);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Age, _new.parameter.age);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Strength, _new.parameter.strength);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Dexterity, _new.parameter.dexterity);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Intelligence, _new.parameter.intelligence);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Will, _new.parameter.will);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Luck, _new.parameter.luck);
            cmd.Set(Mabinogi.SQL.Columns.Pet.AttackMin, _new.parameter.attack_min);
            cmd.Set(Mabinogi.SQL.Columns.Pet.AttackMax, _new.parameter.attack_max);
            cmd.Set(Mabinogi.SQL.Columns.Pet.WAttackMin, _new.parameter.wattack_min);
            cmd.Set(Mabinogi.SQL.Columns.Pet.WAttackMax, _new.parameter.wattack_max);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Critical, _new.parameter.critical);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Protect, _new.parameter.protect);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Defense, _new.parameter.defense);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Rate, _new.parameter.rate);
        }

        private static void BuildParameterEx(Pet _new, SimpleCommand cmd)
        {
            cmd.Set(Mabinogi.SQL.Columns.Pet.StrengthBoost, _new.parameterEx.str_boost);
            cmd.Set(Mabinogi.SQL.Columns.Pet.DexterityBoost, _new.parameterEx.dex_boost);
            cmd.Set(Mabinogi.SQL.Columns.Pet.IntelligenceBoost, _new.parameterEx.int_boost);
            cmd.Set(Mabinogi.SQL.Columns.Pet.WillBoost, _new.parameterEx.will_boost);
            cmd.Set(Mabinogi.SQL.Columns.Pet.LuckBoost, _new.parameterEx.luck_boost);
            cmd.Set(Mabinogi.SQL.Columns.Pet.HeightBoost, _new.parameterEx.height_boost);
            cmd.Set(Mabinogi.SQL.Columns.Pet.FatnessBoost, _new.parameterEx.fatness_boost);
            cmd.Set(Mabinogi.SQL.Columns.Pet.UpperBoost, _new.parameterEx.upper_boost);
            cmd.Set(Mabinogi.SQL.Columns.Pet.LowerBoost, _new.parameterEx.lower_boost);
            cmd.Set(Mabinogi.SQL.Columns.Pet.LifeBoost, _new.parameterEx.life_boost);
            cmd.Set(Mabinogi.SQL.Columns.Pet.ManaBoost, _new.parameterEx.mana_boost);
            cmd.Set(Mabinogi.SQL.Columns.Pet.StaminaBoost, _new.parameterEx.stamina_boost);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Toxic, _new.parameterEx.toxic);
            cmd.Set(Mabinogi.SQL.Columns.Pet.ToxicDrunkenTime, _new.parameterEx.toxic_drunken_time);
            cmd.Set(Mabinogi.SQL.Columns.Pet.ToxicStrength, _new.parameterEx.toxic_str);
            cmd.Set(Mabinogi.SQL.Columns.Pet.ToxicIntelligence, _new.parameterEx.toxic_int);
            cmd.Set(Mabinogi.SQL.Columns.Pet.ToxicDexterity, _new.parameterEx.toxic_dex);
            cmd.Set(Mabinogi.SQL.Columns.Pet.ToxicWill, _new.parameterEx.toxic_will);
            cmd.Set(Mabinogi.SQL.Columns.Pet.ToxicLuck, _new.parameterEx.toxic_luck);
            cmd.Set(Mabinogi.SQL.Columns.Pet.LastTown, _new.parameterEx.lasttown);
            cmd.Set(Mabinogi.SQL.Columns.Pet.LastDungeon, _new.parameterEx.lastdungeon);
        }

        private static void BuildData(Pet _new, SimpleCommand cmd)
        {
            cmd.Set(Mabinogi.SQL.Columns.Pet.UI, _new.data.ui);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Meta, _new.data.meta);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Birthday, _new.data.birthday);
            cmd.Set(Mabinogi.SQL.Columns.Pet.RebirthDay, _new.data.rebirthday);
            cmd.Set(Mabinogi.SQL.Columns.Pet.RebirthAge, _new.data.rebirthage);
            cmd.Set(Mabinogi.SQL.Columns.Pet.PlayTime, _new.data.playtime);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Wealth, _new.data.wealth);
            cmd.Set(Mabinogi.SQL.Columns.Pet.WriteCounter, _new.data.writeCounter);
        }

        private static void BuildCondition(Pet _new, SimpleCommand cmd)
        {
            string text = JsonSerializer.Serialize(_new.conditions);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Condition, text);
        }

        private static void BuildMemory(Pet _new, SimpleCommand cmd)
        {
            string text = JsonSerializer.Serialize(_new.memorys);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Memory, text);
        }

        private static void BuildPrivate(Pet _new, SimpleCommand cmd)
        {
            string text = JsonSerializer.Serialize(_new.@private.reserveds);
            string text2 = JsonSerializer.Serialize(_new.@private.registereds);

            cmd.Set(Mabinogi.SQL.Columns.Pet.Reserved, text);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Registered, text2);
        }

        private static void BuildSummon(Pet _new, SimpleCommand cmd)
        {
            cmd.Set(Mabinogi.SQL.Columns.Pet.Loyalty, _new.summon.loyalty);
            cmd.Set(Mabinogi.SQL.Columns.Pet.Favor, _new.summon.favor);
        }
    }
}

