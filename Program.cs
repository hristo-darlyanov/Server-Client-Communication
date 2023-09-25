using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide
{
    internal class Program
    {
        static void Main(string[] args)
        {

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress iPAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(iPAddress, 11000);

            Socket listener = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            string message = "";

            try
            {
                while (true)
                {
                    // Receive message.
                    var buffer = new byte[1_024];

                    listener.Bind(remoteEP);
                    listener.Listen(10);

                    Socket handle = listener.Accept();

                    while (true)
                    {
                        message = "";
                        while (true)
                        {
                            int messageSize = handle.Receive(buffer);
                            message += Encoding.ASCII.GetString(buffer, 0, messageSize);

                            if (message.Contains("<EOF>"))
                            {
                                message = message.Replace("<EOF>", "");
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.Clear();
            Console.WriteLine("Goodbye");
            Console.ReadKey(true);
        }
    }
}
