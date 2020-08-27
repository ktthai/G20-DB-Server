using Mabinogi.SQL;

namespace XMLDB3
{
	public class JobUpdateBuilder
	{
		public static bool Build(Character _new, Character _old, SimpleCommand cmd)
		{
			bool result = false;
			if (_new.job != null && _old.job != null)
			{
				if (_new.job.jobId != _old.job.jobId)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.JobId,  _new.job.jobId);
                    result = true;
                }
			}
			return result;
		}
	}
}
