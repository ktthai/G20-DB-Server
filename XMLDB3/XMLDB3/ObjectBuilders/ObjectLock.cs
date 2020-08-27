using System;
using System.Collections;
using System.Threading;

namespace XMLDB3
{
	public class ObjectLock
	{
		private delegate void ObjectIDHelper(long _id);

		private delegate void StringIDHelper(string _id);

		private class ObjLockRegistHelper : IObjLockRegistHelper
		{
			private ObjectIDHelper objHelper;

			private StringIDHelper strHelper;

			public ObjLockRegistHelper(ObjectIDHelper _objHelper, StringIDHelper _strHelper)
			{
				objHelper = _objHelper;
				strHelper = _strHelper;
			}

			public void ObjectIDRegistant(long _id)
			{
				objHelper(_id);
			}

			public void StringIDRegistant(string _id)
			{
				strHelper(_id);
			}
		}

		private class LockInstance : ILock
		{
			private int lockID;

			private ObjectLock creator;

			private AutoResetEvent onLock;

			protected uint waitingLockID;

			public bool bIsValidLock;

			private ObjLockRegistHelper beginHelper;

			private ObjLockRegistHelper endHelper;

			IObjLockRegistHelper ILock.BeginHelper => beginHelper;

			IObjLockRegistHelper ILock.EndHelper => endHelper;

			public int LockID => lockID;

			void ILock.Close()
			{
				creator.Close(this);
			}

			void ILock.ForceUnregist()
			{
				creator.ForceUnlock(this);
			}

			public LockInstance(int _lockID, ObjectLock _creator)
			{
				lockID = _lockID;
				creator = _creator;
				beginHelper = new ObjLockRegistHelper(RegisterObjectID, RegisterStringID);
				endHelper = new ObjLockRegistHelper(CloseObjectID, CloseStringID);
				onLock = new AutoResetEvent(initialState: false);
			}

			public void Init()
			{
				bIsValidLock = true;
				waitingLockID = 0u;
				onLock.Reset();
			}

			public override string ToString()
			{
				return string.Concat(lockID + ":" + waitingLockID);
			}

			void ILock.Wait()
			{
				if (waitingLockID != 0)
				{
					if (creator.Timeout == -1)
					{
						onLock.WaitOne();
					}
					else if (!onLock.WaitOne(creator.Timeout, exitContext: false))
					{
						throw new Exception("락 대기 타임아웃:[" + ToString() + "]");
					}
				}
			}

			public bool WakeUp(int _lockID)
			{
				waitingLockID &= (uint)(~(1 << _lockID));
				if (waitingLockID == 0)
				{
					onLock.Set();
					return true;
				}
				return false;
			}

			private void RegisterObjectID(long _id)
			{
				waitingLockID |= creator.LockObjectID(_id, this);
			}

			private void RegisterStringID(string _id)
			{
				waitingLockID |= creator.LockStringID(_id, this);
			}

			private void CloseObjectID(long _id)
			{
				creator.UnlockObjectID(_id, this);
			}

			private void CloseStringID(string _id)
			{
				creator.UnlockStringID(_id, this);
			}
		}

		public const int MaxConcurrentLock = 32;

		public const int DefaultLockTimeout = 120000;

		private int timeout;

		private Queue lockPool;

		private LockInstance[] waitingInstants;

		private uint waitingLockIDs;

		private Hashtable stringHash;

		private Hashtable longHash;

		public object SyncRoot => this;

		public int Available => lockPool.Count;

		public int Timeout => timeout;

		public ObjectLock()
			: this(120000)
		{
		}

		public ObjectLock(int _timeout)
		{
			lockPool = new Queue(33);
			for (int i = 0; i < 32; i++)
			{
				LockInstance obj = new LockInstance(i, this);
				lockPool.Enqueue(obj);
			}
			waitingInstants = new LockInstance[32];
			timeout = _timeout;
			stringHash = new Hashtable(64);
			longHash = new Hashtable(3200);
		}

		public ILock Create()
		{
			if (lockPool.Count > 0)
			{
				LockInstance lockInstance = (LockInstance)lockPool.Dequeue();
				try
				{
					lockInstance.Init();
					waitingInstants[lockInstance.LockID] = null;
					return lockInstance;
				}
				catch (Exception ex)
				{
					lockPool.Enqueue(lockInstance);
					throw ex;
				}
			}
			throw new Exception("더 이상 락을 사용할 수 없습니다.");
		}

		private void Close(LockInstance _lockInst)
		{
			for (int i = 0; i < 32; i++)
			{
				if (waitingInstants[i] != null && waitingInstants[i].WakeUp(_lockInst.LockID))
				{
					waitingInstants[i] = null;
				}
			}
			waitingLockIDs &= (uint)(~(1 << _lockInst.LockID));
			lockPool.Enqueue(_lockInst);
		}

		private uint LockStringID(string _stringID, LockInstance _inst)
		{
			return Lock(_stringID, stringHash, _inst);
		}

		private uint LockObjectID(long _objectID, LockInstance _inst)
		{
			return Lock(_objectID, longHash, _inst);
		}

		private uint Lock(object _object, Hashtable _lockTable, LockInstance _inst)
		{
			uint num = (uint)(1 << _inst.LockID);
			if (_lockTable.ContainsKey(_object))
			{
				uint num2 = (uint)_lockTable[_object];
				if ((num2 & num) == 0)
				{
					WorkSession.WriteStatus("ObjectLock.Lock() : 이미 락 개체[" + num2 + "]가 아이디를 점유하고 있습니다.");
					_lockTable[_object] = (num2 | num);
					if (waitingInstants[_inst.LockID] == null)
					{
						if ((num & waitingLockIDs) != 0)
						{
							throw new Exception("데드락 가능 상황입니다. [" + waitingLockIDs + "|" + num + "]");
						}
						waitingLockIDs |= num2;
						waitingInstants[_inst.LockID] = _inst;
					}
					return num2;
				}
				throw new Exception("락 테이블에 이상이 있습니다.");
			}
			_lockTable[_object] = num;
			return 0u;
		}

		private void UnlockStringID(string _stringID, LockInstance _inst)
		{
			Unlock(_stringID, stringHash, _inst);
		}

		private void UnlockObjectID(long _objectID, LockInstance _inst)
		{
			Unlock(_objectID, longHash, _inst);
		}

		private bool Unlock(object _object, Hashtable _lockTable, LockInstance _inst)
		{
			if (_lockTable.Contains(_object))
			{
				uint num = (uint)(1 << _inst.LockID);
				uint num2 = (uint)_lockTable[_object];
				if (num == num2)
				{
					_lockTable.Remove(_object);
					return true;
				}
				if ((num & num2) != 0)
				{
					_lockTable[_object] = (num2 & ~num);
					return false;
				}
				if (_inst.bIsValidLock)
				{
					ExceptionMonitor.ExceptionRaised(new Exception("락이 재대로 풀리지 않았습니다."), _inst.LockID, _object);
					return false;
				}
				return true;
			}
			ExceptionMonitor.ExceptionRaised(new Exception("락이 재대로 풀리지 않았습니다."), _inst.LockID, _object);
			return true;
		}

		private void ForceUnlock(LockInstance _inst)
		{
			stringHash = _ForceUnlock(stringHash, _inst);
			longHash = _ForceUnlock(longHash, _inst);
		}

		private Hashtable _ForceUnlock(Hashtable _lockTable, LockInstance _inst)
		{
			uint num = (uint)(1 << _inst.LockID);
			Hashtable hashtable = (Hashtable)_lockTable.Clone();
			IDictionaryEnumerator enumerator = _lockTable.GetEnumerator();
			while (enumerator.MoveNext())
			{
				uint num2 = (uint)enumerator.Value;
				if (num2 == num)
				{
					hashtable.Remove(enumerator.Key);
				}
				else if ((num2 & num) != 0)
				{
					hashtable[enumerator.Key] = (num2 & ~num);
				}
			}
			_lockTable = null;
			return hashtable;
		}
	}
}
