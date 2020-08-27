using System;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class PetUpdateBuilder
	{
		public static void Build(Pet _new, Pet _old,  SimpleConnection conn, SimpleTransaction transaction)
		{
			if (_new == null)
			{
				throw new ArgumentException("펫 데이터가 없습니다.", "_new");
			}
			if (_old == null)
			{
				throw new ArgumentException("펫 캐쉬 데이터가 없습니다.", "_old");
			}

			var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Pet, transaction);

			cmd.Where(Mabinogi.SQL.Columns.Pet.Id, _new.id);

			PetAppearanceUpdateBuilder.Build(_new, _old, ref cmd);
			PetParameterUpdateBuilder.Build(_new, _old, ref cmd);
			PetParameterExUpdateBuilder.Build(_new, _old, ref cmd);
			PetDataUpdateBuilder.Build(_new, _old, ref cmd);
			PetMemoryUpdateBuilder.Build(_new, _old, ref cmd);
			PetConditionUpdateBuilder.Build(_new, _old, ref cmd);
			PetPrivateUpdateBuilder.Build(_new, _old, ref cmd);
			PetSummonUpdateBuilder.Build(_new, _old, ref cmd);
			PetMacroCheckerUpdateBuilder.Build(_new, _old, ref cmd);

			cmd.Set(Mabinogi.SQL.Columns.Pet.UpdateTime, DateTime.Now);
			cmd.Set(Mabinogi.SQL.Columns.Pet.CouponCode, _new.inventoryHash);

			cmd.Execute();

			PetSkillUpdateBuilder.Build(_new, _old, conn, transaction);

		}
	}
}
