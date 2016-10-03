using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;


// example:
// Hello, stranger!

//In this task you must solve 100 math questions.
//Every task prints value C, where

//C = A ^ B
//, and you need to return A and B.

//Simple, isn't it?
//C = 287780686812842231154529651696099523125900314252588872220242647268603385115156975836217101154876602680795789047430426560740270070063187307804151321802595771622198460329527084566391828383193273231684840421871834696769724297049578156344171298526363445039857115144907869224462779275185193516140685051842877220756670681043468167076075875952739194309697191673300010095837682596245595957162399423687730503354257713616608264214684740305649833758757222321124088314367794922740132927801211143591705214119168897826229580229686178731378036037312177604025307296216038397342984094725941254130689029452181154044183008884150129735105266718405338898273640519303000749501430250528754892530450015206202558565810317353024096881

namespace MongoliaHellMath
{
    class Program
    {
        private static Socket DoConnect()
        {
            var ipHostInfo = Dns.Resolve("ctf.com.ua");
            var ipAddress = ipHostInfo.AddressList[0];
            var remoteEP = new IPEndPoint(ipAddress, 9988);
            var sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(remoteEP);

            return sender;
        }

        private static string GetNumber(Socket sender)
        {
            var bytes = new byte[10000];
            int bytesRec = sender.Receive(bytes);
            string rec = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            Console.Write(rec);
            
            var regex = new Regex("[0-9].*$");
            var num = regex.Match(rec).Value.Trim();

            return num;
        }

        // C = A ^ B
        // calculate A and B ?!
        // no, just send back C ^ 1
        private static void SendAnswer(string num, Socket sender)
        {
            var answer = num + " 1\n";
            byte[] msg = Encoding.ASCII.GetBytes(answer);
            int bytesSent = sender.Send(msg);
            Console.WriteLine(answer);
        }

        static void Main(string[] args)
        {
            var sender = DoConnect();

            for(int i = 0; i < 100; i++)
            {
                var number = GetNumber(sender);
                SendAnswer(number, sender);                
            }

            // read the flag, no number
            GetNumber(sender);
            Console.ReadKey();
        }
    }
}
