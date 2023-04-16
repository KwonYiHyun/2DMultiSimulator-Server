using System;
using System.Collections.Generic;
using System.Text;

public class GameObject
{
    public GameObjectType ObjectType { get; protected set; }

    public int Id
    {
        get { return objectInfo.objectId; }
        set { objectInfo.objectId = value; }
    }

    public float radius = 0.25f;

    public GameRoom Room { get; set; }

    public ObjectInfo objectInfo { get; set; } = new ObjectInfo();

    public PositionInfo PosInfo { get; private set; } = new PositionInfo();

    public GameObject()
    {
        objectInfo.positionInfo = PosInfo;
    }
}