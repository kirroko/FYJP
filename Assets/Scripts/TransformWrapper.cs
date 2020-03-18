using UnityEngine;

public class TransformWrapper 
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 localScale;

    public static bool operator ==(TransformWrapper lhs, TransformWrapper rhs)
    {
        return lhs.position == rhs.position && lhs.rotation == rhs.rotation && lhs.localScale == rhs.localScale;
    }

    public static bool operator !=(TransformWrapper lhs, TransformWrapper rhs)
    {
        return lhs.position != rhs.position || lhs.rotation != rhs.rotation || lhs.localScale != rhs.localScale;
    }

    public static bool operator ==(TransformWrapper lhs, Transform rhs)
    {
        return lhs.position == rhs.position && lhs.rotation == rhs.rotation && lhs.localScale == rhs.localScale;
    }

    public static bool operator !=(TransformWrapper lhs, Transform rhs)
    {
        return lhs.position != rhs.position || lhs.rotation != rhs.rotation || lhs.localScale != rhs.localScale;
    }
}
