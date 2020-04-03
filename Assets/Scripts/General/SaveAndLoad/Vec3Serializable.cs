using UnityEngine;

[System.Serializable]
public class Vec3Serializable
{
    public float x;
    public float y;
    public float z;

    public Vec3Serializable(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vec3Serializable(Vector3 vector3)
    {
        x = vector3.x;
        y = vector3.y;
        z = vector3.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public static Vec3Serializable ToVec3Serializable(Vector3 vector3)
    {
        return new Vec3Serializable(vector3);
    }
}
