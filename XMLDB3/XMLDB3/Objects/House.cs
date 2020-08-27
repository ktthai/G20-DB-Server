using System;
using System.Collections.Generic;

public class House
{

	public HouseBlock[] block;

	public HouseBid bid;


	public List<HouseBidder> bidders;


	public long houseID;


	public byte constructed;


	public DateTime updateTime;


	public string account;


	public string charName;


	public string houseName;


	public int houseClass;


	public byte roofSkin;


	public byte roofColor1;


	public byte roofColor2;


	public byte roofColor3;

	public byte wallSkin;


	public byte wallColor1;


	public byte wallColor2;


	public byte wallColor3;


	public byte innerSkin;


	public byte innerColor1;


	public byte innerColor2;


	public byte innerColor3;


	public int width;


	public int height;


	public DateTime bidSuccessDate;


	public DateTime taxPrevDate;


	public DateTime taxNextDate;


	public int taxPrice;


	public byte taxAutopay;


	public int houseMoney;


	public int deposit;


	public long flag;
}
