using Mabinogi.SQL;
using System;

namespace XMLDB3
{
	public class PrivateFarmObjectBuilder
	{
		public static PrivateFarm Build(SimpleReader farmReader)
		{
			try
			{
				if (farmReader == null)
				{
					throw new Exception("개인농장 테이블이 없습니다.");
				}
				if (farmReader.Read() == false)
				{
					throw new Exception("개인농장 테이블 열이 하나 이상입니다.");
				}
				PrivateFarm privateFarm = new PrivateFarm();
				privateFarm.id = farmReader.GetInt64(Mabinogi.SQL.Columns.PrivateFarm.Id);
				privateFarm.ownerId = farmReader.GetInt64(Mabinogi.SQL.Columns.PrivateFarm.OwnerId);
				privateFarm.classId = farmReader.GetInt32(Mabinogi.SQL.Columns.PrivateFarm.ClassId);
				privateFarm.level = farmReader.GetInt32(Mabinogi.SQL.Columns.PrivateFarm.Level);
				privateFarm.exp = farmReader.GetInt64(Mabinogi.SQL.Columns.PrivateFarm.Exp);
				privateFarm.name = farmReader.GetString(Mabinogi.SQL.Columns.PrivateFarm.Name);
				privateFarm.worldPosX = farmReader.GetInt16(Mabinogi.SQL.Columns.PrivateFarm.WorldPosX);
				privateFarm.worldPosY = farmReader.GetInt16(Mabinogi.SQL.Columns.PrivateFarm.WorldPosY);
				privateFarm.deleteFlag = farmReader.GetByte(Mabinogi.SQL.Columns.PrivateFarm.DeleteFlag);
				privateFarm.ownerName = farmReader.GetString(Mabinogi.SQL.Columns.PrivateFarm.OwnerName);
				privateFarm.bindedChannel = farmReader.GetInt16(Mabinogi.SQL.Columns.PrivateFarm.BindedChannel);
				privateFarm.nextBindableTime = farmReader.GetInt64(Mabinogi.SQL.Columns.PrivateFarm.NextBindableTime);

				DateTime updateTime = farmReader.GetDateTime(Mabinogi.SQL.Columns.PrivateFarm.UpdateTime);
				if (updateTime != null)
				{
					privateFarm.updatetime = updateTime;
				}
				else
				{
					privateFarm.updatetime = DateTime.MinValue;
				}

				return privateFarm;
			}
			catch (Exception ex)
			{
				ExceptionMonitor.ExceptionRaised(ex);
				return null;
			}
		}
	}
}
