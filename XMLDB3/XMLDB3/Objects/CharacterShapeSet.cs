
public class CharacterShapeSet
{
	public int shapeId;

	public byte shapeCount;

	public byte collectBitFlag;

	public bool IsSame(CharacterShapeSet a)
	{
		if (shapeId == a.shapeId && shapeCount == a.shapeCount && collectBitFlag == a.collectBitFlag)
		{
			return true;
		}
		return false;
	}
}
