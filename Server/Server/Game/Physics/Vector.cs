using System;

public class Vector
{
    public float x;
    public float y;

    public Vector()
    {
        x = 0;
        y = 0;
    }

    public Vector(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector(PositionInfo pos)
    {
        x = pos.posX;
        y = pos.posY;
    }

    public double Magnitude()
    {
        return Math.Sqrt(x * x + y * y);
    }

    public Vector Normalize()
    {
        double magnitude = Magnitude();
        if (magnitude == 0)
        {
            return new Vector(0, 0);
        }
        return new Vector((float)(x / magnitude), (float)(y / magnitude));
    }

    public static Vector operator -(Vector v1, Vector v2)
    {
        return new Vector(v1.x - v2.x, v1.y - v2.y);
    }

    public static Vector operator +(Vector v1, Vector v2)
    {
        return new Vector(v1.x + v2.x, v1.y + v2.y);
    }

    public static Vector operator *(Vector v1, float f1)
    {
        return new Vector((float)Math.Round((double)v1.x * f1), (float)Math.Round((double)v1.y * f1));
    }

    public static Vector operator /(Vector v1, float f1)
    {
        return new Vector((float)Math.Round((double)v1.x / f1), (float)Math.Round((double)v1.y / f1));
    }

    public static bool operator ==(Vector v1, Vector v2)
    {
        return (v1.x == v2.x && v1.y == v2.y);
    }

    public static bool operator !=(Vector v1, Vector v2)
    {
        return (v1.x != v2.x || v1.y != v2.y);
    }
}