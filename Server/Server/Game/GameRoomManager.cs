using Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

public class GameRoomManager
{
    public static GameRoomManager Instance { get; } = new GameRoomManager();

    public GameRoom gameRoom;
    public List<System.Timers.Timer> _timers = new List<System.Timers.Timer>();

    int _roomId = 0;
    
    object _lock = new object();

    public GameRoomManager()
    {
        gameRoom = new GameRoom();
        gameRoom.Init(1);
        gameRoom.roomManager = this;
        gameRoom.TickRoom();
    }

    public GameRoom Generate()
    {
        /*
        lock (_lock)
        {
            int roomId = ++_roomId;

            GameRoom room = new GameRoom();
            room.RoomId = roomId;
            room.roomManager = this;
            room.TickRoom();
            _rooms.Add(roomId, room);

            return room;
        }
        */
        return null;
    }

    public GameRoom Find(int roomId)
    {
        /*
        lock (_lock)
        {
            GameRoom room = null;
            if (_rooms.TryGetValue(roomId, out room))
                return room;

            return null;
        }
        */
        return null;
    }

    public bool Remove(int roomId)
    {
        /*
        lock (_lock)
        {
            GameRoom room = null;
            _rooms.TryGetValue(roomId, out room);
            room.timer.Stop();
            _timers.Remove(room.timer);
            return _rooms.Remove(roomId);
        }
        */
        return false;
    }
}