using Mabinogi;

namespace Authenticator
{
	public class RemoteFunction_CouponTranRollback : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			string couponid = base.Input.ReadString();
			string account = base.Input.ReadString();
			long characterid = base.Input.ReadS64();
			string server = base.Input.ReadString();
			switch (Coupon.Rollback_UsingTransaction(couponid, account, characterid, server))
			{
			case Coupon.RESULT.FAIL:
				newReply.WriteU8(0);
				break;
			case Coupon.RESULT.SUCCESS:
				newReply.WriteU8(1);
				break;
			case Coupon.RESULT.NOT_VALID_STATE:
				newReply.WriteU8(2);
				break;
			case Coupon.RESULT.NOT_VALID_USER:
				newReply.WriteU8(4);
				break;
			case Coupon.RESULT.USED:
				newReply.WriteU8(5);
				break;
			case Coupon.RESULT.NOT_EXIST:
				newReply.WriteU8(6);
				break;
			default:
				newReply.WriteU8(0);
				break;
			}
			Reply(newReply);
			return newReply;
		}

		protected override string BuildName(Message _input)
		{
			string text = _input.ReadString();
			string text2 = _input.ReadString();
			long num = _input.ReadS64();
			string text3 = _input.ReadString();
			return "Coupon Using Transaction Rollback (on Local Test) (coupon:" + text + "@" + text2 + ", target:<" + num + ">@" + text3 + ")";
		}
	}
}
