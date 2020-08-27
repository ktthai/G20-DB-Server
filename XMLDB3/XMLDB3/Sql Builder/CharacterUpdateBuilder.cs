using Mabinogi;
using System;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class CharacterUpdateBuilder
    {
        public static DateTime Build(Character _new, Character _old, SimpleConnection conn, SimpleTransaction transaction, out Message _outBuildResultMsg)
        {
            _outBuildResultMsg = new Message();

            if (_new == null)
            {
                throw new ArgumentException("캐릭터 데이터가 없습니다.", "_new");
            }
            if (_old == null)
            {
                throw new ArgumentException("캐릭터 캐쉬 데이터가 없습니다.", "_old");
            }

            var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Character, transaction);

            bool update = AppearanceUpdateBuilder.Build(_new, _old, cmd);
            if (ParameterUpdateBuilder.Build(_new, _old, cmd))
                update = true;
            if (ParameterExUpdateBuilder.Build(_new, _old, cmd))
                update = true;
            if (UserDataUpdateBuilder.Build(_new, _old, cmd))
                update = true;
            if (TitleUpdateBuilder.Build(_new, _old, cmd))
                update = true;
            if (MarriageUpdateBuilder.Build(_new, _old, cmd))
                update = true;
            if (MemoryUpdateBuilder.Build(_new, _old, cmd, out Message _outBuildResultMsg2))
                update = true;
            _outBuildResultMsg += _outBuildResultMsg2;

            if (ArbeitUpdateBuilder.Build(_new, _old, cmd, out Message _outBuildResultMsg3))
                update = true;
            _outBuildResultMsg += _outBuildResultMsg3;

            if (ConditionUpdateBuilder.Build(_new, _old, cmd, out Message _outBuildResultMsg4))
                update = true;
            _outBuildResultMsg += _outBuildResultMsg4;

            if (PrivateUpdateBuilder.Build(_new, _old, cmd))
                update = true;
            if (ServiceUpdateBuilder.Build(_new, _old, cmd))
                update = true;
            if (FarmUpdateBuilder.Build(_new, _old, cmd))
                update = true;
            if (HeartStickerUpdateBuilder.Build(_new, _old, cmd))
                update = true;
            if (JoustUpdateBuilder.Build(_new, _old, cmd))
                update = true;
            if (MacroCheckerUpdateBuilder.Build(_new, _old, cmd))
                update = true;
            if (DonationUpdateBuilder.Build(_new, _old, cmd))
                update = true;
            if (JobUpdateBuilder.Build(_new, _old, cmd))
                update = true;

            SkillUpdateBuilder.Build(_new, _old, conn, transaction);
            QuestUpdateBuilder.Build(_new, _old, conn, transaction);
            AchievementUpdateBuilder.Build(_new, _old, conn, transaction);
            FavoritePrivateFarmUpdateBuilder.Build(_new, _old, conn, transaction);
            DeedUpdateBuilder.Build(_new, _old, conn, transaction);
            ShapeUpdateBuilder.Build(_new, _old, conn, transaction);
            KeywordUpdateBuilder.Build(_new, _old, conn, transaction);
            DivineKnightUpdateBuilder.Build(_new, _old, conn, transaction);
            SubSkillUpdateBuilder.Build(_new, _old, conn, transaction);
            MyKnightsUpdateBuilder.Build(_new, _old, conn, transaction);

            cmd.Where(Mabinogi.SQL.Columns.Character.Id, _new.id);

            if (ConfigManager.IsPVPable)
            {
                PVPUpdateBuilder.Build(_new, _old, conn, transaction);
            }

            DateTime result = DateTime.Now;
            cmd.Set(Mabinogi.SQL.Columns.Character.UpdateTime, result);
            cmd.Set(Mabinogi.SQL.Columns.Character.CouponCode, _new.inventoryHash);
            // We'd check to see if update = true here, but the inventoryHash and updatetime get written everytime (otherwise, we'd be setting nothing)
            cmd.Execute();
            return result;
        }
    }
}
