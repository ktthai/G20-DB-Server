using System;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class QuestUpdateBuilder
	{
		private static QuestComparer comparer;

		static QuestUpdateBuilder()
		{
			comparer = new QuestComparer();
		}

		private static void BuildQuest(CharacterPrivateRegistered _new, CharacterPrivateRegistered _old, long _idchar, SimpleConnection conn, SimpleTransaction transaction)
		{
			if (_new != null && _old != null && _new.id == _old.id)
			{
				using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterQuest, transaction))
				{
					bool update = false;

					if (_new.start != _old.start)
					{
						cmd.Set(Mabinogi.SQL.Columns.CharacterQuest.Start, _new.start);
						update = true;
					}
					if (_new.end != _old.end)
					{
                        cmd.Set(Mabinogi.SQL.Columns.CharacterQuest.End, _new.end);
                        update = true;
                    }
					if (_new.extra != _old.extra)
					{
						cmd.Set(Mabinogi.SQL.Columns.CharacterQuest.Extra, _new.extra);
                        update = true;
                    }

					cmd.Where(Mabinogi.SQL.Columns.CharacterQuest.Id, _idchar);
					cmd.Where(Mabinogi.SQL.Columns.CharacterQuest.QuestId, _new.id);

					if (update)
						cmd.Execute();
					return;
				}
			}

			if (_new != null && _old == null)
			{
				// PROCEDURE: AddCharacterQuest
				using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterQuest, transaction))
				{
					cmd.Set(Mabinogi.SQL.Columns.CharacterQuest.Start, _new.start);
					cmd.Set(Mabinogi.SQL.Columns.CharacterQuest.End, _new.end);
					cmd.Set(Mabinogi.SQL.Columns.CharacterQuest.Extra, _new.extra);
					cmd.Set(Mabinogi.SQL.Columns.CharacterQuest.Id, _idchar);
					cmd.Set(Mabinogi.SQL.Columns.CharacterQuest.QuestId, _new.id);

					cmd.Execute();
				}
			}
		}

		public static void Build(Character _new, Character _old, SimpleConnection conn, SimpleTransaction transaction)
		{
			CharacterPrivateRegistered[] array = _new.Private.registereds;
			CharacterPrivateRegistered[] array2 = _old.Private.registereds;

			if (array == null)
			{
				array = new CharacterPrivateRegistered[0];
			}
			if (array2 == null)
			{
				array2 = new CharacterPrivateRegistered[0];
			}
			Array.Sort(array, comparer);
			Array.Sort(array2, comparer);
			int num = 0;
			int num2 = 0;
			num = 0;
			num2 = 0;

			while (num < array.Length && num2 < array2.Length)
			{
				if (array[num].id == array2[num2].id)
				{
					BuildQuest(array[num], array2[num2], _new.id, conn, transaction);
					num++;
					num2++;
				}
				else if (array[num].id > array2[num2].id)
				{
					DeleteCharacterQuest(_new.id, array2[num2].id, conn, transaction);
					num2++;
				}
				else
				{
					BuildQuest(array[num], null, _new.id, conn, transaction);
					num++;
				}
			}

			for (; num < array.Length; num++)
			{
				BuildQuest(array[num], null, _new.id, conn, transaction);
			}
			for (; num2 < array2.Length; num2++)
			{
				DeleteCharacterQuest( _new.id, array2[num2].id, conn, transaction);
			}
		}

		private static void DeleteCharacterQuest(long id, int questId, SimpleConnection conn, SimpleTransaction transaction)
		{
			// PROCEDURE: DeleteCharacterQuest

			using(var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterQuest, transaction))
			{
				cmd.Where(Mabinogi.SQL.Columns.CharacterQuest.Id, id);
				cmd.Where(Mabinogi.SQL.Columns.CharacterQuest.QuestId, questId);

				cmd.Execute();
			}
		}
	}
}
