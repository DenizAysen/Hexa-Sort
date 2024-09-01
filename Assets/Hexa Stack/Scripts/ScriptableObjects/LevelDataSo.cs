using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Level/Data", fileName = "New Level Data")]
public class LevelDataSo : ScriptableObject
{
   public LevelData LevelData;
}

[Serializable]
public class LevelData
{
    public int RequiredHexagons;
}
