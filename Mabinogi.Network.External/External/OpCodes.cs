﻿
namespace Mabinogi.Network.External
{
    public enum OpCodes
    {
        // DB
        DB_ACCOUNT_CREATE = 11,                     // MC_DB_ACCOUNT_CREATE = 11,
        DB_ACCOUNT_UPDATE = 12,                     // MC_DB_ACCOUNT_UPDATE = 12,
        DB_ACCOUNT_READ = 13,                       //  MC_DB_ACCOUNT_READ = 13,
        DB_ACCOUNT_DELETE = 14,                     // MC_DB_ACCOUNT_DELETE = 14,
        DB_CHARACTER_CREATE = 15,                   // MC_DB_CHARACTER_CREATE = 15,
        DB_CHARACTER_UPDATE = 16,                   // MC_DB_CHARACTER_UPDATE = 16,
        DB_CHARACTER_READ = 17,                     // MC_DB_CHARACTER_READ = 17,
        DB_CHARACTER_DELETE = 18,                   // MC_DB_CHARACTER_DELETE = 18,
        AUTH_CHARACTERCARD_CREATE = 19,             // SHOP_COMMAND_QUERY_CREATE_CHARACTER_CARD_REQUEST = 192,
        AUTH_CHARACTERCARD_READ = 20,               // SHOP_COMMAND_QUERY_CHARACTERCARD_INFO_REQUEST = 101,
        AUTH_CHARACTERCARD_DELETE = 21,             // SHOP_COMMAND_QUERY_DELETE_CHARACTERCARD_REQUEST = 118,
        DB_GUILD_READ = 22,                         // NET_DB_GUILD_READ = 47,
        DB_GUILDLIST_READ = 24,                     // NET_DB_GUILDLIST_READ = 49,
        DB_GUILD_POINT_ADD = 25,                    // NET_DB_GUILD_POINT_ADD = 51,
        DB_GUILD_MONEY_ADD = 26,                    // NET_DB_GUILD_MONEY_ADD = 52,
        DB_GUILD_MEMEBER_CHECK_JOINTIME = 27,       // NET_DB_GUILD_MEMEBER_CHECK_JOINTIME = 57,
        DB_GUILD_DRAW_MONEY = 28,                   // NET_DB_GUILD_DRAW_MONEY = 58,
        DB_GUILD_CHANGE_GUILDSTONE = 29,            // NET_DB_GUILD_CHANGE_GUILDSTONE = 59,
        DB_ITEM_USABLE_ID = 30,                     // NET_DB_ITEM_USABLE_ID = 60,
        DB_CHARACTER_USABLE_ID = 31,                // NET_DB_CHARACTER_USABLE_ID = 61,
        DB_ITEM_ID_POOL = 32,                       // NET_DB_ITEM_ID_POOL = 62,
        DB_GUILD_UPDATE_TITLE = 33,                 // NET_DB_GUILD_UPDATE_TITLE = 69,
        DB_SIGNAL_LOGIN = 34,                       // NET_DB_SIGNAL_LOGIN = 70,
        DB_SIGNAL_LOGOUT = 35,                      // NET_DB_SIGNAL_LOGOUT = 71,
        DB_SIGNAL_PLAYIN = 36,                      // NET_DB_SIGNAL_PLAYIN = 72,
        DB_SIGNAL_PLAYOUT = 37,                     // NET_DB_SIGNAL_PLAYOUT = 73,
        DB_GUILD_JOINED_MEMBER_COUNT = 38,          // NET_DB_GUILD_JOINED_MEMBER_COUNT = 79,
        DB_PET_CREATE = 39,                         // NET_DB_PET_CREATE = 80,
        DB_PET_UPDATE = 40,                         // NET_DB_PET_UPDATE = 81,
        DB_PET_READ = 41,                           // NET_DB_PET_READ = 82,
        AUTH_PETCARD_CREATE = 42,                   // SHOP_COMMAND_QUERY_CREATE_PET_CARD_REQUEST = 194,
        AUTH_PETCARD_READ = 44,                     // SHOP_COMMAND_QUERY_PETCARD_INFO_REQUEST = 150,
        //AUTH_PETCARD_DELETE = 45,                 // Function doesn't exist
        DB_CASTLE_LIST_READ = 48,                   // NET_DB_CASTLE_LIST_READ = 100,
        DB_HOUSE_READ = 49,                         // NET_DB_HOUSE_READ = 150,
        DB_HOUSE_ITEM_READ = 50,                    // NET_DB_HOUSE_ITEM_READ = 158,
        DB_MEMO_SEND = 51,                          // NET_DB_MEMO_SEND = 200,
        DB_ITEM_DELETE = 52,                        // NET_DB_ITEM_DELETE = 300,
        DB_PET_ITEM_DELETE = 53,                    // NET_DB_PET_ITEM_DELETE = 301,              
        DB_RUIN_READ = 54,                          // NET_DB_RUIN_READ = 500,
        DB_RELIC_READ = 55,                         // NET_DB_RELIC_READ = 600,
        DB_MAIL_READ = 56,                          // NET_DB_MAIL_READ = 1000,
        DB_MAIL_SEND = 57,                          // NET_DB_MAIL_SEND = 1001,
        DB_MAIL_DELETE = 58,                        // NET_DB_MAIL_DELETE = 1002,
        DB_MAIL_UPDATE_STATUS = 59,                 // NET_DB_MAIL_UPDATE_STATUS = 1003,
        DB_MAIL_CHECK_CHARACTER = 60,               // NET_DB_MAIL_CHECK_CHARACTER = 1004,
        DB_MAIL_GET_UNREAD_COUNT = 61,              // NET_DB_MAIL_GET_UNREAD_COUNT = 1005,
        DB_FARM_READ = 62,                          // NET_DB_FARM_READ = 1100,
        DB_AUCTION_BID_READ = 64,                   // NET_DB_AUCTION_BID_READ = 1201,
        DB_AUCTION_BID_ADD = 65,                    // NET_DB_AUCTION_BID_ADD = 1202,
        DB_AUCTION_BID_UPDATE = 66,                 // NET_DB_AUCTION_BID_UPDATE = 1203,
        DB_AUCTION_BID_REMOVE = 67,                 // NET_DB_AUCTION_BID_REMOVE = 1204,
        DB_WINE_AGING_READ = 68,                    // NET_DB_WINE_AGING_READ = 1500,
        DB_BAN_ACCOUNT = 69,                        // NET_DB_BAN_ACCOUNT = 1503,
        DB_UNBAN_ACCOUNT = 70,                      // NET_DB_UNBAN_ACCOUNT = 1504,
        DB_QUERY_ACCUM_LEVEL = 71,                  // NET_DB_QUERY_ACCUM_LEVEL = 1506,
        DB_ROYALALCHEMIST_READ = 72,                // NET_DB_ROYALALCHEMIST_READ = 1510,
        DB_ROYALALCHEMIST_LIST = 73,                // NET_DB_ROYALALCHEMIST_LIST = 1511,
        DB_FAMILY_READ = 74,                        // NET_DB_FAMILY_READ = 1518,
        DB_LOGIN_ID_POOL = 75,                      // NET_DB_LOGIN_ID_POOL = 1650,
        DB_PRIVATEFARM_READ = 76,                   // NET_DB_PRIVATEFARM_READ = 1702,
        DB_SCRAPBOOK_SCRAP = 77,                    // NET_DB_SCRAPBOOK_SCRAP = 1708,
        DB_SCRAPBOOK_QUERY_SCRAPPED_LIST = 78,      // NET_DB_SCRAPBOOK_QUERY_SCRAPPED_LIST = 1709,
        DB_SCRAPBOOK_QUERY_BEST_COOK_LIST = 79,     // NET_DB_SCRAPBOOK_QUERY_BEST_COOK_LIST = 1710,
        DB_COMMERCE_T_READ_ALL_DATA = 80,           // NET_DB_COMMERCE_T_READ_ALL_DATA = 1712,
        DB_COMMERCE_T_QUERY_DUCAT = 81,             // NET_DB_COMMERCE_T_QUERY_DUCAT = 1719,
        DB_COMMERCE_E_READ_ALL = 82,                // NET_DB_COMMERCE_E_READ_ALL = 1720,
        DB_COMMERCE_CRIMINAL_READ_ALL = 83,         // NET_DB_COMMERCE_CRIMINAL_READ_ALL = 1725,
        DB_COMMERCE_CRIMINAL_UPDATE = 84,           // NET_DB_COMMERCE_CRIMINAL_UPDATE = 1726,
        DB_PAWN_COIN_MODIFY = 85,                   // NET_DB_PAWN_COIN_MODIFY = 1733,
        DB_PRIVATE_FARM_UPDATETIME_SELECT = 86,     // NET_DB_PRIVATE_FARM_UPDATETIME_SELECT = 1741,
        DB_SOULMATE_READ_LIST = 87,                 // NET_DB_SOULMATE_READ_LIST = 1746,
        DB_PERSONAL_RANKING_READ = 88,              // NET_DB_PERSONAL_RANKING_READ = 1749,
        DB_PERSONAL_RANKING_UPDATE_SOCRE = 89,      // NET_DB_PERSONAL_RANKING_UPDATE_SOCRE = 1750,
        DB_PERSONAL_RANKING_REMOVE_SCORE = 90,      // NET_DB_PERSONAL_RANKING_REMOVE_SCORE = 1751,
        DB_PERSONAL_RANKING_READ2 = 92,             // NET_DB_PERSONAL_RANKING_READ2 = 1753,
        DB_HELP_POINT_LIST_LOAD = 94,               // NET_DB_HELP_POINT_LIST_LOAD = 1764,
        DB_HELP_POINT_UPDATE = 95,                  // NET_DB_HELP_POINT_UPDATE = 1765,
        DB_HELP_POINT_DECREASE = 96,                // NET_DB_HELP_POINT_DECREASE = 1766,
    }
}