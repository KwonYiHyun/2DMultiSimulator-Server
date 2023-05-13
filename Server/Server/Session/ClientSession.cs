using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Core;

public class ClientSession : Session
{
    public Player MyPlayer { get; set; }

    public int SessionId { get; set; }

    public GameRoom Room;

    public override async void OnConnected()
    {
        Console.WriteLine("OnConnected");
        Room = GameRoomManager.Instance.gameRoom;

        MyPlayer = ObjectManager.Instance.Add<Player>();
        MyPlayer.Position = new Vector(0, 0);

        MyPlayer.Session = this;

        S_Connect connectPkt = new S_Connect();
        connectPkt.id = MyPlayer.Id;
        await MyPlayer.Session.SendAsync(connectPkt.Serialize());
    }

    public override void OnDisconnected()
    {
        if (MyPlayer.Room != null)
            MyPlayer.Room.Push(MyPlayer.Room.Leave, MyPlayer.Id);
    }

    public override Task<int> ReceiveAsync(byte[] headerBuffer, SocketFlags flags = SocketFlags.None)
    {
        return socket.ReceiveAsync(headerBuffer, flags);
    }

    public override Task<int> ReceiveAsync(ArraySegment<byte> buffer, SocketFlags flags = SocketFlags.None)
    {
        return socket.ReceiveAsync(buffer, flags);
    }

    public override Task<int> SendAsync(byte[] headerBuffer, SocketFlags flags = SocketFlags.None)
    {
        try
        {
            return socket.SendAsync(headerBuffer, flags);
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public override Task<int> SendAsync(ArraySegment<byte> buffer, SocketFlags flags = SocketFlags.None)
    {
        return socket.SendAsync(buffer, flags);
    }

    public override Task SendAsync(List<ArraySegment<byte>> buffer)
    {
        if (buffer.Count == 0)
            return Task.CompletedTask;

        return socket.SendAsync(buffer, SocketFlags.None);
    }
}