using Mabinogi;

namespace Authenticator
{
	public class RemoteFunction_CouponTranBegin : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			string couponid = base.Input.ReadString();
			string account = base.Input.ReadString();
			long characterid = base.Input.ReadS64();
			string charactername = base.Input.ReadString();
			string server = base.Input.ReadString();
			long _itemid = base.Input.ReadS64();
			int _coupontype = 0;
			switch (Coupon.Begin_UsingTransaction(couponid, account, characterid, charactername, server, ref _itemid, ref _coupontype))
			{
			case Coupon.RESULT.FAIL:
				newReply.WriteU8(0);
				break;
			case Coupon.RESULT.SUCCESS:
				newReply.WriteU8(1);
				newReply.WriteS32(_coupontype);
				break;
			case Coupon.RESULT.NOT_VALID_STATE:
				newReply.WriteU8(2);
				newReply.WriteS64(_itemid);
				break;
			case Coupon.RESULT.EXPIRED:
				newReply.WriteU8(3);
				break;
			case Coupon.RESULT.USED:
				newReply.WriteU8(5);
				break;
			case Coupon.RESULT.NOT_EXIST:
				newReply.WriteU8(6);
				break;
			case Coupon.RESULT.SERVER_MISMATCH:
				newReply.WriteU8(7);
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
			string text4 = _input.ReadString();
			long num2 = _input.ReadS64();
			return "Coupon Using Transaction Begin (coupon:" + text + "@" + text2 + ", target:" + text3 + "<" + num + ">@" + text4 + ",item:" + num2 + ")";
		}
	}
}
