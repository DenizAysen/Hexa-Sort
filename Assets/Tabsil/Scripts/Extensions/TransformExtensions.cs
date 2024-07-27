using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{    
    /// <summary>
    /// Destroys all of the transform's children
    /// </summary>
    /// <param name="transform">The parent</param>
    public static void Clear(this Transform transform)
    {
        while (transform.childCount > 0)
        {
            Transform child = transform.transform.GetChild(0);
            child.SetParent(null);
            Object.DestroyImmediate(child.gameObject);
        }
    }
}
