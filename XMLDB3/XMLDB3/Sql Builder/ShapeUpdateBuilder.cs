using System;
using System.Collections;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class ShapeUpdateBuilder
	{
		public static void Build(Character _new, Character _old, SimpleConnection conn, SimpleTransaction transaction)
		{
			string text = null;
			if (_old.shape != null && _old.shape.shapeSet != null && _new.shape != null && _new.shape.shapeSet != null)
			{
				Hashtable hashtable = new Hashtable(_old.shape.shapeSet.Length);

				foreach (CharacterShapeSet characterShapeSet in _old.shape.shapeSet)
				{
					hashtable[characterShapeSet.shapeId] = characterShapeSet;
				}

				CharacterShapeSet characterShapeSet3;
				foreach (CharacterShapeSet characterShapeSet in _new.shape.shapeSet)
				{
					characterShapeSet3 = (CharacterShapeSet)hashtable[characterShapeSet.shapeId];
					if (characterShapeSet3 == null || !characterShapeSet3.IsSame(characterShapeSet))
					{
						UpdateCharacterShape(_new.id, characterShapeSet, conn, transaction);
					}
				}
				return;
			}
			if (_new.shape != null && _new.shape.shapeSet != null)
			{
				CharacterShapeSet[] shapeSet3 = _new.shape.shapeSet;
				foreach (CharacterShapeSet characterShapeSet4 in shapeSet3)
				{
					text += $"exec dbo.UpdateCharacterShape @idCharacter={_new.id}, @shapeid={characterShapeSet4.shapeId}, @count={characterShapeSet4.shapeCount}, @flag={characterShapeSet4.collectBitFlag}\n";
				}

				return;
			}
		}

		private static void UpdateCharacterShape(long id, CharacterShapeSet shape, SimpleConnection conn, SimpleTransaction transaction)
		{
			DateTime now = DateTime.Now;
			// PROCEDURE: UpdateCharacterShape
			using(var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterShape, transaction))
			{
				upCmd.Where(Mabinogi.SQL.Columns.CharacterShape.Id, id);
				upCmd.Where(Mabinogi.SQL.Columns.CharacterShape.ShapeId, shape.shapeId);

				upCmd.Set(Mabinogi.SQL.Columns.CharacterShape.Count, shape.shapeCount);
				upCmd.Set(Mabinogi.SQL.Columns.CharacterShape.Flag, shape.collectBitFlag);
				upCmd.Set(Mabinogi.SQL.Columns.CharacterShape.UpdateTime, now);

				if(upCmd.Execute() < 1)
				{
					using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterShape, transaction))
					{
						insCmd.Set(Mabinogi.SQL.Columns.CharacterShape.Id, id);
						insCmd.Set(Mabinogi.SQL.Columns.CharacterShape.ShapeId, shape.shapeId);
						insCmd.Set(Mabinogi.SQL.Columns.CharacterShape.Count, shape.shapeCount);
						insCmd.Set(Mabinogi.SQL.Columns.CharacterShape.Flag, shape.collectBitFlag);
						insCmd.Set(Mabinogi.SQL.Columns.CharacterShape.UpdateTime, now);

						insCmd.Execute();
					}
                }
			}
		}
	}
}
