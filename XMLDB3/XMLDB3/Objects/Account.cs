using Mabinogi.SQL;
using System;

public class Account
{
	public AccountSecondaryPasswordAuth SecondaryPasswordAuth;

	public string id;

	public string password;

	public string name;

	// Ignore the KSSN
	//public string serialnumber;

	public string email;

	public short flag;

	public DateTime blocking_date;

	public short blocking_duration;

	public byte authority;

	public bool changePassword;

    // Ignore the KSSN (encrypted)
    //public string eserialnumber;

    public string machineids = "";

    public static Account Build(SimpleReader reader, bool get2ndPass)
    {
        Account account = new Account();

        account.id = reader.GetString(Mabinogi.SQL.Columns.Account.Id);
        account.password = reader.GetString(Mabinogi.SQL.Columns.Account.Password);
        account.name = reader.GetString(Mabinogi.SQL.Columns.Account.Name);
        account.email = reader.GetString(Mabinogi.SQL.Columns.Account.Email);
        account.flag = reader.GetInt16(Mabinogi.SQL.Columns.Account.Flag);
        account.blocking_date = reader.GetDateTime(Mabinogi.SQL.Columns.Account.BlockingDate);
        account.blocking_duration = reader.GetInt16(Mabinogi.SQL.Columns.Account.BlockingDuration);
        account.authority = reader.GetByte(Mabinogi.SQL.Columns.Account.Authority);
        account.machineids = reader.GetString(Mabinogi.SQL.Columns.Account.MachineIDs);

        if (get2ndPass)
        {
            account.SecondaryPasswordAuth.missCount = reader.GetByte(Mabinogi.SQL.Columns.Account.SecondPassMissCount);
            account.SecondaryPasswordAuth.password2 = reader.GetString(Mabinogi.SQL.Columns.Account.SecondPassword);
        }

        return account;
    }
}
