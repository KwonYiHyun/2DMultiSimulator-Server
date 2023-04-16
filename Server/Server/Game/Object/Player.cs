using System;
using System.Collections.Generic;
using System.Text;

public class Player : GameObject
{
    public ClientSession Session { get; set; }

    public Vector Position { get; set; }

    public Vector targetPosition { get; set; }

    public float movementSpeed = 1.0f;

    long _nextMoveTick = 0;

    public Player()
    {
        ObjectType = GameObjectType.Player;

        Position = new Vector(0, 0);
        targetPosition = new Vector(0, 0);
    }

    public void MoveUpdate()
    {
        if (Room == null)
            return;

        if (_nextMoveTick >= Environment.TickCount64)
            return;

        _nextMoveTick = Environment.TickCount64 + 100;

        // TODO 백터 정규화 전에 현재위치와 비교하여 1보다 작은지 확인
        if (Position == targetPosition) return;
        Vector diff = Position - targetPosition;
        if (MathF.Abs(diff.x) <= 0.3 && MathF.Abs(diff.y) <= 0.3) return;

        Vector normalize = targetPosition - Position;
        normalize = normalize.Normalize();

        Vector targetVector = Position + normalize;

        PositionInfo pos = new PositionInfo();
        pos.posX = (float)Math.Round((double)targetVector.x, 2);
        pos.posY = (float)Math.Round((double)targetVector.y, 2);

        Position = new Vector(pos);

        objectInfo.positionInfo = pos;

        Console.WriteLine("Player Position [x : " + pos.posX + ", y : " + pos.posY + "]");
    }
}