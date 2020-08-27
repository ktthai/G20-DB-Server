

public class Itembase
{
    public Quest quest { get; set; }

    public ItemOption[] options { get; set; }

    public Ego ego { get; set; }

    public long id { get; set; }

    public byte pocket { get; set; }

    public int Class { get; set; }

    public int pos_x { get; set; }

    public int pos_y { get; set; }

    public int color_01 { get; set; }

    public int color_02 { get; set; }

    public int color_03 { get; set; }

    public int price { get; set; }

    public int sellingprice { get; set; }

    public short bundle { get; set; }

    public byte linked_pocket { get; set; }

    public int figure { get; set; }

    public byte flag { get; set; }

    public int durability { get; set; }

    public int durability_max { get; set; }

    public int origin_durability_max { get; set; }

    public short attack_min { get; set; }

    public short attack_max { get; set; }

    public short wattack_min { get; set; }

    public short wattack_max { get; set; }

    public byte balance { get; set; }

    public byte critical { get; set; }

    public int defence { get; set; }

    public short protect { get; set; }

    public short effective_range { get; set; }

    public byte attack_speed { get; set; }

    public byte down_hit_count { get; set; }

    public short experience { get; set; }

    public byte exp_point { get; set; }

    public byte upgraded { get; set; }

    public byte upgrade_max { get; set; }

    public byte grade { get; set; }

    public short prefix { get; set; }

    public short suffix { get; set; }

    public string data { get; set; }

    public int expiration { get; set; }

    public int varint { get; set; }

    public byte storedtype { get; set; }
}
