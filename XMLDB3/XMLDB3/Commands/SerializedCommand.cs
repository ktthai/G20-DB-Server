using Mabinogi;
using System;

namespace XMLDB3
{
	public abstract class SerializedCommand : BasicCommand, ISerializableCommand
	{
		private CommandSerializer serializer;

		protected SerializedCommand(NETWORKMSG _MsgID)
			: base(_MsgID)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			_ReceiveData(_Msg);
		}

		public override bool Prepare()
		{
			serializer = new CommandSerializer(this);
			return true;
		}

		protected abstract void _ReceiveData(Message _Msg);

		public override bool DoProcess()
		{
			try
			{
				if (serializer == null)
				{
					throw new Exception("시리얼라이저가 없습니다.");
				}
				serializer.Wait();
				return _DoProces();
			}
			catch (Exception ex)
			{
				ExceptionMonitor.ExceptionRaised(ex, this);
				WorkSession.WriteStatus(ex.Message);
				return false;
			}
			finally
			{
				try
				{
					if (serializer != null)
					{
						serializer.Close();
					}
				}
				catch (Exception ex2)
				{
					ExceptionMonitor.ExceptionRaised(ex2, this);
				}
				finally
				{
					serializer = null;
				}
			}
		}

		public override void OnError()
		{
			if (serializer != null)
			{
				serializer.Close();
			}
			serializer = null;
			base.OnError();
		}

		protected abstract bool _DoProces();

		public abstract void OnSerialize(IObjLockRegistHelper _helper, bool bBegin);
	}
}
