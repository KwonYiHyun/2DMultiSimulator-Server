using System;
using System.Collections.Generic;
using System.Text;
using Core;

public static class PacketManager
{
    public static Dictionary<PacketType, Action<ClientSession, IPacket>> action = new Dictionary<PacketType, Action<ClientSession, IPacket>>();
    public static Dictionary<PacketType, Func<ClientSession, byte[], IPacket>> packetTypes = new Dictionary<PacketType, Func<ClientSession, byte[], IPacket>>();
    public static PacketHandler packetHandler = new PacketHandler();

    static PacketManager()
    {
        action.Add(PacketType.C_Connect, packetHandler.C_ConnectAction);
        packetTypes.Add(PacketType.C_Connect, MakePacket<C_Connect>);

		action.Add(PacketType.C_StartGame, packetHandler.C_StartGameAction);
        packetTypes.Add(PacketType.C_StartGame, MakePacket<C_StartGame>);

		action.Add(PacketType.C_EnterGame, packetHandler.C_EnterGameAction);
        packetTypes.Add(PacketType.C_EnterGame, MakePacket<C_EnterGame>);

		action.Add(PacketType.C_Move, packetHandler.C_MoveAction);
        packetTypes.Add(PacketType.C_Move, MakePacket<C_Move>);

		action.Add(PacketType.C_Hit, packetHandler.C_HitAction);
        packetTypes.Add(PacketType.C_Hit, MakePacket<C_Hit>);

		
    }

    static T MakePacket<T>(Session session, byte[] buffer) where T : IPacket, new()
    {
        T pkt = new T();
        pkt.DeSerialize(buffer);
        return pkt;
    }
}