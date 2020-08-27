using Mabinogi.SQL;
using System;
using System.Collections.Generic;

namespace XMLDB3
{
	internal class MyKnightsObjectBuilder
	{
		
		public static CharacterMyKnights BuildCharacterMyKnights(SimpleReader reader)
		{
            CharacterMyKnights characterMyKnights = new CharacterMyKnights();
            characterMyKnights.name = string.Empty;
            characterMyKnights.memberList = new CharacterMyKnightsMember[0];
            characterMyKnights.makeTime = new DateTime(0L);

			if (reader.Read())
			{
				characterMyKnights.name = reader.GetString(Mabinogi.SQL.Columns.CharacterMyKnights.Name);
				characterMyKnights.level = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.CharacterMyKnights.Level);
				characterMyKnights.exp = (uint)reader.GetInt32(Mabinogi.SQL.Columns.CharacterMyKnights.Experience);
				characterMyKnights.trainingPoint = (uint)reader.GetInt32(Mabinogi.SQL.Columns.CharacterMyKnights.Point);
				characterMyKnights.dateBuffMember = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.CharacterMyKnights.DateBuffMember);
				characterMyKnights.makeTime = reader.GetDateTime(Mabinogi.SQL.Columns.CharacterMyKnights.CreateDate);
				characterMyKnights.addedSlotCount = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.CharacterMyKnights.AddedSlotCount);
			}
			else
			{
				characterMyKnights.level = 0;
				characterMyKnights.exp = 0u;
				characterMyKnights.trainingPoint = 0u;
				characterMyKnights.dateBuffMember = 0;
				characterMyKnights.addedSlotCount = 0;
			}
			return characterMyKnights;
        }
		
		public static void BuildMyKnightsMembers(SimpleReader reader, CharacterMyKnights myKnights)
		{
			if (!reader.HasRows)
			{
				myKnights.memberList = new CharacterMyKnightsMember[0];
				return;
			}

			List<CharacterMyKnightsMember> arrayList = new List<CharacterMyKnightsMember>();
			CharacterMyKnightsMember characterMyKnightsMember;
			while (reader.Read())
			{
				characterMyKnightsMember = new CharacterMyKnightsMember();
				characterMyKnightsMember.latestDateList = string.Empty;
				characterMyKnightsMember.id = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.CharacterMyKnightsMember.KnightId);
				characterMyKnightsMember.isRecruited = reader.GetByte(Mabinogi.SQL.Columns.CharacterMyKnightsMember.IsMine);
				characterMyKnightsMember.holy = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Holy);
				characterMyKnightsMember.strength = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Strength);
				characterMyKnightsMember.intelligence = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Intelligence);
				characterMyKnightsMember.dexterity = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Dexterity);
				characterMyKnightsMember.will = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Will);
				characterMyKnightsMember.luck = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Luck);
				characterMyKnightsMember.favorLevel = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.CharacterMyKnightsMember.FavorLvl);
				characterMyKnightsMember.favorExp = (uint)reader.GetInt32(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Favor);
				characterMyKnightsMember.stress = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.CharacterMyKnightsMember.FavorLvl);
				characterMyKnightsMember.woundTime = (ulong)reader.GetInt64(Mabinogi.SQL.Columns.CharacterMyKnightsMember.WoundTime);
				characterMyKnightsMember.isSelfCured = reader.GetByte(Mabinogi.SQL.Columns.CharacterMyKnightsMember.IsSelfCured);
				characterMyKnightsMember.curTraining = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.CharacterMyKnightsMember.CurrentTraining);
				characterMyKnightsMember.trainingStartTime = (ulong)reader.GetInt64(Mabinogi.SQL.Columns.CharacterMyKnightsMember.StartTrainingTime);
				characterMyKnightsMember.curTask = (ulong)reader.GetInt64(Mabinogi.SQL.Columns.CharacterMyKnightsMember.CurrentCommand);
				characterMyKnightsMember.curTaskTemplate = (uint)reader.GetInt32(Mabinogi.SQL.Columns.CharacterMyKnightsMember.CurrentCommandTemplate);
				characterMyKnightsMember.taskEndTime = (ulong)reader.GetInt64(Mabinogi.SQL.Columns.CharacterMyKnightsMember.CommandEndTime);
				characterMyKnightsMember.restStartTime = (ulong)reader.GetInt64(Mabinogi.SQL.Columns.CharacterMyKnightsMember.RestStartTime);
				characterMyKnightsMember.lastDateTime = (ulong)reader.GetInt64(Mabinogi.SQL.Columns.CharacterMyKnightsMember.LastDateTime);
				characterMyKnightsMember.firstRecruitTime = (ulong)reader.GetInt64(Mabinogi.SQL.Columns.CharacterMyKnightsMember.FirstScoutTime);
				characterMyKnightsMember.lastRecruitTime = (ulong)reader.GetInt64(Mabinogi.SQL.Columns.CharacterMyKnightsMember.LastScoutTime);
				characterMyKnightsMember.lastDismissTime = (ulong)reader.GetInt64(Mabinogi.SQL.Columns.CharacterMyKnightsMember.LastReleaseTime);
				characterMyKnightsMember.dismissCount = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.CharacterMyKnightsMember.ReleaseCount);
				characterMyKnightsMember.taskTryCount = (uint)reader.GetInt32(Mabinogi.SQL.Columns.CharacterMyKnightsMember.CommandTryCount);
				characterMyKnightsMember.taskSuccessCount = (uint)reader.GetInt32(Mabinogi.SQL.Columns.CharacterMyKnightsMember.CommandSuccessCount);
				characterMyKnightsMember.favorTalkCount = (ulong)reader.GetInt64(Mabinogi.SQL.Columns.CharacterMyKnightsMember.FavorTalkCount);
				characterMyKnightsMember.latestDateList = reader.GetString(Mabinogi.SQL.Columns.CharacterMyKnightsMember.DateList);
				arrayList.Add(characterMyKnightsMember);
			}
			myKnights.memberList = arrayList.ToArray();
		}
	}
}
