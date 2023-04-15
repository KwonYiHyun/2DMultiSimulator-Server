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
        
    }
}