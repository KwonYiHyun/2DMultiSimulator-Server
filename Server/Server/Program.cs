using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static Listener listener = new Listener();
    static List<System.Timers.Timer> _timers = new List<System.Timers.Timer>();

    public static GameRoomManager roomManager = new GameRoomManager();

    static async Task Main(string[] args)
    {
        /*
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);
        */

        IPAddress ipA = IPAddress.Parse("127.0.0.1");
        IPEndPoint endPoint = new IPEndPoint(ipA, 8877);

        Console.WriteLine($"OS : {Environment.OSVersion} \nEndPoint : {endPoint}");

        listener.Init(endPoint, 50);
        await listener.StartAsync();

        while (true)
        {
            Console.WriteLine("Main Thread");
            Thread.Sleep(1000);
        }
    }
}