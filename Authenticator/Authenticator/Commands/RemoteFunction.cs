using Mabinogi;

namespace Authenticator
{
    public abstract class RemoteFunction
    {
        public enum Command
        {
            SHOP_COMMAND_QUERY_FANTASYLIFECLUB_INFO_REQUEST = 100,
            SHOP_COMMAND_QUERY_CHARACTERCARD_INFO_REQUEST = 101,
            SHOP_COMMAND_QUERY_CHARACTERCARD_USE_BEGIN_REQUEST = 102,
            SHOP_COMMAND_QUERY_CHARACTERCARD_USE_COMMIT_REQUEST = 103,
            SHOP_COMMAND_QUERY_CHARACTERCARD_USE_ROLLBACK_REQUEST = 104,
            SHOP_COMMAND_QUERY_CREATE_DEFAULTCARD_REQUEST = 105,
            SHOP_COMMAND_QUERY_FANTASYLIFECLUB_INFO_REPLY = 106,
            SHOP_COMMAND_QUERY_CHARACTERCARD_INFO_REPLY = 107,
            SHOP_COMMAND_QUERY_CHARACTERCARD_USE_BEGIN_REPLY = 108,
            SHOP_COMMAND_QUERY_CHARACTERCARD_USE_COMMIT_REPLY = 109,
            SHOP_COMMAND_QUERY_CHARACTERCARD_USE_ROLLBACK_REPLY = 110,
            SHOP_COMMAND_QUERY_CREATE_DEFAULTCARD_REPLY = 111,
            COUPON_COMMAND_QUERY_USER_BEGIN_REQUEST = 112,
            COUPON_COMMAND_QUERY_USER_COMMIT_REQUEST = 113,
            COUPON_COMMAND_QUERY_USER_ROLLBACK_REQUEST = 114,
            COUPON_COMMAND_QUERY_USER_BEGIN_REPLY = 115,
            COUPON_COMMAND_QUERY_USER_COMMIT_REPLY = 116,
            COUPON_COMMAND_QUERY_USER_ROLLBACK_REPLY = 117,
            SHOP_COMMAND_QUERY_DELETE_CHARACTERCARD_REQUEST = 118,
            SHOP_COMMAND_QUERY_DELETE_CHARACTERCARD_REPLY = 119,
            SHOP_COMMAND_QUERY_PETCARD_INFO_REQUEST = 150,
            SHOP_COMMAND_QUERY_PETCARD_USE_BEGIN_REQUEST = 151,
            SHOP_COMMAND_QUERY_PETCARD_USE_COMMIT_REQUEST = 152,
            SHOP_COMMAND_QUERY_PETCARD_USE_ROLLBACK_REQUEST = 153,
            SHOP_COMMAND_QUERY_PETCARD_INFO_REPLY = 154,
            SHOP_COMMAND_QUERY_PETCARD_USE_BEGIN_REPLY = 155,
            SHOP_COMMAND_QUERY_PETCARD_USE_COMMIT_REPLY = 156,
            SHOP_COMMAND_QUERY_PETCARD_USE_ROLLBACK_REPLY = 157,
            SHOP_COMMAND_QUERY_GIFT_INFO_REQUEST = 170,
            SHOP_COMMAND_QUERY_GIFT_ACCEPT_REQUEST = 171,
            SHOP_COMMAND_QUERY_GIFT_REJECT_REQUEST = 172,
            SHOP_COMMAND_QUERY_GIFT_INFO_REPLY = 173,
            SHOP_COMMAND_QUERY_GIFT_ACCEPT_REPLY = 174,
            SHOP_COMMAND_QUERY_GIFT_REJECT_REPLY = 175,
            SHOP_COMMAND_QUERY_ITEMSHOP_INFO_REQUEST = 180,
            SHOP_COMMAND_QUERY_ITEMSHOP_USE_REQUEST = 181,
            SHOP_COMMAND_QUERY_ITEMSHOP_ROLLBACK_REQUEST = 182,
            SHOP_COMMAND_QUERY_ITEMSHOP_INFO_REPLY = 183,
            SHOP_COMMAND_QUERY_ITEMSHOP_USE_REPLY = 184,
            SHOP_COMMAND_QUERY_ITEMSHOP_ROLLBACK_REPLY = 185,
            SHOP_COMMAND_QUERY_PREMIUMPACK_INFO_REQUEST = 186,
            SHOP_COMMAND_QUERY_PREMIUMPACK_INFO_REPLY = 187,
            SHOP_COMMAND_QUERY_FREESERVICE_INFO_REQUEST = 188,
            SHOP_COMMAND_QUERY_FREESERVICE_INFO_REPLY = 189,
            SHOP_COMMAND_QUERY_FREESERVICE_UPDATE_REQUEST = 190,
            SHOP_COMMAND_QUERY_FREESERVICE_UPDATE_REPLY = 191,
            SHOP_COMMAND_QUERY_CREATE_CHARACTER_CARD_REQUEST = 192,
            SHOP_COMMAND_QUERY_CREATE_CHARACTER_CARD_REPLY = 193,
            SHOP_COMMAND_QUERY_CREATE_PET_CARD_REQUEST = 194,
            SHOP_COMMAND_QUERY_CREATE_PET_CARD_REPLY = 195,
            MONITOR_COMMAND_QUERY_SESSION_INFO_REQUEST = 200,
            MONITOR_COMMAND_QUERY_NETWORK_INFO_REQUEST = 201,
            MONITOR_COMMAND_QUERY_SYSTEM_INFO_REQUEST = 202,
            MONITOR_COMMAND_QUERY_ABORT_SESSION_REQUEST = 203,
            MONITOR_COMMAND_QUERY_DISCONNECT_NETWORK_REQUEST = 204,
            MONITOR_COMMAND_QUERY_EXCEPTION_INFO_REQUEST = 205,
            MONITOR_COMMAND_QUERY_SESSION_INFO_REPLY = 206,
            MONITOR_COMMAND_QUERY_NETWORK_INFO_REPLY = 207,
            MONITOR_COMMAND_QUERY_SYSTEM_INFO_REPLY = 208,
            MONITOR_COMMAND_QUERY_ABORT_SESSION_REPLY = 209,
            MONITOR_COMMAND_QUERY_DISCONNECT_NETWORK_REPLY = 210,
            MONITOR_COMMAND_QUERY_EXCEPTION_INFO_REPLY = 211,
            WEB_COMMAND_QUERY_MAIN_CHARACTER_NAME_REQUEST = 252,
            WEB_COMMAND_QUERY_MAIN_CHARACTER_NAME_REPLY = 253,
            MONITOR_COMMAND_QUERY_TEST_01_REQUEST = 300,
            MONITOR_COMMAND_QUERY_TEST_01_REPLY = 301,
            ACCOUNT_COMMAND_QUERY_PASSWORDCHANGE_2010_REQUEST = 350,
            ACCOUNT_COMMAND_QUERY_PASSWORDCHANGE_2010_REPLY = 351,
            //WEB_COMMAND_QUERY_NEXON_ID = 355,
            //WEB_COMMAND_QUERY_NEXON_OID = 356,
            //WEB_COMMAND_QUERY_NEXON_OTP_USER = 357,
            //WEB_COMMAND_CHECK_NEXON_OTP = 358,
            //WEB_COMMAND_CHECK_NEXON_OTP_BY_OID = 359,
            //WEB_COMMAND_QUERY_MABI_ACCOUNT_LIST_FROM_NEXON_OID = 360,
            //WEB_COMMAND_CREATE_ACCOUNT_NXK = 361,
            //WEB_COMMAND_QUERY_AGE = 362,
            //WEB_COMMAND_QUERY_NEXON_EMAIL_MEMBERSHIP_USER = 363,
            MONITOR_COMMAND_QUERY_SESSION_INFO_INVALID = -1
        }

        private int m_NetworkId;

        private Message m_InputMsg;

        private Command m_ReplyCommand = Command.MONITOR_COMMAND_QUERY_SESSION_INFO_INVALID;

        private int m_QueryKey;

        private string m_Name;

        public string Name => m_Name;

        public string IpPort;
        public bool IsExternalCommand;

        protected Message Input => m_InputMsg;

        public abstract Message Process();

        protected abstract string BuildName(Message _input);

        protected Message GetNewReply()
        {
            Message message = new Message((uint)m_ReplyCommand, 0uL);
            message.WriteS32(m_QueryKey);
            return message;
        }

        protected void Reply(Message _input)
        {
            if (IsExternalCommand)
                DataBridge.ServerSend(IpPort, _input);
            else
                MainProcedure.ServerSend(m_NetworkId, _input);
        }

        public static RemoteFunction Parse(int _networkid, Message _inputmsg)
        {
            RemoteFunction remoteFunction = Parse(_inputmsg);
            if (remoteFunction != null)
            {
                remoteFunction.m_NetworkId = _networkid;
                remoteFunction.m_InputMsg = _inputmsg;
                remoteFunction.m_ReplyCommand = MatchReplyCommand((Command)_inputmsg.ID);
                remoteFunction.m_QueryKey = _inputmsg.ReadS32();
                remoteFunction.m_Name = remoteFunction.BuildName(_inputmsg.Clone());
            }
            return remoteFunction;
        }

        public static RemoteFunction Parse(string ipPort, Message _inputmsg)
        {
            RemoteFunction remoteFunction = Parse(_inputmsg);
            if (remoteFunction != null)
            {
                remoteFunction.IpPort = ipPort;
                remoteFunction.m_NetworkId = (int)_inputmsg.Target;
                remoteFunction.m_InputMsg = _inputmsg;
                remoteFunction.m_ReplyCommand = MatchReplyCommand((Command)_inputmsg.ID);
                remoteFunction.m_QueryKey = _inputmsg.ReadS32();
                remoteFunction.m_Name = remoteFunction.BuildName(_inputmsg.Clone());
            }
            return remoteFunction;
        }

        private static RemoteFunction Parse(Message _inputmsg)
        {
            RemoteFunction remoteFunction = null;
            switch ((Command)_inputmsg.ID)
            {
                case Command.MONITOR_COMMAND_QUERY_SESSION_INFO_REQUEST:
                    remoteFunction = new RemoteFunction_SessionInfo();
                    break;
                case Command.MONITOR_COMMAND_QUERY_NETWORK_INFO_REQUEST:
                    remoteFunction = new RemoteFunction_NetworkInfo();
                    break;
                case Command.MONITOR_COMMAND_QUERY_SYSTEM_INFO_REQUEST:
                    remoteFunction = new RemoteFunction_SystemInfo();
                    break;
                case Command.MONITOR_COMMAND_QUERY_ABORT_SESSION_REQUEST:
                    remoteFunction = new RemoteFunction_SessionForceClose();
                    break;
                case Command.MONITOR_COMMAND_QUERY_DISCONNECT_NETWORK_REQUEST:
                    remoteFunction = new RemoteFunction_NetworkForceClose();
                    break;
                case Command.MONITOR_COMMAND_QUERY_EXCEPTION_INFO_REQUEST:
                    remoteFunction = new RemoteFunction_ExceptionInfo();
                    break;
                case Command.SHOP_COMMAND_QUERY_FANTASYLIFECLUB_INFO_REQUEST:
                    remoteFunction = new FantasyLifeClubInfo();
                    break;
                case Command.SHOP_COMMAND_QUERY_PREMIUMPACK_INFO_REQUEST:
                    remoteFunction = new PremiumPackInfo();
                    break;
                case Command.SHOP_COMMAND_QUERY_CHARACTERCARD_INFO_REQUEST:
                    remoteFunction = new RemoteFunction_CharacterCardInfo();
                    break;
                case Command.SHOP_COMMAND_QUERY_CHARACTERCARD_USE_BEGIN_REQUEST:
                    remoteFunction = new RemoteFunction_CharacterCardTranBegin();
                    break;
                case Command.SHOP_COMMAND_QUERY_CHARACTERCARD_USE_COMMIT_REQUEST:
                    remoteFunction = new RemoteFunction_CharacterCardTranCommit();
                    break;
                case Command.SHOP_COMMAND_QUERY_CHARACTERCARD_USE_ROLLBACK_REQUEST:
                    remoteFunction = new RemoteFunction_CharacterCardTranRollback();
                    break;
                case Command.SHOP_COMMAND_QUERY_CREATE_DEFAULTCARD_REQUEST:
                    remoteFunction = new RemoteFunction_CreateDefaultCard();
                    break;
                case Command.SHOP_COMMAND_QUERY_DELETE_CHARACTERCARD_REQUEST:
                    remoteFunction = new RemoteFunction_CharacterCardDelete();
                    break;
                case Command.COUPON_COMMAND_QUERY_USER_BEGIN_REQUEST:
                    remoteFunction = new RemoteFunction_CouponTranBegin();
                    break;
                case Command.COUPON_COMMAND_QUERY_USER_COMMIT_REQUEST:
                    remoteFunction = new RemoteFunction_CouponTranCommit();
                    break;
                case Command.COUPON_COMMAND_QUERY_USER_ROLLBACK_REQUEST:
                    remoteFunction = new RemoteFunction_CouponTranRollback();
                    break;
                case Command.SHOP_COMMAND_QUERY_PETCARD_INFO_REQUEST:
                    remoteFunction = new RemoteFunction_PetCardInfo();
                    break;
                case Command.SHOP_COMMAND_QUERY_PETCARD_USE_BEGIN_REQUEST:
                    remoteFunction = new RemoteFunction_PetCardTranBegin();
                    break;
                case Command.SHOP_COMMAND_QUERY_PETCARD_USE_COMMIT_REQUEST:
                    remoteFunction = new RemoteFunction_PetCardTranCommit();
                    break;
                case Command.SHOP_COMMAND_QUERY_PETCARD_USE_ROLLBACK_REQUEST:
                    remoteFunction = new RemoteFunction_PetCardTranRollback();
                    break;
                case Command.SHOP_COMMAND_QUERY_GIFT_INFO_REQUEST:
                    remoteFunction = new RemoteFunction_GiftInfo();
                    break;
                case Command.SHOP_COMMAND_QUERY_GIFT_ACCEPT_REQUEST:
                    remoteFunction = new RemoteFunction_GiftAccept();
                    break;
                case Command.SHOP_COMMAND_QUERY_GIFT_REJECT_REQUEST:
                    remoteFunction = new RemoteFunction_GiftReject();
                    break;
                case Command.SHOP_COMMAND_QUERY_ITEMSHOP_INFO_REQUEST:
                    remoteFunction = new RemoteFunction_ItemShopInfo();
                    break;
                case Command.SHOP_COMMAND_QUERY_ITEMSHOP_USE_REQUEST:
                    remoteFunction = new RemoteFunction_ItemShopUse();
                    break;
                case Command.SHOP_COMMAND_QUERY_ITEMSHOP_ROLLBACK_REQUEST:
                    remoteFunction = new RemoteFunction_ItemShopRollback();
                    break;
                case Command.SHOP_COMMAND_QUERY_FREESERVICE_INFO_REQUEST:
                    remoteFunction = new RemoveFunction_FreeServiceInfo();
                    break;
                case Command.SHOP_COMMAND_QUERY_FREESERVICE_UPDATE_REQUEST:
                    remoteFunction = new RemoveFunction_FreeServiceInfoUpdate();
                    break;
                case Command.SHOP_COMMAND_QUERY_CREATE_CHARACTER_CARD_REQUEST:
                    remoteFunction = new RemoteFunction_CreateCharacterCard();
                    break;
                case Command.SHOP_COMMAND_QUERY_CREATE_PET_CARD_REQUEST:
                    remoteFunction = new RemoteFunction_CreatePetCard();
                    break;
                case Command.WEB_COMMAND_QUERY_MAIN_CHARACTER_NAME_REQUEST:
                    remoteFunction = new RemoteFunction_QueryMainCharacterName();
                    break;
                /*case 355u:
                    remoteFunction = new RemoteFunction_QueryNexonId();
                    break; */
                /*case 356u:
                    remoteFunction = new RemoteFunction_QueryNexonOID();
                    break; */
                /*case 357u:
                    remoteFunction = new RemoteFunction_QueryNexonOTPUser();
                    break; */
                /*case 358u:
                    remoteFunction = new RemoteFunction_CheckNexonOTP();
                    break; */
                /*case 359u:
                    remoteFunction = new RemoteFunction_CheckNexonOTPbyOID();
                    break;*/
                /*case 360u:
                    remoteFunction = new RemoteFunction_QueryMabiAccountListFromNexonOID();
                    break;*/
                /*case 361u:
                        remoteFunction = new RemoteFunction_CreateAccountNxK();
                    break;*/
                /*case 362u:
                    remoteFunction = new RemoteFunction_QueryAge();
                    break;*/
                /*case 363u:
                        remoteFunction = new RemoteFunction_QueryNexonEmailMembershipUser();
                    break;*/
                case Command.MONITOR_COMMAND_QUERY_TEST_01_REQUEST:
                    remoteFunction = new RemoteFunction_Test01();
                    break;
                case Command.ACCOUNT_COMMAND_QUERY_PASSWORDCHANGE_2010_REQUEST:
                    remoteFunction = new PasswordChange2010Function();
                    break;
                default:
                    remoteFunction = null;
                    break;
            }
            return remoteFunction;
        }

        private static Command MatchReplyCommand(Command _requestcmd)
        {
            switch (_requestcmd)
            {
                case Command.MONITOR_COMMAND_QUERY_SESSION_INFO_REQUEST:
                    return Command.MONITOR_COMMAND_QUERY_SESSION_INFO_REPLY;
                case Command.MONITOR_COMMAND_QUERY_NETWORK_INFO_REQUEST:
                    return Command.MONITOR_COMMAND_QUERY_NETWORK_INFO_REPLY;
                case Command.MONITOR_COMMAND_QUERY_SYSTEM_INFO_REQUEST:
                    return Command.MONITOR_COMMAND_QUERY_SYSTEM_INFO_REPLY;
                case Command.MONITOR_COMMAND_QUERY_ABORT_SESSION_REQUEST:
                    return Command.MONITOR_COMMAND_QUERY_ABORT_SESSION_REPLY;
                case Command.MONITOR_COMMAND_QUERY_DISCONNECT_NETWORK_REQUEST:
                    return Command.MONITOR_COMMAND_QUERY_DISCONNECT_NETWORK_REPLY;
                case Command.MONITOR_COMMAND_QUERY_EXCEPTION_INFO_REQUEST:
                    return Command.MONITOR_COMMAND_QUERY_EXCEPTION_INFO_REPLY;
                case Command.MONITOR_COMMAND_QUERY_TEST_01_REQUEST:
                    return Command.MONITOR_COMMAND_QUERY_TEST_01_REPLY;
                case Command.SHOP_COMMAND_QUERY_FANTASYLIFECLUB_INFO_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_FANTASYLIFECLUB_INFO_REPLY;
                case Command.SHOP_COMMAND_QUERY_CHARACTERCARD_INFO_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_CHARACTERCARD_INFO_REPLY;
                case Command.SHOP_COMMAND_QUERY_CHARACTERCARD_USE_BEGIN_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_CHARACTERCARD_USE_BEGIN_REPLY;
                case Command.SHOP_COMMAND_QUERY_CHARACTERCARD_USE_COMMIT_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_CHARACTERCARD_USE_COMMIT_REPLY;
                case Command.SHOP_COMMAND_QUERY_CHARACTERCARD_USE_ROLLBACK_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_CHARACTERCARD_USE_ROLLBACK_REPLY;
                case Command.SHOP_COMMAND_QUERY_CREATE_DEFAULTCARD_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_CREATE_DEFAULTCARD_REPLY;
                case Command.SHOP_COMMAND_QUERY_DELETE_CHARACTERCARD_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_DELETE_CHARACTERCARD_REPLY;
                case Command.COUPON_COMMAND_QUERY_USER_BEGIN_REQUEST:
                    return Command.COUPON_COMMAND_QUERY_USER_BEGIN_REPLY;
                case Command.COUPON_COMMAND_QUERY_USER_COMMIT_REQUEST:
                    return Command.COUPON_COMMAND_QUERY_USER_COMMIT_REPLY;
                case Command.COUPON_COMMAND_QUERY_USER_ROLLBACK_REQUEST:
                    return Command.COUPON_COMMAND_QUERY_USER_ROLLBACK_REQUEST;
                case Command.SHOP_COMMAND_QUERY_CREATE_CHARACTER_CARD_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_CREATE_CHARACTER_CARD_REPLY;
                case Command.SHOP_COMMAND_QUERY_PETCARD_INFO_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_PETCARD_INFO_REPLY;
                case Command.SHOP_COMMAND_QUERY_PETCARD_USE_BEGIN_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_PETCARD_USE_BEGIN_REPLY;
                case Command.SHOP_COMMAND_QUERY_PETCARD_USE_COMMIT_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_PETCARD_USE_COMMIT_REPLY;
                case Command.SHOP_COMMAND_QUERY_PETCARD_USE_ROLLBACK_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_PETCARD_USE_ROLLBACK_REPLY;
                case Command.SHOP_COMMAND_QUERY_CREATE_PET_CARD_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_CREATE_PET_CARD_REPLY;
                case Command.SHOP_COMMAND_QUERY_GIFT_INFO_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_GIFT_INFO_REPLY;
                case Command.SHOP_COMMAND_QUERY_GIFT_ACCEPT_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_GIFT_ACCEPT_REPLY;
                case Command.SHOP_COMMAND_QUERY_GIFT_REJECT_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_GIFT_REJECT_REPLY;
                case Command.SHOP_COMMAND_QUERY_ITEMSHOP_INFO_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_ITEMSHOP_INFO_REPLY;
                case Command.SHOP_COMMAND_QUERY_ITEMSHOP_USE_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_ITEMSHOP_USE_REPLY;
                case Command.SHOP_COMMAND_QUERY_ITEMSHOP_ROLLBACK_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_ITEMSHOP_ROLLBACK_REPLY;
                case Command.SHOP_COMMAND_QUERY_PREMIUMPACK_INFO_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_PREMIUMPACK_INFO_REPLY;
                case Command.SHOP_COMMAND_QUERY_FREESERVICE_INFO_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_FREESERVICE_INFO_REPLY;
                case Command.SHOP_COMMAND_QUERY_FREESERVICE_UPDATE_REQUEST:
                    return Command.SHOP_COMMAND_QUERY_FREESERVICE_UPDATE_REPLY;
                case Command.ACCOUNT_COMMAND_QUERY_PASSWORDCHANGE_2010_REQUEST:
                    return Command.ACCOUNT_COMMAND_QUERY_PASSWORDCHANGE_2010_REPLY;
                case Command.WEB_COMMAND_QUERY_MAIN_CHARACTER_NAME_REQUEST:
                    return Command.WEB_COMMAND_QUERY_MAIN_CHARACTER_NAME_REPLY;
                default:
                    return Command.MONITOR_COMMAND_QUERY_SESSION_INFO_INVALID;
            }
        }
    }
}
