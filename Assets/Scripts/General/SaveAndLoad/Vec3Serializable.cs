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

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}
