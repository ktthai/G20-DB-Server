using Mabinogi;

namespace XMLDB3
{
	internal class MabiNovelSerializer
	{
		public static PageData Serialize(Message _msg)
		{
			PageData pageData = new PageData();
			pageData.sn = 0L;
			pageData.page = _msg.ReadU16();
			pageData.bgId = _msg.ReadU16();
			pageData.bgmId = _msg.ReadU16();
			pageData.portraitId = _msg.ReadU16();
			pageData.portraitPos = _msg.ReadU16();
			pageData.emotionId = _msg.ReadU16();
			pageData.soundEffectId = _msg.ReadU16();
			pageData.effectId = _msg.ReadU16();
			pageData.ambassador = _msg.ReadString();
			return pageData;
		}

		public static Message Deserialize(PageData _page, Message _msg)
		{
			_msg.WriteU16(_page.page);
			_msg.WriteU16(_page.bgId);
			_msg.WriteU16(_page.bgmId);
			_msg.WriteU16(_page.portraitId);
			_msg.WriteU16(_page.portraitPos);
			_msg.WriteU16(_page.emotionId);
			_msg.WriteU16(_page.soundEffectId);
			_msg.WriteU16(_page.effectId);
			_msg.WriteString(_page.ambassador);
			return _msg;
		}
	}
}
