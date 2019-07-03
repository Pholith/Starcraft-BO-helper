using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Starcraft_BO_helper
{
    class UDP_Controller
    {
        UdpClient udpClient;
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort); // endpoint where server is listening

        private const int listeningPort = 22022; // listeningPort of the client
        private const int serverPort = 22022; // listeningPort of the server
        private const string serverIP = "90.3.160.150";  // IP of the server

        // Send a message original method
        private void SendUdp(int srcPort, string dstIp, int dstPort, byte[] data)
        {
            udpClient.Send(data, data.Length);
        }
        // Send a message simplified method
        internal void sendMessageToServer(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);

            SendUdp(serverPort, serverIP, listeningPort, bytes);
        }

        // Log application using to have utilisation informations
        public UDP_Controller()
        {
            udpClient = new UdpClient();
            udpClient.Connect(endPoint);
            sendMessageToServer("Ping");
            Console.WriteLine("Ping send to log server.");
        }
    }
}
