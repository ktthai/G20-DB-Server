using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mabinogi;
using Mabinogi.SQL;

namespace Authenticator
{
    public static class Authenticator
    {
        private static void UnhandledExceptionRaised(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = (Exception)args.ExceptionObject;
            if (Console.Out != null)
            {
                Console.WriteLine(ex.ToString());
            }
            ExceptionMonitor.ExceptionRaised(ex);
        }

        public static void Start()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += UnhandledExceptionRaised;
            Console.WriteLine("Starting Authenticator");
            try
            {
                ServerConfiguration.Load();
                try
                {
                    if (!ServerConfiguration.IsLocalTestMode)
                    {
                        try
                        {
                            MySqlSimpleConnection sqlConnection = new MySqlSimpleConnection(ServerConfiguration.CharacterCardConnectionString);
                        }
                        catch(SimpleSqlException ex)
                        {
                            Console.WriteLine("SQL test failed:");
                            Console.WriteLine(ex.InnerException);
                            Console.ReadLine();
                        }
                    }
                }
                finally
                {
                }
                MainProcedure.ServerStart(ServerConfiguration.ServicePort);

            }
            catch (Exception ex)
            {
                try
                {
                    if (Console.Out != null)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    ExceptionMonitor.ExceptionRaised(ex);
                }
                finally
                {
                    MainProcedure.ServerStop();
                }
                return;
            }
            Console.WriteLine("launcher on");
            string text;
            while ((text = Console.ReadLine()) != null)
            {
                switch (text)
                {
                    case "configload":
                        ServerConfiguration.Load();
                        continue;
                    case "tranbegin":
                        {
                            Console.WriteLine("쿠폰 테스트");
                            Console.WriteLine("Begin Trasaction 테스트");
                            Console.Write("쿠폰 번호를 입력하세요:");
                            string data = Console.ReadLine();
                            Message message = new Message(112u, 0uL);
                            message.WriteS32(0);
                            message.WriteString(data);
                            message.WriteString("langley");
                            message.WriteS64(122341342L);
                            message.WriteString("테스트캐릭터");
                            message.WriteString("test_server");
                            message.WriteS64(1231231L);
                            RemoteFunction remoteFunction = RemoteFunction.Parse(0, message);
                            remoteFunction.Process();
                            continue;
                        }
                    case "trancommit":
                        {
                            Console.WriteLine("쿠폰 테스트");
                            Console.WriteLine("commit Trasaction 테스트");
                            Console.Write("쿠폰 번호를 입력하세요:");
                            string data3 = Console.ReadLine();
                            Message message3 = new Message(113u, 0uL);
                            message3.WriteS32(0);
                            message3.WriteString(data3);
                            message3.WriteString("langley");
                            message3.WriteS64(122341342L);
                            message3.WriteString("test_server");
                            RemoteFunction remoteFunction3 = RemoteFunction.Parse(0, message3);
                            remoteFunction3.Process();
                            continue;
                        }
                    case "tranrollback":
                        {
                            Console.WriteLine("쿠폰 테스트");
                            Console.WriteLine("rollback Trasaction 테스트");
                            Console.Write("쿠폰 번호를 입력하세요:");
                            string data2 = Console.ReadLine();
                            Message message2 = new Message(114u, 0uL);
                            message2.WriteS32(0);
                            message2.WriteString(data2);
                            message2.WriteString("langley2");
                            message2.WriteS64(122341342L);
                            message2.WriteString("test_server");
                            RemoteFunction remoteFunction2 = RemoteFunction.Parse(0, message2);
                            remoteFunction2.Process();
                            continue;
                        }
                    case "printlog":
                        if (ServerConfiguration.IsSessionLogToConsole)
                        {
                            Console.WriteLine("로그 출력을 중단합니다.");
                            ServerConfiguration.IsSessionLogToConsole = false;
                        }
                        else
                        {
                            Console.WriteLine("로그 출력을 시작합니다.");
                            ServerConfiguration.IsSessionLogToConsole = true;
                        }
                        continue;
                    default:
                        continue;
                    case "shutdown":
                        break;
                }
                break;
            }
            MainProcedure.ServerStop();
        }
    }
}
