using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace Service
{
    public class Client
    {
        private string _ip = "";

        private int _port;

        public Client(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        /// <summary>
        /// 連線至主機
        /// </summary>
        public string ConnectToServer(string Message)
        {
            string ResponseString = "";

            //預設主機IP
            string hostIP = _ip;

            //先建立IPAddress物件,IP為欲連線主機之IP
            IPAddress ipa = IPAddress.Parse(hostIP);

            //建立IPEndPoint
            IPEndPoint ipe = new IPEndPoint(ipa, _port);

            //先建立一個TcpClient;
            TcpClient tcpClient = new TcpClient();

            //開始連線

            tcpClient.Connect(ipe);

            CommunicationBase cb = new CommunicationBase();

            cb.SendMsg(Message, tcpClient);

            ResponseString = cb.ReceiveMsg(tcpClient);

            tcpClient.Close();

            return ResponseString;
        }
    }
}