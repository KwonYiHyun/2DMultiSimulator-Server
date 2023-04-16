using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core;

public class PacketHandler
{
    public void C_ConnectAction(ClientSession session, IPacket packet)
    {

    }

    public void C_StartGameAction(ClientSession session, IPacket packet)
    {
        GameRoom Room = session.Room;

        Room.Push(Room.Enter, session.MyPlayer);
    }

    public void C_EnterGameAction(ClientSession session, IPacket packet)
    {

    }

    public void C_MoveAction(ClientSession session, IPacket packet)
    {
        C_Move movePacket = packet as C_Move;

        Player player = session.MyPlayer;
        if (player == null)
            return;

        GameRoom room = player.Room;
        if (room == null)
            return;

        player.movementSpeed = movePacket.speed;

        room.Push(room.HandleMove, player, movePacket);

        S_Move mPkt = new S_Move();
        mPkt.objectId = player.objectInfo.objectId;
        mPkt.positionInfo = movePacket.positionInfo;
        room.Push(room.Broadcast, mPkt);

        Console.WriteLine($"[move] {session.MyPlayer.Id} {movePacket.positionInfo.posX} / {movePacket.positionInfo.posY} speed = {movePacket.speed}");
    }

    public void C_HitAction(ClientSession session, IPacket packet)
    {

    }
}