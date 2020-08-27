using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WatsonTcp;

namespace Mabinogi.Network.External
{
    public class ClientSettings
    {
        public string IP { get; set; }
        public int Port { get; set; }
        public string CertPath { get; set; }
        public string CertPass { get; set; }
        public string PresharedKey { get; set; }

        public bool UsingPSK { get; set; }
        public bool UsingURL { get; set; }
        public bool UsingCert { get; set; }
        public bool MutualAuth { get; set; }
        public bool Reconnect { get; set; }
        public bool AcceptInvalidCertificates { get; set; }

        public static ClientSettings Default
        {
            get { return new ClientSettings() { AcceptInvalidCertificates = false, CertPass = "", CertPath = "", MutualAuth = true, Port = 16001, PresharedKey = "testpass12345678", UsingCert = false, UsingPSK = true }; }
        }
    }

    public class ExternalClient
    {
        protected WatsonTcpClient _client;
        private int _connectAttempts;
        private ClientSettings _settings;

        public bool IsConnected => _client.Connected;

        public ExternalClient()
        {

        }

        public void Start()
        {
            if (_settings == null)
            {
                Console.WriteLine("Client: No settings.");
                return;
            }

            string ip = _settings.IP;
            if (_settings.UsingURL)
            {
                var result = Dns.GetHostEntry(ip).AddressList;
                if (result.Length > 0)
                {
                    ip = result[0].ToString();
                }
                else
                {
                    throw new Exception("Failed to get host server address");
                }
            }

            if (_settings.UsingCert)
                _client = new WatsonTcpClient(ip, _settings.Port, _settings.CertPath, _settings.CertPass);
            else
                _client = new WatsonTcpClient(ip, _settings.Port);

            _client.ServerConnected += ServerConnected;
            _client.ServerDisconnected += ServerDisconnected;
            _client.MessageReceived += MessageReceived;
            _client.AcceptInvalidCertificates = _settings.AcceptInvalidCertificates;
            _client.MutuallyAuthenticate = _settings.MutualAuth;
        }

        public void Connect()
        {
            _client.Start();
        }

        public void Initialize(ClientSettings settings)
        {
            _settings = settings;
        }

        public bool Send(Message message)
        {
            return _client.Send(message.ToBuffer());
        }

        public void SendAsync(Message message)
        {
            _client.SendAsync(message.ToBuffer());
        }

        protected virtual void MessageReceived(object sender, MessageReceivedFromServerEventArgs e)
        {
            OnReceive(e.Data);
        }

        protected virtual void OnReceive(byte[] data)
        {

        }

        protected void ServerConnected(object sender, EventArgs e)
        {
            if (_settings.UsingPSK)
                _client.Authenticate(_settings.PresharedKey);
        }

        protected void ServerDisconnected(object sender, EventArgs e)
        {
            // TODO: Log disconnection information
            //Console.WriteLine("Server disconnected");

            //ClientDisconnected.Invoke();
            if (_settings.Reconnect)
                ConnectionLoop();
        }

        void ConnectionLoop()
        {
            bool result = false;
            _connectAttempts = 0;

            while (!result)
            {
                _client.Start();
                _connectAttempts++;
                if (_connectAttempts > 10)
                {
                    Thread.Sleep(1000 * (_connectAttempts / 10));
                }
                result = _client.Connected;
            }
        }
    }
}
