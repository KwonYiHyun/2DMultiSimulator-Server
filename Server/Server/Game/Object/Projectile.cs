﻿using System;
using System.Collections.Generic;
using System.Text;

public class Projectile : GameObject
{
    public Projectile()
    {
        ObjectType = GameObjectType.Projectile;
    }

    public virtual void Update()
    {

    }

    public virtual bool isHit(GameObject player, GameObject projectile)
    {
        return false;
    }
}