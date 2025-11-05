using System;
using System.Net;
using System.Net.Sockets;

namespace MyApp
{

    internal class Program
    {
        static void Main(string[] args)
        {
            string ntpServer = "0.ch.pool.ntp.org";

            byte[] timeMessage = new byte[48];
            timeMessage[0] = 0x1B; //LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

            IPEndPoint ntpReference = new IPEndPoint(Dns.GetHostAddresses(ntpServer)[0], 123);

            UdpClient client = new UdpClient();
            client.Connect(ntpReference);

            client.Send(timeMessage, timeMessage.Length);

            timeMessage = client.Receive(ref ntpReference);

            //DateTime ntpTime = NtpPacket.ToDateTime(timeMessage);

            ulong intPart = (ulong)timeMessage[40] << 24 | (ulong)timeMessage[41] << 16 | (ulong)timeMessage[42] << 8 | (ulong)timeMessage[43];
            ulong fractPart = (ulong)timeMessage[44] << 24 | (ulong)timeMessage[45] << 16 | (ulong)timeMessage[46] << 8 | (ulong)timeMessage[47];

            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            var networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);


            Console.WriteLine($"Heure actuelle :\n- {networkDateTime.ToString("dddd, dd MMMM yyyy")}\n- {networkDateTime}\n- {networkDateTime.ToString("d")}");

            var timeDiff = DateTime.Now - networkDateTime;
            Console.WriteLine($"Diff avec heure local : {timeDiff}");

            DateTime localTime = networkDateTime.Add(timeDiff);
            Console.WriteLine($"Heure locale : {localTime}");

            DateTime localTimeUtc = localTime.ToUniversalTime();
            Console.WriteLine(localTimeUtc);

            DateTime localTimeFromUtc = localTimeUtc.ToLocalTime();
            Console.WriteLine(localTimeFromUtc);

            DateTime GMTDate = localTime.ToUniversalTime().AddHours(-1);
            Console.WriteLine(GMTDate);

            DateTime localTimeFromGMT = GMTDate.AddHours(2);
            Console.WriteLine(localTimeFromGMT);

            client.Close();
        }
    }
}