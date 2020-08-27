using Mabinogi;
using System;

namespace XMLDB3
{
	public class HelpPointRankSerializer
	{
		public static HelpPointRank Serialize(Message _message)
		{
			HelpPointRank helpPointRank = new HelpPointRank();
			helpPointRank.charId = _message.ReadTypeOf(helpPointRank.charId);
			helpPointRank.charName = _message.ReadTypeOf(helpPointRank.charName);
			helpPointRank.NormalHelpPoint = _message.ReadTypeOf(helpPointRank.NormalHelpPoint);
			helpPointRank.AccumulatedHelpPoint = _message.ReadTypeOf(helpPointRank.AccumulatedHelpPoint);
			helpPointRank.lastUpdate = new DateTime(_message.ReadS64());
			return helpPointRank;
		}

		public static void Deserialize(HelpPointRank _data, Message _message)
		{
			if (_data != null)
			{
				_message.WriteTypeOf(_data.charId);
				_message.WriteTypeOf(_data.charName);
				_message.WriteTypeOf(_data.NormalHelpPoint);
				_message.WriteTypeOf(_data.AccumulatedHelpPoint);
				_message.WriteS64(_data.lastUpdate.Ticks);
			}
		}
	}
}
