using System;


public class InviteEvent
{
	private string mabiIdField;

	private string servernameField;

	private string invitecharacternameField;

	private DateTime senddateField;

	public string mabiId
	{
		get
		{
			return mabiIdField;
		}
		set
		{
			mabiIdField = value;
		}
	}

	public string servername
	{
		get
		{
			return servernameField;
		}
		set
		{
			servernameField = value;
		}
	}

	public string invitecharactername
	{
		get
		{
			return invitecharacternameField;
		}
		set
		{
			invitecharacternameField = value;
		}
	}

	public DateTime senddate
	{
		get
		{
			return senddateField;
		}
		set
		{
			senddateField = value;
		}
	}
}
