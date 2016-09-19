using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Regexpire
{
    class Program
    {
        static void Main(string[] args)
        {
            var sender = DoConnect();
            GetResponse(sender); // Can you match these regexes?
            System.Threading.Thread.Sleep(500);

            while (true) // when is the end??? doesnt matter
            {
                var response = GetResponse(sender);                
                var pattern = response.Substring(0, response.Length - 1); // remove end line
                var match = FindMatch(pattern);
                SendAnswer(match, sender);
            }            
        }

        private static Socket DoConnect()
        {
            var ipHostInfo = Dns.Resolve("misc.chal.csaw.io");
            var ipAddress = ipHostInfo.AddressList[0];
            var remoteEP = new IPEndPoint(ipAddress, 8001);
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

        private static void SendAnswer(string answer, Socket sender)
        {
            byte[] msg = Encoding.ASCII.GetBytes(answer + "\n");
            int bytesSent = sender.Send(msg);
            Console.WriteLine(answer);
        }

        static string FindMatch(string pattern)
        {
            pattern = pattern.Replace("\\W", "$");
            pattern = pattern.Replace("\\w", "a");
            pattern = pattern.Replace("\\d", "1");
            pattern = pattern.Replace("\\D", "a");

            var match = "";
            int i = 0;
            
            while(i < pattern.Length)
            {
                var charAtI = pattern[i];

                if (charAtI.Equals('*') || charAtI.Equals('+'))
                {
                    i++;
                    continue;
                }

                if (!charAtI.Equals('(') && !charAtI.Equals('['))
                {
                    var repeat = FindRepeater(pattern, i);

                    if (repeat == -1)
                    {
                        match += charAtI;
                    }
                    else
                    {
                        for (var j = 0; j < repeat; j++)
                        {
                            match += charAtI;
                        }
                    }

                    i++;
                    if (repeat != -1)
                    {
                        i += repeat.ToString().Length + 2;
                    }
                    continue;
                }

                if (charAtI.Equals('('))
                {
                    var endPos = pattern.IndexOf(')', i);
                    var group = pattern.Substring(i + 1, endPos - i);
                    var first = Regex.Split(group, "\\|")[0];
                    var repeat = FindRepeater(pattern, endPos);
                    if(repeat == -1)
                    {
                        match += first;
                    }
                    else
                    {
                        for(var j = 0; j < repeat; j++)
                        {
                            match += first;
                        }
                    }

                    i = endPos + 1;
                    if(repeat != -1)
                    {
                        i += repeat.ToString().Length + 2;
                    }
                    continue;
                }

                if (charAtI.Equals('['))
                {
                    var endPos = pattern.IndexOf(']', i);
                    var group = pattern.Substring(i + 1, endPos - i);
                    var first = group[0];

                    var repeat = FindRepeater(pattern, endPos);
                    if (repeat == -1)
                    {
                        match += first;
                    }
                    else
                    {
                        for (var j = 0; j < repeat; j++)
                        {
                            match += first;
                        }
                    }

                    i = endPos + 1;
                    if (repeat != -1)
                    {
                        i += repeat.ToString().Length + 2;
                    }
                    continue;
                }

                //throw new Exception();
            }

            //var regex = new Regex(pattern);
            //if (!regex.IsMatch(match))
            //{
            //    throw new Exception();
            //}

            return match;
        }

        private static int FindRepeater(string str, int pos)
        {
            if(pos == str.Length - 1)
            {
                return -1;
            }

            var substr = str.Substring(pos + 1);

            if(substr[0] == '{')
            {
                var end = substr.IndexOf('}');
                var repstr = substr.Substring(1, end - 1);
                return int.Parse(repstr);
            }
            else
            {
                return -1; // no repeat
            }            
        }
        
    }
}
