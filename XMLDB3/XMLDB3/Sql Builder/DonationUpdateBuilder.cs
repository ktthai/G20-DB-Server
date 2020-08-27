using Mabinogi.SQL;

namespace XMLDB3
{
	public class DonationUpdateBuilder
	{
		public static bool Build(Character _new, Character _old, SimpleCommand cmd)
		{
			bool result = false;
			if (_new.donation != null && _old.donation != null)
			{
				if (_new.donation.donationValue != _old.donation.donationValue)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.DonationValue, _new.donation.donationValue);
                    result = true;
                }
				if (_new.donation.donationUpdate != _old.donation.donationUpdate)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.DonationUpdate , _new.donation.donationUpdate);
                    result = true;
                }
			}
			return result;
		}
	}
}
