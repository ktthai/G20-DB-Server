using Mabinogi.SQL;
using System;

namespace XMLDB3
{
	public class PetDataObjectBuilder
	{
		public static PetData Build(SimpleReader reader)
		{
			PetData petData = new PetData();
			petData.ui = reader.GetString(Mabinogi.SQL.Columns.Pet.UI);
			petData.meta = reader.GetString(Mabinogi.SQL.Columns.Pet.Meta);
			petData.birthday = reader.GetDateTime(Mabinogi.SQL.Columns.Pet.Birthday);

			DateTime rebirthDay;
			if (reader.GetDateTimeSafe(Mabinogi.SQL.Columns.Pet.RebirthDay, out rebirthDay))
			{
				petData.rebirthday = rebirthDay;
			}
			else
			{
				petData.rebirthday = DateTime.MinValue;
			}

			petData.rebirthage = reader.GetInt16(Mabinogi.SQL.Columns.Pet.RebirthAge);
			petData.playtime = reader.GetInt32(Mabinogi.SQL.Columns.Pet.PlayTime);
			petData.wealth = reader.GetInt32(Mabinogi.SQL.Columns.Pet.Wealth);
			petData.writeCounter = reader.GetByte(Mabinogi.SQL.Columns.Pet.WriteCounter);
			return petData;
		}
	}
}
