using Mabinogi;
using System;
using System.Collections;
using System.Reflection;

namespace XMLDB3
{
    public abstract class BasicCommand : ICloneable
    {
        private int m_TargetClient;

        private uint m_ID;

        private uint m_QueryId;

        protected NETWORKMSG m_MsgID;

        private bool m_IsValid;

        private BasicCommand m_ChainCommand;

        private static Hashtable commandDic = new Hashtable();

        public string IpPort;
        public bool IsExternalCommand;
        public virtual bool IsPrimeCommand => false;

        public NETWORKMSG MsgID => m_MsgID;

        public int Target => m_TargetClient;

        public uint ID => m_ID;

        public uint QueryID => m_QueryId;

        public BasicCommand Next
        {
            get
            {
                return m_ChainCommand;
            }
            set
            {
                m_ChainCommand = value;
            }
        }

        public bool IsValid => m_IsValid;

        public virtual bool ReplyEnable => true;

        public object Clone()
        {
            return MemberwiseClone();
        }

        protected BasicCommand(NETWORKMSG _MsgID)
        {
            m_MsgID = _MsgID;
        }

        public virtual bool Prepare()
        {
            return true;
        }

        public abstract bool DoProcess();

        public abstract Message MakeMessage();

        public virtual void OnError()
        {
            if (m_IsValid && ReplyEnable)
            {
                Message msg = MakeMessage();
                MainProcedure.ServerSend(Target, msg);
            }
        }

        protected abstract void ReceiveData(Message _Msg);

        public override string ToString()
        {
            return GetType().Name + ":" + m_ID + ":" + (m_IsValid ? "Valid" : "Invalid");
        }

        public static void Initialize()
        {
            Assembly callingAssembly = Assembly.GetCallingAssembly();
            Type[] types = callingAssembly.GetTypes();
            Type[] array = types;
            foreach (Type type in array)
            {
                if (type.IsClass && !type.IsAbstract && HasBaseType(type, "BasicCommand"))
                {
                    BasicCommand basicCommand = (BasicCommand)callingAssembly.CreateInstance(type.FullName);
                    if (!commandDic.ContainsKey(basicCommand.MsgID))
                    {
                        commandDic.Add(basicCommand.MsgID, basicCommand);
                    }
                }
            }
        }

        private static bool HasBaseType(Type type, string typeName)
        {
            Type type2 = type;
            do
            {
                type2 = type2.BaseType;
                if (type2 != null && type2.Name == typeName)
                {
                    return true;
                }
            }
            while (type2 != null);
            return false;
        }

        public static BasicCommand Parse(int _ClientId, Message _Msg)
        {
            var result = Parse(_Msg);
            if (result == null)
                throw new Exception("connection " + _ClientId + " send invalid message " + _Msg.ID);
            result.m_TargetClient = _ClientId;
            return result;
        }

        public static BasicCommand Parse(string ipPort,  Message _Msg)
        {
            var result = Parse(_Msg);
            if (result == null)
                throw new Exception("connection " + ipPort + " send invalid message " + _Msg.ID);
            result.IpPort = ipPort;
            result.IsExternalCommand = true;
            result.m_TargetClient = (int)_Msg.Target;
            return result;
        }

        private static BasicCommand Parse(Message _Msg)
        {
            uint queryId = _Msg.ReadU32();
            BasicCommand basicCommand = null;
            if (commandDic.ContainsKey((NETWORKMSG)_Msg.ID))
            {
                basicCommand = (BasicCommand)commandDic[(NETWORKMSG)_Msg.ID];
            }
            if (basicCommand != null)
            {
                BasicCommand basicCommand2 = (BasicCommand)basicCommand.Clone();
                if (basicCommand2 != null)
                {
                    basicCommand2.m_ID = _Msg.ID;
                    basicCommand2.m_QueryId = queryId;
                    try
                    {
                        basicCommand2.ReceiveData(_Msg);
                    }
                    catch (Exception ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex, basicCommand2);
                        basicCommand2.OnError();
                        return null;
                    }
                    basicCommand2.m_IsValid = true;
                    return basicCommand2;
                }
            }
            return null;
        }
    }
}
