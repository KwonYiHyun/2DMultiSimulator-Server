﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using Core;

public class Listener
{
    private Socket socket;

    public void Init(IPEndPoint endPoint, int backlog)
    {
        Console.WriteLine("Server Init");
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(endPoint);
        socket.Listen(backlog);
        socket.NoDelay = true;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            try
            {
                Socket clientSocket = await socket.AcceptAsync();
                ClientSession session = SessionManager.Instance.Generate(clientSocket);
                session.OnConnected();
                ThreadPool.QueueUserWorkItem(RunAsync, session);
            }
            catch (SocketException e)
            {
                Console.WriteLine("====================================\n" +
                                    "======AcceptAsync Socket Error======\n" +
                                    "====================================\n" + e + "\n" +
                                    "====================================");
            }
        }
    }

    public async void RunAsync(object sender)
    {
        ClientSession session = sender as ClientSession;
        byte[] headerBuffer = new byte[2];

        try
        {
            while (true)
            {
                var recvTask = session.ReceiveAsync(headerBuffer);
                var timeoutTask = Task.Delay(1000 * 200);

                var result = await Task.WhenAny(recvTask, timeoutTask);

                if (result == timeoutTask)
                {
                    Console.WriteLine("client disconnect");
                    // TODO 소켓 dispose, 자원해제
                    session.Disconnect();
                    return;
                }

                int recvLen = await recvTask;

                if (recvLen < 1)
                {
                    Console.WriteLine("client disconnect");
                    // TODO 소켓 dispose, 자원해제
                    session.Disconnect();
                    return;
                }
                else if (recvLen == 1)
                {
                    await session.ReceiveAsync(new ArraySegment<byte>(headerBuffer, 1, 1));
                }

                short dataSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(headerBuffer));
                byte[] dataBuffer = new byte[dataSize];

                int totalRecv = 0;
                while (totalRecv < dataSize)
                {
                    int n2 = await session.ReceiveAsync(new ArraySegment<byte>(dataBuffer, totalRecv, dataSize - totalRecv), SocketFlags.None);
                    totalRecv += n2;
                }

                PacketType type = (PacketType)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(dataBuffer));
                IPacket pkt = null;

                Func<ClientSession, byte[], IPacket> func;

                if (PacketManager.packetTypes.TryGetValue(type, out func))
                {
                    pkt = func.Invoke(session, dataBuffer);
                }
                else
                {
                    Console.WriteLine("Listener.RunAsync (ERROR) [Type에 맞는 패킷이 없습니다.]");
                }

                Action<ClientSession, IPacket> action;
                if (PacketManager.action.TryGetValue(type, out action))
                {
                    action?.Invoke(session, pkt);
                }
                else
                {
                    Console.WriteLine("Listener.RunAsync (ERROR) [Type에 맞는 Action이 없습니다.]");
                }
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("==========================================\n" +
                                "======RunAsync SocketException Error======\n" +
                                "==========================================\n" + e + "\n" +
                                "==========================================");
        }catch (Exception e)
        {
            Console.WriteLine("====================================\n" +
                                "======RunAsync Exception Error======\n" +
                                "====================================\n" + e + "\n" +
                                "====================================");
        }
    }
}