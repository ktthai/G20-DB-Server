using Mabinogi;

namespace XMLDB3
{
	public class FarmSerializer
	{
		public static void Deserialize(Farm _farm, Message _message)
		{
			_message.WriteS64(_farm.farmID);
			_message.WriteString(_farm.ownerAccount);
			_message.WriteS64(_farm.ownerCharID);
			_message.WriteString(_farm.ownerCharName);
			_message.WriteS64(_farm.expireTime);
			_message.WriteU8(_farm.crop);
			_message.WriteS64(_farm.plantTime);
			_message.WriteS16(_farm.waterWork);
			_message.WriteS16(_farm.nutrientWork);
			_message.WriteS16(_farm.insectWork);
			_message.WriteS16(_farm.water);
			_message.WriteS16(_farm.nutrient);
			_message.WriteS16(_farm.insect);
			_message.WriteS16(_farm.growth);
			_message.WriteU8(_farm.currentWork);
			_message.WriteS64(_farm.workCompleteTime);
			_message.WriteU8(_farm.todayWorkCount);
			_message.WriteS64(_farm.lastWorkTime);
		}

		public static Farm Serialize(Message _message)
		{
			Farm farm = new Farm();
			farm.farmID = _message.ReadS64();
			farm.crop = _message.ReadU8();
			farm.plantTime = _message.ReadS64();
			farm.waterWork = _message.ReadS16();
			farm.nutrientWork = _message.ReadS16();
			farm.insectWork = _message.ReadS16();
			farm.water = _message.ReadS16();
			farm.nutrient = _message.ReadS16();
			farm.insect = _message.ReadS16();
			farm.growth = _message.ReadS16();
			farm.currentWork = _message.ReadU8();
			farm.workCompleteTime = _message.ReadS64();
			farm.todayWorkCount = _message.ReadU8();
			farm.lastWorkTime = _message.ReadS64();
			return farm;
		}
	}
}
