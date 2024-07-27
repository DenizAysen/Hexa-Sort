#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class MenuItems
{
    [MenuItem("Tools/Organize Hierarchy")]
    static void OrganizeHierarchy()
    {
        new GameObject("--- ENVIRONEMENT ---");
        new GameObject(" ");

        new GameObject("--- GAMEPLAY ---");
        new GameObject(" ");

        new GameObject("--- UI ---");
        new GameObject(" ");

        new GameObject("--- MANAGERS ---");
    }

    [MenuItem("Tools/Reload Scene")]
    static void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}

#endif