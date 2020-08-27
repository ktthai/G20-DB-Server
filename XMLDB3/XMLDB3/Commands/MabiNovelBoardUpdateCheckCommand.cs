using Mabinogi;

namespace XMLDB3
{
	internal class MabiNovelBoardUpdateCheckCommand : BasicCommand
	{
		private long sn_;

		private long updateTime_;

		private REPLY_RESULT result_ = REPLY_RESULT.ERROR;

		public MabiNovelBoardUpdateCheckCommand()
			: base(NETWORKMSG.NET_DB_MABINOVEL_NOVEL_BOARD_UPDATE_CHECK)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			sn_ = (long)_message.ReadU64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("MabiNovelBoardUpdateCheckCommand.DoProcess() : 함수에 진입하였습니다");
			result_ = QueryManager.MabiNovelBoard.GetUpdateTime(sn_, out updateTime_);
			if (result_ == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("MabiNovelBoardUpdateCheckCommand.DoProcess() : 업데이트 시간을 받아 왔습니다.");
			}
			else
			{
				WorkSession.WriteStatus("MabiNovelBoardUpdateCheckCommand.DoProcess() : 업데이트 시간을 못 받아 왔습니다.");
			}
			return result_ == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("MabiNovelBoardUpdateCheckCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)result_);
			message.WriteS64(updateTime_);
			return message;
		}
	}
}
