using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLDB3.ItemMarket;

namespace XMLDB3
{
    public static class XMLDB3
    {
        public static void Start()
        {
            //TODO: Use[\u3131 - \uD79D] to translate all Korean messages
            XMLDB3Main.Start();
            if (ConfigManager.IsLocalMode)
            {
                Console.WriteLine("Running in local test mode");
            }
            else
            {
                Console.WriteLine("Running in sql mode");
                Console.Write("Testing sql connection : ");
                try
                {
                    ConfigManager.TestSqlConnection();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Sql connection failed. [Console]");
                    ExceptionMonitor.ExceptionRaised(ex);
                    XMLDB3Main.Shutdown();
                    Console.ReadLine();
                    return;
                }
                Console.WriteLine("done");
            }
            Console.WriteLine("launcher:on");
            string text;
            while ((text = Console.ReadLine()) != null)
            {
                text = text.ToLower();
                switch (text)
                {
                    case "clearexception":
                        XMLDB3Main.ClearException();
                        Console.WriteLine("All exceptions' cleared\n");
                        continue;
                    case "checkitemmarket":
                        ItemMarketManager.CheckHandlers();
                        continue;
                    default:
                        if (text.StartsWith("redirection"))
                        {
                            string[] array = text.Split(' ', '\t');
                            if (array.Length < 2)
                            {
                                Console.WriteLine("Usage : redirection [on|off]");
                                continue;
                            }
                            switch (array[1].ToLower())
                            {
                                case "on":
                                    CommandRedirection.Init(ConfigManager.RedirectionServer, ConfigManager.RedirectionPort);
                                    break;
                                case "off":
                                    CommandRedirection.Stop();
                                    break;
                                default:
                                    Console.WriteLine("Usage : redirection [on|off]");
                                    break;
                            }
                        }
                        else if (text.StartsWith("create_gmaccount"))
                        {
                            string[] array3 = text.Split(' ', '\t');
                            if (array3.Length < 2)
                            {
                                Console.WriteLine("Usage : create_gmaccount [account_id]");
                            }
                            else
                            {
                                QueryManager.CreateGMAccount(array3[1]);
                            }
                        }
                        else
                        {
                            if (!text.StartsWith("inventoryhash"))
                            {
                                continue;
                            }
                            string[] array4 = text.Split(' ', '\t');
                            if (array4.Length < 2)
                            {
                                Console.WriteLine("Usage : inventoryHash [on|off]");
                                continue;
                            }
                            switch (array4[1].ToLower())
                            {
                                case "on":
                                    ConfigManager.DoesCheckHash = true;
                                    break;
                                case "off":
                                    ConfigManager.DoesCheckHash = false;
                                    break;
                                default:
                                    Console.WriteLine("Usage : inventoryHash [on|off]");
                                    break;
                            }
                        }
                        continue;
                    case "shutdown":
                        break;
                }
                break;
            }
            XMLDB3Main.Shutdown();
        }
    }
}
