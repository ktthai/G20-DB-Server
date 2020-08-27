using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class PrivateFarmFieldObjectBuilder
	{
		public static Dictionary<long, PrivateFarmFacility> BuildFacilityTable(SimpleReader reader)
		{
			try
			{
				if (reader == null)
				{
					return null;
				}
				Dictionary<long, PrivateFarmFacility> hashtable = new Dictionary<long, PrivateFarmFacility>();
				PrivateFarmFacility privateFarmFacility;

				while (reader.Read())
				{
					privateFarmFacility = new PrivateFarmFacility();
					privateFarmFacility.privateFarmId = reader.GetInt64(Mabinogi.SQL.Columns.PrivateFarmFacility.PrivateFarmId);
					privateFarmFacility.facilityId = reader.GetInt64(Mabinogi.SQL.Columns.PrivateFarmFacility.FacilityId);
					privateFarmFacility.classId = reader.GetInt32(Mabinogi.SQL.Columns.PrivateFarmFacility.ClassId);
					privateFarmFacility.x = reader.GetInt32(Mabinogi.SQL.Columns.PrivateFarmFacility.X);
					privateFarmFacility.y = reader.GetInt32(Mabinogi.SQL.Columns.PrivateFarmFacility.Y);
					privateFarmFacility.dir = reader.GetByte(Mabinogi.SQL.Columns.PrivateFarmFacility.Dir);
					privateFarmFacility.color = new int[9];
					for (int i = 0; i < 9; i++)
					{
						privateFarmFacility.color[i] = reader.GetInt32(Mabinogi.SQL.Columns.PrivateFarmFacility.Color[i]);
					}
					privateFarmFacility.finishTime = reader.GetInt64(Mabinogi.SQL.Columns.PrivateFarmFacility.FinishTime);
					privateFarmFacility.lastProcessingTime = reader.GetInt64(Mabinogi.SQL.Columns.PrivateFarmFacility.LastProcessingTime);
					privateFarmFacility.linkedFacilityId = reader.GetInt64(Mabinogi.SQL.Columns.PrivateFarmFacility.LinkedFacilityId);
					privateFarmFacility.permissionFlag = reader.GetInt32(Mabinogi.SQL.Columns.PrivateFarmFacility.PermissionFlag);
					privateFarmFacility.meta = reader.GetString(Mabinogi.SQL.Columns.PrivateFarmFacility.Meta);
					privateFarmFacility.customName = reader.GetString(Mabinogi.SQL.Columns.PrivateFarmFacility.CustomName);
					hashtable.Add(privateFarmFacility.facilityId, privateFarmFacility);
				}
				return hashtable;
			}
			catch (Exception ex)
			{
				ExceptionMonitor.ExceptionRaised(ex);
				return null;
			}
		}

		public static Dictionary<long, PrivateFarmVisitor> BuildVisitorTable(SimpleReader reader)
		{
			try
			{
				if (reader == null)
				{
					return null;
				}
				Dictionary<long, PrivateFarmVisitor> hashtable = new Dictionary<long, PrivateFarmVisitor>();
				PrivateFarmVisitor privateFarmVisitor;

				while (reader.Read())
				{
					privateFarmVisitor = new PrivateFarmVisitor();
					privateFarmVisitor.charName = reader.GetString(Mabinogi.SQL.Columns.PrivateFarmVisitor.CharName);
					privateFarmVisitor.accountName = reader.GetString(Mabinogi.SQL.Columns.PrivateFarmVisitor.Account);
					privateFarmVisitor.charId = reader.GetInt64(Mabinogi.SQL.Columns.PrivateFarmVisitor.CharId);
					privateFarmVisitor.status = reader.GetByte(Mabinogi.SQL.Columns.PrivateFarmVisitor.Status);
					hashtable.Add(privateFarmVisitor.charId, privateFarmVisitor);
				}
				return hashtable;
			}
			catch (Exception ex)
			{
				ExceptionMonitor.ExceptionRaised(ex);
				return null;
			}
		}
	}
}
