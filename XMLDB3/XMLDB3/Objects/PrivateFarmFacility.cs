
namespace XMLDB3
{
	public class PrivateFarmFacility
	{
		public long privateFarmId;

		public long facilityId;

		public int classId;

		public int x;

		public int y;

		public byte dir;

		public int[] color;

		public long finishTime;

		public long lastProcessingTime;

		public long linkedFacilityId;

		public int permissionFlag;

		public string meta;

		public string customName;

		public int color1
		{
			get
			{
				return color[0];
			}
			set
			{
				if (color == null)
				{
					color = new int[9];
				}
				color[0] = value;
			}
		}

		public int color2
		{
			get
			{
				return color[1];
			}
			set
			{
				if (color == null)
				{
					color = new int[1];
				}
				color[1] = value;
			}
		}

		public int color3
		{
			get
			{
				return color[2];
			}
			set
			{
				if (color == null)
				{
					color = new int[9];
				}
				color[2] = value;
			}
		}

		public int color4
		{
			get
			{
				return color[3];
			}
			set
			{
				if (color == null)
				{
					color = new int[9];
				}
				color[3] = value;
			}
		}

		public int color5
		{
			get
			{
				return color[4];
			}
			set
			{
				if (color == null)
				{
					color = new int[9];
				}
				color[4] = value;
			}
		}

		public int color6
		{
			get
			{
				return color[5];
			}
			set
			{
				if (color == null)
				{
					color = new int[9];
				}
				color[5] = value;
			}
		}

		public int color7
		{
			get
			{
				return color[6];
			}
			set
			{
				if (color == null)
				{
					color = new int[9];
				}
				color[6] = value;
			}
		}

		public int color8
		{
			get
			{
				return color[7];
			}
			set
			{
				if (color == null)
				{
					color = new int[9];
				}
				color[7] = value;
			}
		}

		public int color9
		{
			get
			{
				return color[8];
			}
			set
			{
				if (color == null)
				{
					color = new int[9];
				}
				color[8] = value;
			}
		}
	}
}
