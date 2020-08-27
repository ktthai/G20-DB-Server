using System.Text.Json;

namespace XMLDB3
{
	public class ItemXmlFieldHelper
	{
		//public class ItemOptionContainer
		//{
		//	public ItemOption[] options;
		//}

		//public class QuestObjectiveContainer
		//{
		//	public QuestObjective[] objectives;
		//}

		private static string LATEST_ITEMOPTION_VER = "2";

		public static string BuildItemOptionXml(ItemOption[] _options)
		{
			return JsonSerializer.Serialize(_options);
		}

		public static ItemOption[] BuildItemOption(string _options)
		{
			return JsonSerializer.Deserialize<ItemOption[]>(_options);
		}


		public static string BuildQuestObjectiveXml(QuestObjective[] _objectives)
		{
			return JsonSerializer.Serialize(_objectives);
		}

		public static QuestObjective[] BuildQuestObjective(string _objectives)
		{
			return JsonSerializer.Deserialize<QuestObjective[]>(_objectives);
		}
	}
}
