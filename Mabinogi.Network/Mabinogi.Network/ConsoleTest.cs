using System;
using System.Collections;
using System.Net;

namespace Mabinogi.Network
{
	public class ConsoleTest
	{
		public static void Demo()
		{
			try
			{
				int num = 0;
				ConsoleTestServer consoleTestServer = new ConsoleTestServer();
				ArrayList arrayList = new ArrayList();
				TESTMODE test_mode = TESTMODE.TEST_NORMAL;
				while (true)
				{
					try
					{
						Console.WriteLine("서버를 구동할 포트를 입력해주세요");
						num = Convert.ToInt32(Console.ReadLine());
					}
					catch (FormatException)
					{
						Console.WriteLine("숫자를 입력하세요");
						continue;
					}
					break;
				}
				consoleTestServer.Start(num);
				Console.WriteLine("서버가 구동되었습니다");
				while (true)
				{
					Console.WriteLine("1. 클라이언트 추가하기");
					Console.WriteLine("2. 클라이언트 제거하기");
					Console.WriteLine("3. 기본 테스트");
					Console.WriteLine("4. 연결 접속 테스트");
					Console.WriteLine("5. 패킷 전송 테스트");
					Console.WriteLine("6. 대용량 패킷 전송 테스트");
					Console.WriteLine("7. 복합 테스트");
					Console.WriteLine("10. 서버 정보 보기");
					Console.WriteLine("11. 클라이언트 정보 보기");
					Console.WriteLine("12. 서버쪽에서 특정 클라이언트 접속 종료시키기");
					Console.WriteLine("20. 끝내기");
					int num2 = 0;
					while (true)
					{
						try
						{
							Console.WriteLine("수행할 테스트를 입력하세요");
							num2 = Convert.ToInt32(Console.ReadLine());
						}
						catch (FormatException)
						{
							continue;
						}
						break;
					}
					switch (num2)
					{
					case 11:
						break;
					case 1:
						try
						{
							Console.WriteLine("추가할 클라이언트를 입력하세요");
							int num4 = Convert.ToInt32(Console.ReadLine());
							for (int j = 0; j < num4; j++)
							{
								IPAddress iPAddress = Dns.GetHostEntry("localhost").AddressList[0];
								ConsoleTestClient consoleTestClient6 = new ConsoleTestClient();
								consoleTestClient6.test_mode = test_mode;
								consoleTestClient6.ConnectIP(iPAddress.ToString(), num);
								arrayList.Add(consoleTestClient6);
							}
						}
						catch (FormatException)
						{
							Console.WriteLine("잘못된 입력입니다");
						}
						break;
					case 2:
						try
						{
							Console.WriteLine("모든 클라이언트가 종료됩니다");
							foreach (ConsoleTestClient item in arrayList)
							{
								item.Stop();
								item.test_mode = TESTMODE.TEST_QUIT;
							}
							arrayList.Clear();
						}
						catch (FormatException)
						{
							Console.WriteLine("잘못된 입력입니다");
						}
						break;
					case 3:
						Console.WriteLine("===============================================================================");
						Console.WriteLine("모든 클라이언트를 기본 테스트 모드로 변경합니다");
						Console.WriteLine("기본 테스트 모드는 서버와 256byte~1024byte 사이의 데이터를 보내서, 서버의 에코를 기다립니다.");
						Console.WriteLine("===============================================================================");
						test_mode = TESTMODE.TEST_NORMAL;
						foreach (ConsoleTestClient item2 in arrayList)
						{
							item2.test_mode = test_mode;
						}
						break;
					case 4:
						Console.WriteLine("===============================================================================");
						Console.WriteLine("모든 클라이언트를 연결 접속 테스트로 변경합니다");
						Console.WriteLine("연결 접속 테스트는 10~20개사이의 패킷을 주보받고 컨넥션을 재 연결합니다.");
						Console.WriteLine("===============================================================================");
						test_mode = TESTMODE.TEST_CONNECT;
						foreach (ConsoleTestClient item3 in arrayList)
						{
							item3.test_mode = test_mode;
						}
						break;
					case 5:
						Console.WriteLine("===============================================================================");
						Console.WriteLine("모든 클라이언트를 연결 접속 테스트로 변경합니다");
						Console.WriteLine("패킷 과부하 테스트는 56byte~1024byte 사이의 데이를 보낸후에, 에코가 도착하면 메시지를 수를 두배씩 늘립니다");
						Console.WriteLine("메모리 부족등으로 프로그램이 종료될 위험이 있습니다");
						Console.WriteLine("너무 많은 클라이언트가 동작되지 않도록 주의해 주십시오");
						Console.WriteLine("===============================================================================");
						test_mode = TESTMODE.TEST_PACKET;
						foreach (ConsoleTestClient item4 in arrayList)
						{
							item4.test_mode = test_mode;
						}
						break;
					case 6:
						Console.WriteLine("===============================================================================");
						Console.WriteLine("대용량 패킷 테스트입니다.");
						Console.WriteLine("10K ~ 100K 사이의 패킷을 보내게 됩니다.");
						Console.WriteLine("===============================================================================");
						test_mode = TESTMODE.TEST_BIG_PACKET;
						foreach (ConsoleTestClient item5 in arrayList)
						{
							item5.test_mode = test_mode;
						}
						break;
					case 7:
						Console.WriteLine("===============================================================================");
						Console.WriteLine("복합 테스트입니다.");
						Console.WriteLine("512 ~ 10K 사이의 패킷을 보내게 됩니다.");
						Console.WriteLine("랜덤한 수의 패킷을 주고 받으면 연결을 종료합니다");
						Console.WriteLine("연결이 종료된 이후 얼마정도 시간이 지난후에 다시 서버에 연결을 하게 됩니다.");
						Console.WriteLine("===============================================================================");
						test_mode = TESTMODE.TEST_FUSION;
						foreach (ConsoleTestClient item6 in arrayList)
						{
							item6.test_mode = test_mode;
						}
						break;
					case 10:
					{
						Console.WriteLine("서버 상세 정보");
						Console.WriteLine("----------------------------------------------------------");
						Console.WriteLine("총 클라이언트 연결 시도 횟수 : {0}", consoleTestServer.StatisticInfo.TotalConnectedCount);
						Console.WriteLine("총 클라이언트 연결 완료 횟수 : {0}", consoleTestServer.StatisticInfo.TotalConfirmedConnectionCount);
						Console.WriteLine("총 클라이언트 연결 종료 횟수 : {0}", consoleTestServer.StatisticInfo.TotalClosedCount);
						Console.WriteLine("");
						Console.WriteLine("총 수신한 데이터 크기 : {0}", consoleTestServer.StatisticInfo.TotalReceiveDataSize);
						Console.WriteLine("총 수신 이벤트 발생 횟수 : {0}", consoleTestServer.StatisticInfo.TotalReceiveFunctionCalled);
						Console.WriteLine("총 수신 메시지 수 : {0}", consoleTestServer.StatisticInfo.TotalReceiveMsgCount);
						Console.WriteLine("");
						Console.WriteLine("총 전송한 데이터 크기 : {0}", consoleTestServer.StatisticInfo.TotalSendDataSize);
						Console.WriteLine("총 전송 이벤트 횟수 : {0}", consoleTestServer.StatisticInfo.TotalSendFunctionCalled);
						Console.WriteLine("총 전송 메시지 수 : {0}", consoleTestServer.StatisticInfo.TotalSendMsgCount);
						Console.WriteLine("----------------------------------------------------------");
						ServerInstanceInfo[] instanceInfo = consoleTestServer.InstanceInfo;
						if (instanceInfo == null)
						{
							Console.WriteLine("연결된 객체가 없습니다");
						}
						else
						{
							Console.WriteLine("{0} 개의 연결된 객체가 있습니다", instanceInfo.Length);
							ServerInstanceInfo[] array = instanceInfo;
							foreach (ServerInstanceInfo serverInstanceInfo in array)
							{
								Console.WriteLine("\t********************************************");
								Console.WriteLine("\t인스턴스 객체 {0} 의 정보", serverInstanceInfo.ID);
								Console.WriteLine("");
								Console.WriteLine("\t\t총 수신한 데이터 크기 : {0}", serverInstanceInfo.TotalReceiveDataSize);
								Console.WriteLine("\t\t총 수신 이벤트 발생 횟수 : {0}", serverInstanceInfo.TotalReceiveFunctionCalled);
								Console.WriteLine("\t\t총 수신 메시지 수 : {0}", serverInstanceInfo.TotalReceiveMsgCount);
								Console.WriteLine("");
								Console.WriteLine("\t\t총 전송한 데이터 크기 : {0}", serverInstanceInfo.TotalSendDataSize);
								Console.WriteLine("\t\t총 전송 이벤트 횟수 : {0}", serverInstanceInfo.TotalSendFunctionCalled);
								Console.WriteLine("\t\t총 전송 메시지 수 : {0}", serverInstanceInfo.TotalSendMsgCount);
							}
						}
						Console.WriteLine("----------------------------------------------------------");
						break;
					}
					case 12:
					{
						int num3 = 0;
						try
						{
							Console.Write("클라이언트 아이디(서버 정보를 참조하세요) : ");
							num3 = Convert.ToInt32(Console.ReadLine());
						}
						catch (FormatException)
						{
						}
						Console.WriteLine("클라이언트 {0} 를 종료시킵니다", num3);
						consoleTestServer.DestroyClient(num3);
						break;
					}
					case 20:
						Console.WriteLine("클라이언트를 종료합니다");
						foreach (ConsoleTestClient item7 in arrayList)
						{
							item7.Stop();
						}
						arrayList.Clear();
						Console.WriteLine("서버를 종료합니다");
						consoleTestServer.Stop();
						return;
					default:
						Console.WriteLine("제시된 커맨드만 동작합니다");
						break;
					}
				}
			}
			catch (Exception ex6)
			{
				Console.WriteLine(ex6.ToString());
			}
			Console.WriteLine("테스트를 끝냅니다");
		}
	}
}
