public class Event
{
	public EventFantasylifeclub fantasylifeclub { get; set; }

	public EventPremiumPack premiumpack { get; set; }


	public Event()
    {
		fantasylifeclub = new EventFantasylifeclub();
		premiumpack = new EventPremiumPack();
    }
}
