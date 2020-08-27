using Mabinogi.SQL;
using System;
using System.Collections;

namespace XMLDB3
{
	internal class MyKnightsUpdateBuilder
	{
		public static void Build(Character _new, Character _old, SimpleConnection conn, SimpleTransaction transaction)
		{
			if (_new.myKnights == null || _new.myKnights.memberList == null)
			{
				return;
			}

			if (!_new.myKnights.IsSame(_old.myKnights))
			{
				// PROCEDURE: UpdateCharacterMyKnights
				using(var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterMyKnights, transaction))
				{
					DateTime now = DateTime.Now;
					upCmd.Where(Mabinogi.SQL.Columns.CharacterMyKnights.CharId, _new.id);

					upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnights.Name, _new.myKnights.name);
					upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnights.Level, _new.myKnights.level);
					upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnights.Experience, _new.myKnights.exp);
					upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnights.Point, _new.myKnights.trainingPoint);
					upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnights.DateBuffMember, _new.myKnights.dateBuffMember);
					upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnights.CreateDate, _new.myKnights.makeTime);
					upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnights.AddedSlotCount, _new.myKnights.addedSlotCount);
					upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnights.UpdateTime, now);

					if (upCmd.Execute() < 1)
					{
						using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterMyKnights, transaction))
						{
							insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnights.CharId, _new.id);
							insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnights.Name, _new.myKnights.name);
							insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnights.Level, _new.myKnights.level);
							insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnights.Experience, _new.myKnights.exp);
							insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnights.Point, _new.myKnights.trainingPoint);
							insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnights.DateBuffMember, _new.myKnights.dateBuffMember);
							insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnights.CreateDate, _new.myKnights.makeTime);
							insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnights.AddedSlotCount, _new.myKnights.addedSlotCount);
							insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnights.UpdateTime, now);

							insCmd.Execute();
						}

                    }
				}
			}

			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable();
			if (_old.myKnights != null && _old.myKnights.memberList != null)
			{
				foreach (CharacterMyKnightsMember characterMyKnightsMember in _old.myKnights.memberList)
				{
					hashtable.Add(characterMyKnightsMember.id, characterMyKnightsMember);
				}
			}

			if (_new.myKnights != null && _new.myKnights.memberList != null)
			{
				foreach (CharacterMyKnightsMember characterMyKnightsMember2 in _new.myKnights.memberList)
				{
					hashtable2.Add(characterMyKnightsMember2.id, characterMyKnightsMember2);
				}
			}

			CharacterMyKnightsMember characterMyKnightsMember4;
			foreach (CharacterMyKnightsMember value in hashtable2.Values)
			{
				characterMyKnightsMember4 = null;
				if (hashtable.Contains(value.id))
				{
					characterMyKnightsMember4 = (CharacterMyKnightsMember)hashtable[value.id];
				}
				if (characterMyKnightsMember4 == null || !value.IsSame(characterMyKnightsMember4))
				{
					// PROCEDURE: UpdateCharacterMyKnightsMember
					using(var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterMyKnightsMember, transaction))
					{
						upCmd.Where(Mabinogi.SQL.Columns.CharacterMyKnightsMember.CharId, _new.id);
						upCmd.Where(Mabinogi.SQL.Columns.CharacterMyKnightsMember.KnightId, value.id);

						upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.IsMine, value.isRecruited);
						upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Holy, value.holy);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Strength, value.strength);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Intelligence, value.intelligence);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Dexterity, value.dexterity);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Will, value.will);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Luck, value.luck);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.FavorLvl, value.favorLevel);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Favor, value.favorExp);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Stress, value.stress);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.WoundTime, value.woundTime);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.IsSelfCured, value.isSelfCured);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.CurrentTraining, value.curTraining);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.StartTrainingTime, value.trainingStartTime);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.CurrentCommand, value.curTask);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.CurrentCommandTemplate, value.curTaskTemplate);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.CommandEndTime, value.taskEndTime);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.RestStartTime, value.restStartTime);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.DateList, value.latestDateList);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.LastDateTime, value.lastDateTime);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.FirstScoutTime, value.firstRecruitTime);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.LastScoutTime, value.lastRecruitTime);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.LastReleaseTime, value.lastDismissTime);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.ReleaseCount, value.dismissCount);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.CommandTryCount, value.taskTryCount);
						upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.FavorTalkCount, value.favorTalkCount);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.CommandSuccessCount, value.taskSuccessCount);

                        if (upCmd.Execute() < 1)
						{
							using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterMyKnightsMember, transaction))
							{
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.CharId, _new.id);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.KnightId, value.id);

								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.IsMine, value.isRecruited);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Holy, value.holy);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Strength, value.strength);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Intelligence, value.intelligence);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Dexterity, value.dexterity);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Will, value.will);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Luck, value.luck);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.FavorLvl, value.favorLevel);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Favor, value.favorExp);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.Stress, value.stress);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.WoundTime, value.woundTime);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.IsSelfCured, value.isSelfCured);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.CurrentTraining, value.curTraining);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.StartTrainingTime, value.trainingStartTime);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.CurrentCommand, value.curTask);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.CurrentCommandTemplate, value.curTaskTemplate);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.CommandEndTime, value.taskEndTime);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.RestStartTime, value.restStartTime);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.DateList, value.latestDateList);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.LastDateTime, value.lastDateTime);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.FirstScoutTime, value.firstRecruitTime);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.LastScoutTime, value.lastRecruitTime);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.LastReleaseTime, value.lastDismissTime);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.ReleaseCount, value.dismissCount);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.CommandTryCount, value.taskTryCount);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.FavorTalkCount, value.favorTalkCount);
								insCmd.Set(Mabinogi.SQL.Columns.CharacterMyKnightsMember.CommandSuccessCount, value.taskSuccessCount);

								insCmd.Execute();
							}
                        }
					}
				}
			}
		}
	}
}
