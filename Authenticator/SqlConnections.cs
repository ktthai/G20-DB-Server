public class SqlConnections
{
    public Connection fantasylifeclub { get; set; }

    public Connection premiumpack { get; set; }

    public Connection pceventcoupon { get; set; }

    public Connection CharacterCard { get; set; }
    public Connection PetCard { get; set; }

    public Connection gift { get; set; }

    public Connection freeservice { get; set; }

    public SqlConnections()
    {
        fantasylifeclub = new Connection();
        premiumpack = new Connection();
        pceventcoupon = new Connection();
        CharacterCard = new Connection();
        gift = new Connection();
        freeservice = new Connection();
        PetCard = new Connection();
    }
}
