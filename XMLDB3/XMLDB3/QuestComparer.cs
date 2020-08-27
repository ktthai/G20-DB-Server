using System;
using System.Collections;

namespace XMLDB3
{
	public class QuestComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			if (x != null && y != null)
			{
				CharacterPrivateRegistered characterPrivateRegistered = x as CharacterPrivateRegistered;
				CharacterPrivateRegistered characterPrivateRegistered2 = y as CharacterPrivateRegistered;
				if (characterPrivateRegistered == null)
				{
					throw new ArgumentException("Argument Type is not CharacterPrivateRegistered", "x");
				}
				if (characterPrivateRegistered2 == null)
				{
					throw new ArgumentException("Argument Type is not CharacterPrivateRegistered", "y");
				}
				if (characterPrivateRegistered.id == characterPrivateRegistered2.id)
				{
					return 0;
				}
				if (characterPrivateRegistered.id <= characterPrivateRegistered2.id)
				{
					return -1;
				}
				return 1;
			}
			if (x == null && y == null)
			{
				return 0;
			}
			if (x != null)
			{
				return 1;
			}
			return -1;
		}
	}
}
