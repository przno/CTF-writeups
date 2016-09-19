using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Coinslot
{
    class Program
    {
        private static Socket DoConnect()
        {
            var ipHostInfo = Dns.Resolve("misc.chal.csaw.io");
            var ipAddress = ipHostInfo.AddressList[0];
            var remoteEP = new IPEndPoint(ipAddress, 8000);
            var sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(remoteEP);

            Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
            Console.WriteLine();

            return sender;
        }

        private static string GetResponse(Socket sender)
        {
            var bytes = new byte[1024];
            int bytesRec = sender.Receive(bytes);
            string rec = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            Console.Write(rec);

            return rec;
        }

        private static decimal GetAmount(string s, bool isFirstTime)
        {
            int pos = isFirstTime ? 0 : 1;
            string num = s.Split('\n')[pos].Replace("$", "");
            decimal f = decimal.Parse(num);

            return f;
        }

        private static decimal DoStep(decimal amount, decimal size, Socket sender)
        {
            decimal d = amount / size;

            if (d >= 1)
            {
                int di = (int)d;
                byte[] msg = Encoding.ASCII.GetBytes(di.ToString() + "\n");
                int bytesSent = sender.Send(msg);
                Console.WriteLine(di);
                return amount % size;
            }
            else
            {
                byte[] msg = Encoding.ASCII.GetBytes("0\n");
                int bytesSent = sender.Send(msg);
                Console.WriteLine(0);
                return amount;
            }
        }

        static void Main(string[] args)
        {
            var sender = DoConnect();
            var isFirstTime = true;

            while (true)
            {
                var response = GetResponse(sender);

                if (response.Contains("$10,000 bills"))
                {
                    var f = GetAmount(response, isFirstTime);
                    isFirstTime = false;

                    f = DoStep(f, 10000, sender);
                    GetResponse(sender);

                    f = DoStep(f, 5000, sender);
                    GetResponse(sender);
                    
                    f = DoStep(f, 1000, sender);
                    GetResponse(sender);
                    
                    f = DoStep(f, 500, sender);
                    GetResponse(sender);
                    
                    f = DoStep(f, 100, sender);
                    GetResponse(sender);
                    
                    f = DoStep(f, 50, sender);
                    GetResponse(sender);
                    
                    f = DoStep(f, 20, sender);
                    GetResponse(sender);
                    
                    f = DoStep(f, 10, sender);
                    GetResponse(sender);
                    
                    f = DoStep(f, 5, sender);
                    GetResponse(sender);
                    
                    f = DoStep(f, 1, sender);
                    GetResponse(sender);
                    
                    f = DoStep(f, (decimal)0.5, sender);
                    GetResponse(sender);
                    
                    f = DoStep(f, (decimal)0.25, sender);
                    GetResponse(sender);
                    
                    f = DoStep(f, (decimal)0.10, sender);
                    GetResponse(sender);
                    
                    f = DoStep(f, (decimal)0.05, sender);
                    GetResponse(sender);
                    
                    f = DoStep(f, (decimal)0.01, sender);
                }
                else
                {
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                    Console.ReadKey();
                }
            }
        }
    }
}