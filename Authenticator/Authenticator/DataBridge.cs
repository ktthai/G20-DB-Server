using Mabinogi;
using Mabinogi.Network.External;
using System;
using System.IO;
using System.Text.Json;

namespace Authenticator
{
    public class DataBridge : ExternalServer
    {
        private static DataBridge m_DataBridge = new DataBridge();
        private const string Config = "Auth_BridgeConfig.json";

        public static void ServerStart()
        {
            ServerSettings settings;
            if (File.Exists(Config))
            {
                string conf = File.ReadAllText(Config);
                settings = JsonSerializer.Deserialize<ServerSettings>(conf);
            }
            else
            {
                settings = new ServerSettings();
                File.WriteAllText(Config, JsonSerializer.Serialize(settings, new JsonSerializerOptions() { WriteIndented = true }));
            }
            m_DataBridge.Initialize(settings);
            m_DataBridge.Start();
        }

        public static void ServerStop()
        {
            m_DataBridge.Stop();
        }
            
        

        public static void ServerSend(string ipPort, Message _msg)
        {
            WorkSession.WriteStatus("ServerSend(" + ipPort + ", msg) : 함수에 진입하였습니다");

            // Change message opcode (ID)
            m_DataBridge.SendMessage(ipPort, _msg);
        }

        //protected override void OnExceptionRaised(int _id, Exception _ex)
        //{
        //ExceptionMonitor.ExceptionRaised(_ex, _id);
        //}

        protected override void OnReceive(string ipPort, Message _msg)
        {
            try
            {
                RemoteFunction remoteFunction = RemoteFunction.Parse(ipPort, _msg);
                if (remoteFunction != null)
                {
                    try
                    {
                        WorkSession.Begin(remoteFunction.Name, (int)_msg.Target);
                        remoteFunction.Process();
                    }
                    finally
                    {
                        WorkSession.End();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
            }
        }
    }
}