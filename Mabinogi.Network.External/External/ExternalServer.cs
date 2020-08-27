using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mabinogi.Network;
using WatsonTcp;

namespace Mabinogi.Network.External
{
    public class ServerSettings
    {
        public string IP { get; set; }
        public int Port { get; set; }
        public string CertPath { get; set; }
        public string CertPass { get; set; }
        public string PresharedKey { get; set; }

        public bool UsingPSK { get; set; }
        public bool UsingCert { get; set; }
        public bool MutualAuth { get; set; }
        public bool AcceptInvalidCertificates { get; set; }

        public static ServerSettings Default
        {
            get { return new ServerSettings() { AcceptInvalidCertificates = false, CertPass = "", CertPath = "", MutualAuth = false, Port = 16001, PresharedKey = "testpass12345678", UsingCert = false, UsingPSK = true }; }
        }
    }
    public class ExternalServer
    {
        protected WatsonTcpServer _server;

        public void Initialize(ServerSettings settings)
        {
            if (settings.UsingCert)
                _server = new WatsonTcpServer(settings.IP, settings.Port, settings.CertPath, settings.CertPass);
            else
                _server = new WatsonTcpServer(settings.IP, settings.Port);

            _server.ClientConnected += ClientConnected;
            _server.ClientDisconnected += ClientDisconnected;
            _server.MessageReceived += MessageReceived;
            _server.AuthenticationSucceeded += AuthenticationSucceeded;
            _server.AcceptInvalidCertificates = settings.AcceptInvalidCertificates;
            _server.MutuallyAuthenticate = settings.MutualAuth;
            if (settings.UsingPSK)
                _server.PresharedKey = settings.PresharedKey;
        }

        protected virtual void AuthenticationSucceeded(object sender, AuthenticationSucceededEventArgs e)
        {
            Console.WriteLine("Client authenticated: " + e.IpPort);
        }

        public void Start()
        {
            if (_server == null)
            {
                Console.WriteLine("ExternalServer: WARNING! YOU ARE USING DEFAULT SETTINGS!");
                Initialize(ServerSettings.Default);
            }
            _server.Start();

        }

        public void Stop()
        {
            _server.Dispose();
        }

        protected void ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            Console.WriteLine("Client connected: " + e.IpPort);
            if (_server.MutuallyAuthenticate)
                return;
        }

        protected virtual void ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            Console.WriteLine("Client disconnected: " + e.IpPort);
        }

        protected virtual void OnReceive(string ipPort, Message _msg)
        {

        }

        protected void SendMessageAsync(string ipPort, Message msg)
        {
            _server.SendAsync(ipPort, msg.ToBuffer());
        }

        protected bool SendMessage(string ipPort, Message msg)
        {
            return _server.Send(ipPort, msg.ToBuffer());
        }

        protected virtual void MessageReceived(object sender, MessageReceivedFromClientEventArgs e)
        {
            var msg = new Message(e.Data, e.Data.Length);

            OnReceive(e.IpPort, msg);
        }
    }
}
