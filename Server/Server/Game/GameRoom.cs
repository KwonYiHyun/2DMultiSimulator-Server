using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core;


public class GameRoom : JobSerializer
{
    public int RoomId { get; set; }
    public GameRoomManager roomManager { get; set; }

    public System.Timers.Timer timer;

    Dictionary<int, Player> _players = new Dictionary<int, Player>();
    Dictionary<int, Projectile> _projectiles = new Dictionary<int, Projectile>();


    public void Init(int mapId)
    {
        
    }

    public void Update()
    {
        foreach (Projectile projectile in _projectiles.Values)
        {
            projectile.Update();
        }

        foreach (Player player in _players.Values)
        {
            player.MoveUpdate();
        }

        Flush();
    }

    public virtual async void Enter(GameObject gameObject)
    {
        if (gameObject == null)
            return;

        GameObjectType type = ObjectManager.GetObjectTypeById(gameObject.Id);

        if (type == GameObjectType.Player)
        {
            Player player = gameObject as Player;
            _players.TryAdd(gameObject.Id, player);
            player.Room = this;

            // TODO : 본인에게 정보 전송
            {
                S_EnterGame enterPacket = new S_EnterGame();
                enterPacket.objectInfo = player.Info;
                await player.Session.SendAsync(enterPacket.Serialize());

                S_Spawn spawnPacket = new S_Spawn();

                foreach (Player p in _players.Values)
                {
                    if (player != p)
                    {
                        spawnPacket.objectInfoList.Add(p.Info);
                    }
                }
                if (spawnPacket.objectInfoList.Count > 0)
                {
                    await player.Session.SendAsync(spawnPacket.Serialize());
                }
            }
        }
        else if (type == GameObjectType.Projectile)
        {
            Projectile projectile = gameObject as Projectile;
            _projectiles.Add(gameObject.Id, projectile);
            projectile.Room = this;
        }

        // TODO : 타인에게 정보 전송
        {
            S_Spawn spawnPacket = new S_Spawn();
            spawnPacket.objectInfoList.Add(gameObject.Info);

            foreach (Player p in _players.Values)
            {
                if (p.Id != gameObject.Id)
                {
                    await p.Session.SendAsync(spawnPacket.Serialize());
                }
            }
        }
    }

    public async virtual void Leave(int objectId)
    {
        GameObjectType type = ObjectManager.GetObjectTypeById(objectId);

        if (type == GameObjectType.Player)
        {
            Player player = null;
            if (_players.Remove(objectId, out player) == false)
                return;

            // TODO : 맵에서 플레이어 정보 삭제
            {

            }
            player.Room = null;

            // TODO : 본인에게 정보 전송
            {
                S_LeaveGame leavePacket = new S_LeaveGame();
                await player.Session.SendAsync(leavePacket.Serialize());
            }
            // Player가 나갔을 때 남은인원 0명이명 방 폭파
            if (_players.Count == 0)
            {
                // GameRoomManager.Instance.Remove(RoomId);
            }
        }
        else if (type == GameObjectType.Projectile)
        {
            Projectile projectile = null;
            if (_projectiles.Remove(objectId, out projectile) == false)
                return;

            projectile.Room = null;
        }

        // TODO : 타인에게 정보 전송
        {
            S_Despawn despawnPacket = new S_Despawn();
            despawnPacket.objectIdList.Add(objectId);
            foreach (Player p in _players.Values)
            {
                if (p.Id != objectId)
                {
                    await p.Session.SendAsync(despawnPacket.Serialize());
                }
            }
        }
    }

    public void HandleMove(Player player, C_Move movePacket)
    {
        PositionInfo moveInfo = movePacket.positionInfo;

        player.Info.positionInfo = moveInfo;
    }

    public void HandleSkill(Player player)
    {

    }

    public async void Broadcast(IPacket packet)
    {
        try
        {
            foreach (Player p in _players.Values)
            {
                await p.Session.SendAsync(packet.Serialize());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

    }

    public void TickRoom(int tick = 100)
    {
        timer = new System.Timers.Timer();
        timer.Interval = tick;
        timer.Elapsed += ((s, e) => { this.Update(); });
        timer.AutoReset = true;
        timer.Enabled = true;
        roomManager._timers.Add(timer);
    }
}