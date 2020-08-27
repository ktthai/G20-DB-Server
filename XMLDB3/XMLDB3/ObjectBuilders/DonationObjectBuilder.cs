using Mabinogi.SQL;

namespace XMLDB3
{
	public class DonationObjectBuilder
	{
		public static CharacterDonation Build(SimpleReader reader)
		{
			CharacterDonation characterDonation = new CharacterDonation();
			characterDonation.donationValue = reader.GetInt32(Mabinogi.SQL.Columns.Character.DonationValue);
			characterDonation.donationUpdate = reader.GetInt64(Mabinogi.SQL.Columns.Character.DonationUpdate);
			return characterDonation;
		}
	}
}
