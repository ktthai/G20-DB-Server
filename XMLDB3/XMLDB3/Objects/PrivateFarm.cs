using System;
using System.Collections.Generic;

namespace XMLDB3
{
	public class PrivateFarm
	{
		public long id;

		public long ownerId;

		public int classId;

		public int level;

		public long exp;

		public string name;

		public short worldPosX;

		public short worldPosY;

		public byte deleteFlag;

		public string ownerName;

		public short bindedChannel;

		public long nextBindableTime;

		public uint penalty;

		public Dictionary<long, PrivateFarmFacility> field;

		public Dictionary<long, PrivateFarmVisitor> visitorList;

		public DateTime updatetime;

		public PrivateFarmFacility[] _field
		{
			get
			{
				if (field != null)
				{
					PrivateFarmFacility[] array = new PrivateFarmFacility[field.Values.Count];
					field.Values.CopyTo(array, 0);
					return array;
				}
				return null;
			}
			set
			{
				field = new Dictionary<long, PrivateFarmFacility>(value.Length);
				foreach (PrivateFarmFacility privateFarmFacility in value)
				{
					field.Add(privateFarmFacility.facilityId, privateFarmFacility);
				}
			}
		}

		public PrivateFarmVisitor[] _visitor
		{
			get
			{
				if (visitorList != null)
				{
					PrivateFarmVisitor[] array = new PrivateFarmVisitor[visitorList.Values.Count];
					visitorList.Values.CopyTo(array, 0);
					return array;
				}
				return null;
			}
			set
			{
				visitorList = new Dictionary<long, PrivateFarmVisitor>(value.Length);
				foreach (PrivateFarmVisitor privateFarmVisitor in value)
				{
					visitorList.Add(privateFarmVisitor.charId, privateFarmVisitor);
				}
			}
		}

		public override string ToString()
		{
			return base.ToString() + ":" + id;
		}
	}
}
