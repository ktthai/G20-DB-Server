using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class GuildCreateBuilder
	{
		public static void Build(Guild _new, SimpleConnection conn, SimpleTransaction transaction)
		{
			if (_new != null)
			{
				long num = CheckGuildMaster(_new.members);
				
				CreateGuild(num, _new, conn, transaction);

				foreach (GuildMember guildMember in _new.members)
				{
					CreateGuildMember(guildMember, _new, conn, transaction);
				}
			}
		}

		private static void CreateGuild(long guildMasterId, Guild newGuild, SimpleConnection conn, SimpleTransaction transaction)
		{
			// PROCEDURE: CreateGuild3

			using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
			{
				DateTime now = DateTime.Now;
				cmd.Set(Mabinogi.SQL.Columns.Guild.Id, newGuild.id);
				cmd.Set(Mabinogi.SQL.Columns.Guild.Name, newGuild.name);
				cmd.Set(Mabinogi.SQL.Columns.Guild.Server, newGuild.server);
				cmd.Set(Mabinogi.SQL.Columns.Guild.GuildPoint, newGuild.guildpoint);
				cmd.Set(Mabinogi.SQL.Columns.Guild.GuildMoney, newGuild.guildmoney);
				cmd.Set(Mabinogi.SQL.Columns.Guild.DrawableMoney, newGuild.drawablemoney);
				cmd.Set(Mabinogi.SQL.Columns.Guild.DrawableDate, now);
				cmd.Set(Mabinogi.SQL.Columns.Guild.GuildType, newGuild.guildtype);
				cmd.Set(Mabinogi.SQL.Columns.Guild.JoinType, newGuild.jointype);
				cmd.Set(Mabinogi.SQL.Columns.Guild.MaxMember, 5); // Default max members
				cmd.Set(Mabinogi.SQL.Columns.Guild.GuildAbility, 0);
				cmd.Set(Mabinogi.SQL.Columns.Guild.CreateTime, now);
				cmd.Set(Mabinogi.SQL.Columns.Guild.UpdateTime, now);
				cmd.Set(Mabinogi.SQL.Columns.Guild.GuildMasterId, guildMasterId);
				cmd.Set(Mabinogi.SQL.Columns.Guild.WebMemberCount, 0);
				cmd.Set(Mabinogi.SQL.Columns.Guild.Expiration, newGuild.expiration);
				cmd.Set(Mabinogi.SQL.Columns.Guild.Enable, newGuild.enable);
				cmd.Execute();
			}

			using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.MabiGuild.GuildMenu, transaction))
			{
				cmd.Set(Mabinogi.SQL.Columns.GuildMenu.GuildId, newGuild.id);
				cmd.Set(Mabinogi.SQL.Columns.GuildMenu.MenuId, 1);
				cmd.Set(Mabinogi.SQL.Columns.GuildMenu.MenuName, "Relatives");
				cmd.Set(Mabinogi.SQL.Columns.GuildMenu.Level1, "1/1/1/1");
				cmd.Set(Mabinogi.SQL.Columns.GuildMenu.Level2, "1/0/1/1");
				cmd.Set(Mabinogi.SQL.Columns.GuildMenu.Level3, "1/0/0/1") ;
				cmd.Set(Mabinogi.SQL.Columns.GuildMenu.Level4, "0/0/0/0");
				cmd.Execute();
			}

            using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.MabiGuild.GuildMenu, transaction))
            {
                cmd.Set(Mabinogi.SQL.Columns.GuildMenu.GuildId, newGuild.id);
                cmd.Set(Mabinogi.SQL.Columns.GuildMenu.MenuId, 2);
                cmd.Set(Mabinogi.SQL.Columns.GuildMenu.MenuName, "Lei wax bully"); //This seems like a reasonable translation
                cmd.Set(Mabinogi.SQL.Columns.GuildMenu.Level1, "1/1/1/1");
                cmd.Set(Mabinogi.SQL.Columns.GuildMenu.Level2, "1/1/1/1");
                cmd.Set(Mabinogi.SQL.Columns.GuildMenu.Level3, "1/1/1/1");
                cmd.Set(Mabinogi.SQL.Columns.GuildMenu.Level4, "0/0/0/0");
                cmd.Execute();
            }

            using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.MabiGuild.GuildText, transaction))
            {
                cmd.Set(Mabinogi.SQL.Columns.GuildText.GuildId, newGuild.id);
                cmd.Set(Mabinogi.SQL.Columns.GuildText.Profile, newGuild.profile);
                cmd.Set(Mabinogi.SQL.Columns.GuildText.Greeting, newGuild.greeting);
                cmd.Set(Mabinogi.SQL.Columns.GuildText.Leaving, newGuild.leaving);
                cmd.Set(Mabinogi.SQL.Columns.GuildText.Refuse, newGuild.refuse);
                cmd.Set(Mabinogi.SQL.Columns.GuildText.Emblem, string.Empty);
                cmd.Execute();
            }
        }

		private static void CreateGuildMember(GuildMember member, Guild guild, SimpleConnection conn, SimpleTransaction transaction)
		{
			using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.GuildMember, transaction))
			{
				cmd.Where(Mabinogi.SQL.Columns.GuildMember.Id, member.memberid);
				cmd.Set(Mabinogi.SQL.Columns.GuildMember.Id, 0);

				using (var reader = cmd.ExecuteReader())
					if (reader.HasRows)
						return;
			}

			GuildAdapter.CheckGuildMemberJointime(member.memberid, guild.server, conn);

			using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.MabiGuild.GuildMember, transaction))
			{
				cmd.Set(Mabinogi.SQL.Columns.GuildMember.Id, member.memberid);
				cmd.Set(Mabinogi.SQL.Columns.GuildMember.GuildId, guild.id);
				cmd.Set(Mabinogi.SQL.Columns.GuildMember.Name, member.name);
				cmd.Set(Mabinogi.SQL.Columns.GuildMember.Account, member.account);
				cmd.Set(Mabinogi.SQL.Columns.GuildMember.Class, member.@class);
				cmd.Set(Mabinogi.SQL.Columns.GuildMember.Point, member.point);
				cmd.Set(Mabinogi.SQL.Columns.GuildMember.JoinTime, DateTime.Now);
				cmd.Set(Mabinogi.SQL.Columns.GuildMember.Text, string.Empty);
				cmd.Set(Mabinogi.SQL.Columns.GuildMember.JoinMsg, string.Empty);

				cmd.Execute();
			}
		}

		private static long CheckGuildMaster(List<GuildMember> _guildmembers)
		{
			long num = 0L;
			bool flag = false;
			foreach (GuildMember guildMember in _guildmembers)
			{
				if (guildMember.@class == 0)
				{
					if (flag)
					{
						throw new Exception("두 명 이상의 길드 마스터가 존재합니다.");
					}
					num = guildMember.memberid;
				}
			}
			if (num == 0)
			{
				throw new Exception("길드 마스터가 존재하지 않습니다.");
			}
			return num;
		}
	}
}
