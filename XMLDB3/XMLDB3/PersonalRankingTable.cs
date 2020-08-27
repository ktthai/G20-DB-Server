using System;
using System.Collections.Generic;
using System.Linq;

namespace XMLDB3
{
	public class PersonalRankingTable
	{
		public class ScorePair
		{
			private ulong charId_;

			private int score_;

			public ulong CharId => charId_;

			public int Score => score_;

			public ScorePair(ulong _charId, int _score)
			{
				charId_ = _charId;
				score_ = _score;
			}
		}

		private const long TABLE_REBUILD_TIMEOUT = 1200000000L;

		private static Dictionary<PersonalRanking.RankIdTimePair, PersonalRankingTable> rankingTableCache_ = new Dictionary<PersonalRanking.RankIdTimePair, PersonalRankingTable>();

		private static Dictionary<PersonalRanking.RankIdTimePair, DateTime> lastUpdateTime_ = new Dictionary<PersonalRanking.RankIdTimePair, DateTime>();

		private Dictionary<uint, List<ScorePair>> rankingMap_;

		public Dictionary<uint, List<ScorePair>> RankingMap => rankingMap_;

		public static PersonalRankingTable GetTable(PersonalRankingAdapter _adapter, PersonalRanking.EId _rankingId, PersonalRanking.EScoreSortType _sortType, DateTime _rankingCycleStartTime, DateTime? _rankingCycleEndTime, DateTime _currentTime)
		{
			PersonalRanking.RankIdTimePair key = new PersonalRanking.RankIdTimePair(_rankingId, _rankingCycleStartTime);
			if (!lastUpdateTime_.ContainsKey(key))
			{
				lastUpdateTime_[key] = DateTime.MinValue;
			}
			DateTime dateTime = _currentTime;
			if (dateTime > _rankingCycleStartTime && (dateTime.Ticks - lastUpdateTime_[key].Ticks > 1200000000 || dateTime.Ticks - lastUpdateTime_[key].Ticks < 0))
			{
				rankingTableCache_[key] = RebuildTable(_adapter, _rankingId, _sortType, _rankingCycleStartTime, _rankingCycleEndTime, dateTime);
			}
			if (!rankingTableCache_.ContainsKey(key))
			{
				return null;
			}
			return rankingTableCache_[key];
		}

		public static PersonalRankingTable RebuildTable(PersonalRankingAdapter _adapter, PersonalRanking.EId _rankingId, PersonalRanking.EScoreSortType _sortType, DateTime _rankingCycleStartTime, DateTime? _rankingCycleEndTime, DateTime _currentTime)
		{
			PersonalRanking.RankIdTimePair key = new PersonalRanking.RankIdTimePair(_rankingId, _rankingCycleStartTime);
			rankingTableCache_[key] = null;
			PersonalRankingTable personalRankingTable = new PersonalRankingTable();
			PersonalRankingList srcDataList = _adapter.ReadList(_rankingId, _rankingCycleStartTime, _rankingCycleEndTime);
			ProcessRankingData(_sortType, srcDataList, personalRankingTable);
			if (personalRankingTable != null)
			{
				lastUpdateTime_[key] = _currentTime;
			}
			return personalRankingTable;
		}

		public static void ClearCache(PersonalRanking.EId _rankingId)
		{
			if (rankingTableCache_ == null)
			{
				rankingTableCache_ = new Dictionary<PersonalRanking.RankIdTimePair, PersonalRankingTable>();
				return;
			}
			if (lastUpdateTime_ == null)
			{
				lastUpdateTime_ = new Dictionary<PersonalRanking.RankIdTimePair, DateTime>();
				return;
			}
			PersonalRanking.RankIdTimePair[] array = (from item in rankingTableCache_.AsParallel()
				where item.Key.RankingId == _rankingId
				select item.Key).ToArray();
			PersonalRanking.RankIdTimePair[] array2 = (from item in lastUpdateTime_.AsParallel()
				where item.Key.RankingId == _rankingId
				select item.Key).ToArray();
			PersonalRanking.RankIdTimePair[] array3 = array;
			foreach (PersonalRanking.RankIdTimePair key in array3)
			{
				rankingTableCache_[key] = null;
			}
			PersonalRanking.RankIdTimePair[] array4 = array2;
			foreach (PersonalRanking.RankIdTimePair key2 in array4)
			{
				lastUpdateTime_[key2] = DateTime.MinValue;
			}
		}

		public static void ProcessRankingData(PersonalRanking.EScoreSortType _sortType, PersonalRankingList _srcDataList, PersonalRankingTable _outputTable)
		{
			if (_srcDataList != null && _srcDataList.Items != null)
			{
				List<PersonalRanking> list = null;
				switch (_sortType)
				{
				case PersonalRanking.EScoreSortType.highScoreWin:
					list = (from item in _srcDataList.Items.AsParallel()
						orderby item.score descending
						select item).ToList();
					break;
				case PersonalRanking.EScoreSortType.lowScoreWin:
					list = (from item in _srcDataList.Items.AsParallel()
						orderby item.score
						select item).ToList();
					break;
				}
				uint num = 1u;
				int? num2 = null;
				foreach (PersonalRanking item in list)
				{
					if (num2.HasValue && num2 != item.score)
					{
						num++;
					}
					if (!_outputTable.rankingMap_.ContainsKey(num))
					{
						_outputTable.rankingMap_[num] = new List<ScorePair>();
					}
					_outputTable.rankingMap_[num].Add(new ScorePair(item.charId, item.score));
					num2 = item.score;
				}
			}
		}

		private PersonalRankingTable()
		{
			rankingMap_ = new Dictionary<uint, List<ScorePair>>();
		}
	}
}
