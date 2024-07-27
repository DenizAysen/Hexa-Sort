using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtilities 
{
    public static Vector3 GetCenter(Vector3 v0, Vector3 v1)
    {
        return (v0 + v1) / 2;
    }

    public static Vector3 GetCenterOfMass(params Vector3[] vectors)
    {
        Vector3 center = Vector3.zero;

        foreach (Vector3 v in vectors)
            center += v;

        return center / vectors.Length;
    }
}
