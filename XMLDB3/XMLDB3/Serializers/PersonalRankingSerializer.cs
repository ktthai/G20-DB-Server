using Mabinogi;
using System;

namespace XMLDB3
{
	public class PersonalRankingSerializer
	{
		public static PersonalRanking Serialize(Message _message)
		{
			PersonalRanking personalRanking = new PersonalRanking();
			personalRanking.rankingId = _message.ReadTypeOf(personalRanking.rankingId);
			personalRanking.charId = _message.ReadTypeOf(personalRanking.charId);
			personalRanking.score = _message.ReadTypeOf(personalRanking.score);
			personalRanking.lastUpdate = new DateTime((long)(_message.ReadU64() * 10000));
			return personalRanking;
		}

		public static void Deserialize(PersonalRanking _data, Message _message)
		{
			if (_data != null)
			{
				_message.WriteTypeOf(_data.rankingId);
				_message.WriteTypeOf(_data.charId);
				_message.WriteTypeOf(_data.score);
				_message.WriteU64((ulong)_data.lastUpdate.Ticks);
			}
		}
	}
}
