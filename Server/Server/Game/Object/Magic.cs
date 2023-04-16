using System;
using System.Collections.Generic;
using System.Text;

public class Magic : Projectile
{
    public GameObject Owner { get; set; }
    
    long _nextMoveTick = 0;

    public override void Update()
    {
        /*
        if (Owner == null || Room == null)
            return;
        */

        if (_nextMoveTick >= Environment.TickCount64)
            return;

        _nextMoveTick = Environment.TickCount + 100;

        foreach (Player p in Room._players.Values)
        {
            if(isHit(p, this))
            {
                S_Hit hit = new S_Hit();
                hit.objectInfo = p.objectInfo;
                p.Session.SendAsync(hit.Serialize());
            }
        }
    }

    public override bool isHit(GameObject player, GameObject projectile)
    {
        double x_1 = player.objectInfo.positionInfo.posX; // 점 A의 x 좌표
        double y_1 = player.objectInfo.positionInfo.posY; // 점 A의 y 좌표
        double x_2 = projectile.PosInfo.posX; // 점 B의 x 좌표
        double y_2 = projectile.PosInfo.posY; // 점 B의 y 좌표

        double r_1 = player.radius;  // 원 A의 반지름
        double r_2 = projectile.radius;  // 원 B의 반지름

        // A에서 B까지의 거리 계산
        double distance = Math.Sqrt(Math.Pow(x_2 - x_1, 2) + Math.Pow(y_2 - y_1, 2));

        if (distance <= r_1 + r_2)
            return true;
        else
            return false;
    }
}