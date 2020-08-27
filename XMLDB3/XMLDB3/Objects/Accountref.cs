using Mabinogi.SQL;
using System;
using System.Collections.Generic;

public class AccountRef
{
	public List<AccountrefCharacter> Character;

	public List<AccountrefPet> Pet;

	public List<PawnCoinLog> PawnCoinLog;

	public List<AccountrefMeta> AccountMeta;

	public string account;

	public int flag;

	public short maxslot;

	public DateTime In;

	public DateTime Out;

	public int playabletime;

	public byte supportRace;

	public byte supportRewardState;

	public int lobbyOption;

	public int supportLastChangeTime;

	public byte macroCheckFailure;

	public byte macroCheckSuccess;

	public bool beginnerFlag;

	public int pawnCoin;

	public string ip;

	public string mid;

	public AccountRef()
	{
		Character = new List<AccountrefCharacter>();
		Pet = new List<AccountrefPet>();
		PawnCoinLog = new List<PawnCoinLog>();
		AccountMeta = new List<AccountrefMeta>();
	}

    public static AccountRef Build(SimpleReader reader)
    {
        AccountRef result = new AccountRef();

        result.account = reader.GetString(Mabinogi.SQL.Columns.AccountRef.Id);
		result.flag = reader.GetInt32(Mabinogi.SQL.Columns.AccountRef.Flag);
        result.maxslot = reader.GetInt16(Mabinogi.SQL.Columns.AccountRef.MaxSlot);
        result.In = reader.GetDateTime(Mabinogi.SQL.Columns.AccountRef.In);
        result.Out = reader.GetDateTime(Mabinogi.SQL.Columns.AccountRef.Out);
        result.playabletime = reader.GetInt32(Mabinogi.SQL.Columns.AccountRef.PlayableTime);
        result.supportRace = reader.GetByte(Mabinogi.SQL.Columns.AccountRef.SupportRace);
        result.supportRewardState = reader.GetByte(Mabinogi.SQL.Columns.AccountRef.SupportRewardState);
        result.lobbyOption = reader.GetInt32(Mabinogi.SQL.Columns.AccountRef.LobbyOption);
        result.supportLastChangeTime = reader.GetInt32(Mabinogi.SQL.Columns.AccountRef.SupportLastChangeTime);
        result.macroCheckFailure = reader.GetByte(Mabinogi.SQL.Columns.AccountRef.MacroCheckFailure);
        result.beginnerFlag = reader.GetBoolean(Mabinogi.SQL.Columns.AccountRef.BeginnerFlag);
        result.macroCheckSuccess = reader.GetByte(Mabinogi.SQL.Columns.AccountRef.MacroCheckSuccess);

		return result;
    }
}
