using Mabinogi;

namespace XMLDB3
{
	public class RecommendSerializer
	{
		public static void DeserializeOldbie(Recommend _data, Message _message)
		{
			if (_data != null)
			{
				_message.WriteString(_data.oldbieCharName);
				_message.WriteString(_data.oldbieServerId);
				_message.WriteS64(_data.recommendTime);
				_message.WriteS64(_data.flagTime1);
				_message.WriteS64(_data.flagTime2);
				_message.WriteS64(_data.flagTime3);
				_message.WriteS64(_data.flagTime4);
				_message.WriteS64(_data.flagTime5);
				_message.WriteS64(_data.flagTime6);
				_message.WriteS64(_data.flagTime7);
				_message.WriteS64(_data.flagTime8);
				_message.WriteS64(_data.flagTime9);
				_message.WriteS64(_data.flagTime10);
				_message.WriteS64(_data.flagTime11);
				_message.WriteS64(_data.flagTime12);
				_message.WriteS64(_data.flagTime13);
				_message.WriteS64(_data.flagTime14);
				_message.WriteS64(_data.flagTime15);
				_message.WriteS64(_data.flagTime16);
				_message.WriteS64(_data.flagTime17);
				_message.WriteS64(_data.flagTime18);
				_message.WriteS64(_data.flagTime19);
				_message.WriteS64(_data.flagTime20);
				_message.WriteS64(_data.flagTime21);
				_message.WriteS64(_data.flagTime22);
				_message.WriteS64(_data.flagTime23);
				_message.WriteS64(_data.flagTime24);
				_message.WriteS64(_data.flagTime25);
				_message.WriteS64(_data.flagTime26);
				_message.WriteS64(_data.flagTime27);
				_message.WriteS64(_data.flagTime28);
				_message.WriteS64(_data.flagTime29);
			}
		}

		public static void DeserializeNewbie(Recommend _data, Message _message)
		{
			if (_data != null)
			{
				_message.WriteString(_data.newbieCharName);
				_message.WriteString(_data.newbieServerId);
				_message.WriteS64(_data.recommendTime);
				_message.WriteS64(_data.flagTime1);
				_message.WriteS64(_data.flagTime2);
				_message.WriteS64(_data.flagTime3);
				_message.WriteS64(_data.flagTime4);
				_message.WriteS64(_data.flagTime5);
				_message.WriteS64(_data.flagTime6);
				_message.WriteS64(_data.flagTime7);
				_message.WriteS64(_data.flagTime8);
				_message.WriteS64(_data.flagTime9);
				_message.WriteS64(_data.flagTime10);
				_message.WriteS64(_data.flagTime11);
				_message.WriteS64(_data.flagTime12);
				_message.WriteS64(_data.flagTime13);
				_message.WriteS64(_data.flagTime14);
				_message.WriteS64(_data.flagTime15);
				_message.WriteS64(_data.flagTime16);
				_message.WriteS64(_data.flagTime17);
				_message.WriteS64(_data.flagTime18);
				_message.WriteS64(_data.flagTime19);
				_message.WriteS64(_data.flagTime20);
				_message.WriteS64(_data.flagTime21);
				_message.WriteS64(_data.flagTime22);
				_message.WriteS64(_data.flagTime23);
				_message.WriteS64(_data.flagTime24);
				_message.WriteS64(_data.flagTime25);
				_message.WriteS64(_data.flagTime26);
				_message.WriteS64(_data.flagTime27);
				_message.WriteS64(_data.flagTime28);
				_message.WriteS64(_data.flagTime29);
			}
		}
	}
}
