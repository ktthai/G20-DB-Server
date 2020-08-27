using System;

public class MembershipStausBilling
{
    private string accountIDField;

    private DateTime updateTimeField;

    public string accountID
    {
        get
        {
            return accountIDField;
        }
        set
        {
            accountIDField = value;
        }
    }

    public DateTime updateTime
    {
        get
        {
            return updateTimeField;
        }
        set
        {
            updateTimeField = value;
        }
    }
}
