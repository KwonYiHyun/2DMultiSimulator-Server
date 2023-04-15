using System;

public class Vector
{
    public float x;
    public float z;

    public Vector()
    {
        x = 0;
        z = 0;
    }

    public Vector(float x, float z)
    {
        this.x = x;
        this.z = z;
    }

    public Vector(PositionInfo pos)
    {
        x = pos.posX;
        z = pos.posZ;
    }

    public double Magnitude()
    {
        return Math.Sqrt(x * x + z * z);
    }

    public Vector Normalize()
    {
        double magnitude = Magnitude();
        if (magnitude == 0)
        {
            return new Vector(0, 0);
        }
        return new Vector((float)(x / magnitude), (float)(z / magnitude));
    }

    public static Vector operator -(Vector v1, Vector v2)
    {
        return new Vector(v1.x - v2.x, v1.z - v2.z);
    }

    public static Vector operator +(Vector v1, Vector v2)
    {
        return new Vector(v1.x + v2.x, v1.z + v2.z);
    }

    public static Vector operator *(Vector v1, float f1)
    {
        return new Vector((float)Math.Round((double)v1.x * f1), (float)Math.Round((double)v1.z * f1));
    }

    public static Vector operator /(Vector v1, float f1)
    {
        return new Vector((float)Math.Round((double)v1.x / f1), (float)Math.Round((double)v1.z / f1));
    }

    public static bool operator ==(Vector v1, Vector v2)
    {
        return (v1.x == v2.x && v1.z == v2.z);
    }

    public static bool operator !=(Vector v1, Vector v2)
    {
        return (v1.x != v2.x || v1.z != v2.z);
    }
}