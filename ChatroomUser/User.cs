using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatroomUser
{
    class User
    {
        private NetworkStream networkStream;
        private TcpClient client;
        private string ipAddress = "192.168.1.77";
        private int portNumber = 52262;

        public User()
        {
            try
            {
                client = new TcpClient(ipAddress, portNumber);
            }
            catch
            {
                Console.WriteLine("Cannot connect to server");


            }
        }
        private void ConnectToNetwork(TcpClient tcpClient)
        {
            networkStream = tcpClient.GetStream();
            Console.WriteLine("Connected");
        }
        private void SendMessage()
        {
            while (true)
            {
                string message = Console.ReadLine();
                try
                {
                    byte[] dataToSend = ASCIIEncoding.ASCII.GetBytes(message);
                    Console.WriteLine("Sending");
                    networkStream.Write(dataToSend, 0, dataToSend.Length);
                    Console.WriteLine("message sent");
                }
                catch
                {
                    Console.WriteLine("failed to reach server");
                }
            }
        }
        private void ReceiveMessage()
        {
            while (networkStream != null)
            {
                try
                {
                    
                    byte[] textToRead = new byte[client.ReceiveBufferSize];
                    int bytesRead = networkStream.Read(textToRead, 0, client.ReceiveBufferSize);
                    Console.WriteLine(Encoding.ASCII.GetString(textToRead, 0, bytesRead));
                }
                catch
                {
                    networkStream.Close();
                    client.Close();
                    Console.WriteLine("Failed to receive message");
                    break;
                }
            }
        }
        public void RunUser()
        {
            ConnectToNetwork(client);
            Thread CheckingForMessages = new Thread(new ThreadStart(ReceiveMessage));
            Thread SendingMessages = new Thread(new ThreadStart(SendMessage));
            CheckingForMessages.Start();
            SendingMessages.Start();

            
        }
    }
}
