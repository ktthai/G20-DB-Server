using Mabinogi.SQL;
using System.Collections.Generic;

namespace XMLDB3
{
	public class ShapeObjectBuilder
	{
		public static CharacterShape Build(SimpleReader reader)
		{
			CharacterShape characterShape = new CharacterShape();
			if (reader.HasRows)
			{
				List<CharacterShapeSet> arrayList = new List<CharacterShapeSet>();
				CharacterShapeSet characterShapeSet;
				while (reader.Read())
				{
					characterShapeSet = new CharacterShapeSet();
					characterShapeSet.shapeId = reader.GetInt32(Mabinogi.SQL.Columns.CharacterShape.ShapeId);
					characterShapeSet.shapeCount = reader.GetByte(Mabinogi.SQL.Columns.CharacterShape.Count); 
					characterShapeSet.collectBitFlag = reader.GetByte(Mabinogi.SQL.Columns.CharacterShape.Flag);
					arrayList.Add(characterShapeSet);
				}
				characterShape.shapeSet = arrayList.ToArray();
			}
			return characterShape;
		}
	}
}
