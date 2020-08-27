using Mabinogi;
using System;
using System.Threading;

namespace XMLDB3
{
	public class ProcessManager
	{
		private const int MaxThread = 20;

		private CommandQueue cmdQueue;

		private CommandQueue primeCmdQueue;

		private bool bSystemActive;

		private Thread[] processThread;

		private AutoResetEvent threadWait;

		private static ProcessManager processManager;

		public static void Start()
		{
			processManager = new ProcessManager();
		}

		public static void Shutdown()
		{
			processManager.Dispose();
		}

		public static void AddCommand(BasicCommand _Cmd)
		{
			processManager._AddCommand(_Cmd);
		}

		public static void AddPrimeCommand(BasicCommand _Cmd)
		{
			processManager._AddPrimeCommand(_Cmd);
		}

		private ProcessManager()
		{
			cmdQueue = new CommandQueue();
			primeCmdQueue = new CommandQueue();
			bSystemActive = true;
			threadWait = new AutoResetEvent(initialState: false);
			processThread = new Thread[20];
			for (int i = 0; i < 20; i++)
			{
				processThread[i] = new Thread(ProcessThread);
				processThread[i].Name = i.ToString();
				processThread[i].Start();
			}
		}

		private void Dispose()
		{
			bSystemActive = false;
			threadWait.Set();
			for (int i = 0; i < 20; i++)
			{
				processThread[i].Join();
			}
		}

		private void _AddCommand(BasicCommand _cmd)
		{
			lock (cmdQueue)
			{
				_cmd.Prepare();
				cmdQueue.Push(_cmd);
				threadWait.Set();
			}
		}

		private void _AddPrimeCommand(BasicCommand _cmd)
		{
			lock (primeCmdQueue)
			{
				_cmd.Prepare();
				primeCmdQueue.Push(_cmd);
				threadWait.Set();
			}
		}

		private static BasicCommand GetCommandFromQueue(CommandQueue _queue)
		{
			BasicCommand basicCommand = null;
			try
			{
				lock (_queue)
				{
					basicCommand = _queue.Pop();
					return basicCommand;
				}
			}
			catch (Exception ex)
			{
				basicCommand?.OnError();
				throw ex;
			}
		}

		private void ProcessThread()
		{
			while (true)
			{
				BasicCommand basicCommand = null;
				try
				{
					basicCommand = GetCommandFromQueue(primeCmdQueue);
					if (basicCommand == null)
					{
						basicCommand = GetCommandFromQueue(cmdQueue);
					}
					if (basicCommand != null)
					{
						WorkSession.Begin(Thread.CurrentThread.Name, basicCommand.GetType().Name, basicCommand.Target);
						try
						{
							WorkSession.WriteStatus("작업 세션을 시작합니다");
							for (BasicCommand basicCommand2 = basicCommand; basicCommand2 != null; basicCommand2 = basicCommand2.Next)
							{
								try
								{
									basicCommand2.DoProcess();
								}
								catch (Exception ex)
								{
									ExceptionMonitor.ExceptionRaised(ex, basicCommand2);
								}
							}
							for (BasicCommand basicCommand3 = basicCommand; basicCommand3 != null; basicCommand3 = basicCommand3.Next)
							{
								if (basicCommand3.ReplyEnable)
								{
									WorkSession.WriteStatus("ProcessManager.ProcessThread() : " + basicCommand3.ToString() + " 의 응답 메시지를 클아이언트에 전송합니다");
									Message msg = basicCommand3.MakeMessage();
									if (basicCommand3.IsExternalCommand)
									{
										DataBridge.ServerSend(basicCommand3.IpPort, msg);
									}
									else
									{ 
										MainProcedure.ServerSend(basicCommand3.Target, msg);
									}
								}
								else
								{
									WorkSession.WriteStatus("ProcessManager.ProcessThread() : " + basicCommand3.ToString() + " 의 응답 메시지가 없습니다.");
								}
							}
						}
						catch (Exception ex2)
						{
							ExceptionMonitor.ExceptionRaised(ex2, basicCommand);
							WorkSession.WriteStatus(ex2.ToString());
						}
						finally
						{
							WorkSession.WriteStatus("작업 세션을 종료합니다");
							WorkSession.End();
						}
					}
					else
					{
						if (!bSystemActive)
						{
							threadWait.Set();
							return;
						}
						threadWait.WaitOne();
					}
				}
				catch (Exception ex3)
				{
					basicCommand?.OnError();
					ExceptionMonitor.ExceptionRaised(ex3, basicCommand);
				}
			}
		}

		public static Message CacheStatisticsToMessage()
		{
			Message message = new Message(0u, 0uL);
			message.WriteS32(4);
			message += new CacheStatistics("").ToMessage();
			message += processManager.cmdQueue.Statistics.ToMessage();
			message += ObjectCache.Character.Statistics.ToMessage();
			return message + ObjectCache.Bank.Statistics.ToMessage();
		}
	}
}
