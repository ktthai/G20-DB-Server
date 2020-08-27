using System;
using System.Collections;
using System.Threading;

namespace XMLDB3
{
	public class CommandSerializer
	{
		private enum LockState
		{
			Invalid = -1,
			Created,
			Registered,
			Entered,
			Closed
		}

		private const int DefaultTimeout = -1;

		private static int timeout;

		private static Queue registerQueue;

		private static Thread workThread;

		private static ObjectLock objectLock;

		private static AutoResetEvent wakeupEvent;

		private static bool bActive;

		private LockState state = LockState.Invalid;

		private ILock lockInst;

		private ISerializableCommand command;

		public static void Initialize(int _timeout)
		{
			timeout = _timeout;
			bActive = true;
			objectLock = new ObjectLock();
			wakeupEvent = new AutoResetEvent(initialState: false);
			registerQueue = new Queue();
			workThread = new Thread(MainLoop);
			workThread.Start();
		}

		public static void Initialize()
		{
			Initialize(-1);
		}

		public static void Shutdown()
		{
			bActive = false;
			wakeupEvent.Set();
			workThread.Join();
		}

		public CommandSerializer(ISerializableCommand _command)
		{
			command = _command;
			state = LockState.Created;
			lock (registerQueue.SyncRoot)
			{
				registerQueue.Enqueue(this);
				wakeupEvent.Set();
			}
		}

		public void Wait()
		{
			Monitor.Enter(this);
			try
			{
				while (state != LockState.Registered)
				{
					if (state != 0)
					{
						throw new Exception("상태 값이 잘못 되었습니다.");
					}
					WorkSession.WriteStatus(command.ToString() + "의 락이 할당되기를 대기합니다.");
					if (!Monitor.Wait(this, timeout))
					{
						throw new Exception("락 대기 타임 아웃");
					}
				}
				WorkSession.WriteStatus(command.ToString() + "의 명령순서를 대기합니다.");
				lockInst.Wait();
			}
			finally
			{
				Monitor.Exit(this);
			}
		}

		public bool Close()
		{
			lock (this)
			{
				switch (state)
				{
				case LockState.Closed:
					throw new Exception("이미 자원이 해재되었습니다.");
				case LockState.Registered:
					Close(this);
					break;
				case LockState.Invalid:
					Close(this);
					break;
				}
				state = LockState.Closed;
			}
			return true;
		}

		private static void Close(CommandSerializer _cs)
		{
			Monitor.Enter(objectLock.SyncRoot);
			try
			{
				WorkSession.WriteStatus(_cs.command.ToString() + "에 락을 해제합니다.");
				if (_cs != null && _cs.lockInst != null)
				{
					try
					{
						_cs.command.OnSerialize(_cs.lockInst.EndHelper, bBegin: false);
					}
					catch (Exception ex)
					{
						MailSender.Send(ex.ToString());
						ExceptionMonitor.ExceptionRaised(ex, _cs.command);
						_cs.lockInst.ForceUnregist();
					}
					_cs.lockInst.Close();
					if (objectLock.Available > 0)
					{
						Monitor.Pulse(objectLock.SyncRoot);
					}
				}
			}
			finally
			{
				Monitor.Exit(objectLock.SyncRoot);
			}
		}

		private static ILock CreateLock()
		{
			Monitor.Enter(objectLock.SyncRoot);
			try
			{
				while (objectLock.Available == 0)
				{
					WorkSession.WriteStatus("여유 락이 생길때 까지 대기합니다.");
					if (!Monitor.Wait(objectLock.SyncRoot, timeout))
					{
						throw new Exception("락 생성 타임아웃");
					}
				}
				return objectLock.Create();
			}
			catch (Exception ex)
			{
				ExceptionMonitor.ExceptionRaised(ex);
				return null;
			}
			finally
			{
				Monitor.Exit(objectLock.SyncRoot);
			}
		}

		private static void MainLoop()
		{
			while (true)
			{
				CommandSerializer commandSerializer = null;
				lock (registerQueue.SyncRoot)
				{
					if (registerQueue.Count > 0)
					{
						commandSerializer = (CommandSerializer)registerQueue.Dequeue();
					}
				}
				if (commandSerializer != null)
				{
					WorkSession.Begin("CommandSerializer", null, 0);
					ILock @lock = CreateLock();
					Monitor.Enter(commandSerializer);
					try
					{
						if (@lock == null)
						{
							WorkSession.WriteStatus(commandSerializer.command.ToString() + "의 락을 얻어오는데 실패했습니다.");
							commandSerializer.state = LockState.Invalid;
						}
						else
						{
							WorkSession.WriteStatus(commandSerializer.command.ToString() + "의 락을 생성하였습니다.");
							if (commandSerializer.state == LockState.Created)
							{
								WorkSession.WriteStatus(commandSerializer.command.ToString() + "에 락[" + @lock.ToString() + "]을 할당합니다.");
								commandSerializer.lockInst = @lock;
								lock (objectLock.SyncRoot)
								{
									WorkSession.WriteStatus(commandSerializer.command.ToString() + "에 락을 겁니다.");
									commandSerializer.command.OnSerialize(commandSerializer.lockInst.BeginHelper, bBegin: true);
								}
								commandSerializer.state = LockState.Registered;
							}
							else
							{
								commandSerializer.state = LockState.Invalid;
							}
						}
					}
					catch (Exception ex)
					{
						ExceptionMonitor.ExceptionRaised(ex);
						commandSerializer.state = LockState.Invalid;
					}
					finally
					{
						WorkSession.WriteStatus(commandSerializer.command.ToString() + "에 할당신호를 보냅니다.");
						Monitor.Pulse(commandSerializer);
						Monitor.Exit(commandSerializer);
						WorkSession.End();
					}
				}
				else
				{
					if (!bActive)
					{
						break;
					}
					wakeupEvent.WaitOne();
				}
			}
		}
	}
}
