using Mabinogi;
using System.Collections.Generic;

namespace XMLDB3
{
	public class PersonalRankingTableSerializer
	{
		private static Dictionary<ulong, string> charIdNameMapping_ = new Dictionary<ulong, string>();

		public static Dictionary<ulong, string> CharIdNameMap
		{
			get
			{
				return charIdNameMapping_;
			}
			set
			{
				charIdNameMapping_ = value;
			}
		}

		public static void Deserialize(PersonalRankingTable _data, Message _message)
		{
			if (_data == null || _data.RankingMap == null || _data.RankingMap.Count == 0)
			{
				_message.WriteS32(0);
				return;
			}
			_message.WriteS32(_data.RankingMap.Count);
			foreach (KeyValuePair<uint, List<PersonalRankingTable.ScorePair>> item in _data.RankingMap)
			{
				_message.WriteU32(item.Key);
				Deserialize(item.Value, _message);
			}
		}

		public static void Deserialize(List<PersonalRankingTable.ScorePair> _list, Message _message)
		{
			if (_list == null || _list.Count == 0)
			{
				_message.WriteS32(0);
				return;
			}
			_message.WriteS32(_list.Count);
			foreach (PersonalRankingTable.ScorePair item in _list)
			{
				Deserialize(item, _message);
			}
		}

		public static void Deserialize(PersonalRankingTable.ScorePair _data, Message _message)
		{
			if (charIdNameMapping_.ContainsKey(_data.CharId) && charIdNameMapping_[_data.CharId] != null)
			{
				_message.WriteString(charIdNameMapping_[_data.CharId]);
			}
			else
			{
				_message.WriteString("");
			}
			_message.WriteTypeOf(_data.Score);
		}
	}
}
