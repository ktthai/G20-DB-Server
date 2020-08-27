using Mabinogi;
using System;
using System.IO;
using System.Net;

namespace XMLDB3.ItemMarket
{
	public class ItemMarketResponse
	{
		protected int packetNo;

		protected int packetLength;

		protected IMMessage messageType;

		protected int result;

		protected Message message;

		public IMMessage Type => messageType;

		public int Result => result;

		public int PacketLength => packetLength;

		public int PacketNo => packetNo;

		public virtual bool IsSystemMessage => false;

		protected ItemMarketResponse()
		{
		}

		public static ItemMarketResponse BuildRespose(BinaryReader _br)
		{
			ItemMarketResponse itemMarketResponse = null;
			int num = 0;
			int num2 = 0;
			try
			{
				if (_br.BaseStream.Length <= _br.BaseStream.Position + 5)
				{
					return null;
				}
				if (_br.ReadByte() != 160)
				{
					throw new Exception("Invalid Protocol Header");
				}
				num = IPAddress.NetworkToHostOrder(_br.ReadInt32()) + 5;
				if (_br.BaseStream.Length < num)
				{
					return null;
				}
				num2 = IPAddress.NetworkToHostOrder(_br.ReadInt32());
				byte b = _br.ReadByte();
				switch (b)
				{
				case 1:
					itemMarketResponse = new IMInitializeResponse();
					break;
				case byte.MaxValue:
					itemMarketResponse = new IMHeartbeatResponse();
					break;
				case 17:
					itemMarketResponse = new IMCheckEnteranceResponse();
					break;
				case 18:
					itemMarketResponse = new IMCheckBalanceResponse();
					break;
				case 33:
					itemMarketResponse = new IMInquirySaleItemResponse();
					break;
				case 36:
					itemMarketResponse = new IMInquiryMyPageResponse();
					break;
				case 34:
					itemMarketResponse = new IMInquiryStorageResponse();
					break;
				case 49:
					itemMarketResponse = new IMItemListResponse();
					break;
				case 50:
					itemMarketResponse = new IMItemSearchResponse();
					break;
				case 65:
					itemMarketResponse = new IMSaleRequestResponse();
					break;
				case 66:
					itemMarketResponse = new IMSaleRequestCommitResponse();
					break;
				case 69:
					itemMarketResponse = new IMSaleRequestRollbackResponse();
					break;
				case 67:
					itemMarketResponse = new IMSaleCancelResponse();
					break;
				case 68:
					itemMarketResponse = new IMPurchaseResponse();
					break;
				case 129:
					itemMarketResponse = new IMGetItemResponse();
					break;
				case 130:
					itemMarketResponse = new IMGetItemCommitResponse();
					break;
				case 131:
					itemMarketResponse = new IMGetItemRollbackResponse();
					break;
				case 164:
					itemMarketResponse = new IMAdministratorAccountChangeResponse();
					break;
				}
				if (itemMarketResponse != null)
				{
					itemMarketResponse.messageType = (IMMessage)b;
				}
				else
				{
					itemMarketResponse = new ItemMarketResponse();
				}
				itemMarketResponse.packetLength = num;
				itemMarketResponse.packetNo = num2;
				return itemMarketResponse;
			}
			catch (EndOfStreamException)
			{
				return null;
			}
			catch (Exception ex2)
			{
				ExceptionMonitor.ExceptionRaised(ex2);
				if (itemMarketResponse == null)
				{
					itemMarketResponse = new ItemMarketResponse();
					itemMarketResponse.packetLength = num;
					itemMarketResponse.packetNo = num2;
				}
				return itemMarketResponse;
			}
		}

		public virtual void Build(BinaryReader _br, Message _message)
		{
			_message.WriteU8(0);
		}
	}
}
