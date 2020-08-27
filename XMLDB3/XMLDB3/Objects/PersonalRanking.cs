using System;

public class PersonalRanking
{
	public enum EId
	{
		none,
		girgashiyRaidHardcore
	}

	public enum EScoreSortType
	{
		highScoreWin,
		lowScoreWin
	}

	public class RankIdCharPair : IFormattable
	{
		private EId rankingId_;

		private ulong charId_;

		public EId RankingId => rankingId_;

		public ulong CharId => charId_;

		public RankIdCharPair(EId _rankingId, ulong _charId)
		{
			rankingId_ = _rankingId;
			charId_ = _charId;
		}

		public override string ToString()
		{
			return $"{rankingId_}_{charId_}";
		}

		public string ToString(string _format)
		{
			throw new FormatException("Not Supported");
		}

		public string ToString(string _format, IFormatProvider _provider)
		{
			throw new FormatException("Not Supported");
		}
	}

	public struct RankIdTimePair
	{
		private EId rankingId_;

		private DateTime cycleStartTime_;

		public EId RankingId => rankingId_;

		public DateTime CycleStartTime => cycleStartTime_;

		public RankIdTimePair(EId _rankingId, DateTime _cycleStartTime)
		{
			rankingId_ = _rankingId;
			cycleStartTime_ = _cycleStartTime;
		}
	}

	private uint rankingIdField;

	private ulong charIdField;

	private int scoreField;

	private DateTime lastUpdateField;

	public uint rankingId
	{
		get
		{
			return rankingIdField;
		}
		set
		{
			rankingIdField = value;
		}
	}

	public ulong charId
	{
		get
		{
			return charIdField;
		}
		set
		{
			charIdField = value;
		}
	}

	public int score
	{
		get
		{
			return scoreField;
		}
		set
		{
			scoreField = value;
		}
	}

	public DateTime lastUpdate
	{
		get
		{
			return lastUpdateField;
		}
		set
		{
			lastUpdateField = value;
		}
	}

	public bool IsNewScoreBetter(EScoreSortType _sortType, int _newScore)
	{
		switch (_sortType)
		{
		case EScoreSortType.highScoreWin:
			return score <= _newScore;
		case EScoreSortType.lowScoreWin:
			return score >= _newScore;
		default:
			return false;
		}
	}

	public bool IsRankingIdMatch(EId _rankingId)
	{
		if (rankingId == (uint)_rankingId)
		{
			return true;
		}
		return false;
	}

	public bool IsTimeValid(DateTime _rankingCycleStartTime, DateTime? _rankingCycleEndTime)
	{
		bool flag = this.lastUpdate >= _rankingCycleStartTime;
		if (!_rankingCycleEndTime.HasValue)
		{
			return flag;
		}
		if (flag)
		{
			DateTime lastUpdate = this.lastUpdate;
			DateTime? t = _rankingCycleEndTime;
			return lastUpdate < t;
		}
		return false;
	}
}
