using System.Data;

namespace XMLDB3
{
	public class QuestObjectBuilder
	{
		public static Quest Build(DataRow _row)
		{
			Quest quest = new Quest();
			quest.id = (long)_row["id"];
			quest.templateid = (int)_row["templateid"];
			quest.complete = (byte)_row["complete"];
			quest.start_time = (long)_row["start_time"];
			quest.data = (string)_row["data"];
			quest.objectives = ItemXmlFieldHelper.BuildQuestObjective((string)_row["objective"]);
			return quest;
		}
	}
}
