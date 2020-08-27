using Mabinogi;

namespace Authenticator
{
    public class RemoteFunction_QueryMainCharacterName : RemoteFunction
    {
        public override Message Process()
        {
            Message newReply = GetNewReply();
            string account = base.Input.ReadString();
            //WebMainCharacterName webMainCharacterName = new WebMainCharacterName(account, ServerConfiguration.IsLocalTestMode);
            //string serverName = string.Empty;
            //string characterName = string.Empty;
            //if (webMainCharacterName.QueryName(ref serverName, ref characterName))
            //{
            //    newReply.WriteU8(1);
            //    newReply.WriteString(serverName);
            //    newReply.WriteString(characterName);
            //}
            //else
            {
                newReply.WriteU8(0);
            }
            Reply(newReply);
            return newReply;
        }

        protected override string BuildName(Message _input)
        {
            string str = _input.ReadString();
            return "Query main character name (" + str + ")";
        }
    }
}
