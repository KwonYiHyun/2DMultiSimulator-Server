using System;
using System.Collections.Generic;
using System.Text;

public class Player : GameObject
{
    public ClientSession Session { get; set; }

    public Vector Position { get; set; }

    public Vector targetPosition { get; set; }

    public float movementSpeed = 1.0f;

    public Player()
    {
        ObjectType = GameObjectType.Player;

        Position = new Vector(0, 0);
        targetPosition = new Vector(0, 0);
    }

    public void MoveUpdate()
    {
        targetPosition = new Vector(Info.positionInfo);

        if (Position == targetPosition) return;
        Vector diff = Position - targetPosition;
        if (MathF.Abs(diff.x) <= 1 && MathF.Abs(diff.y) <= 1) return;

        Vector normalize = targetPosition - Position;
        normalize = normalize.Normalize();

        Vector targetVector = Position + normalize * movementSpeed;

        PositionInfo pos = new PositionInfo();
        pos.posX = (float)Math.Round((double)targetVector.x, 1);
        pos.posY = (float)Math.Round((double)targetVector.y, 1);

        S_Move movePacket = new S_Move();
        movePacket.objectId = Info.objectId;
        movePacket.positionInfo = pos;

        Room.Push(Room.Broadcast, movePacket);

        Position = new Vector(pos);
        Console.WriteLine("Player Position [x : " + pos.posX + ", y : " + pos.posY + "]");
    }
}