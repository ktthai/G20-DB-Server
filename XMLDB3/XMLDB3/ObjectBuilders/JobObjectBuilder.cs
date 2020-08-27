using Mabinogi.SQL;

namespace XMLDB3
{
	public class JobObjectBuilder
	{
		public static CharacterJob Build(SimpleReader reader)
		{
			CharacterJob characterJob = new CharacterJob();
			characterJob.jobId = reader.GetByte(Mabinogi.SQL.Columns.Character.JobId);
			return characterJob;
		}
	}
}
