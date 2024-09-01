using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable Objects/Level/Manager" , fileName = "New Level Manager")]
public class LevelManagerSo : ScriptableObject
{
    public LevelDataSo[] LevelDataSos;

    public LevelDataSo GetCurrentLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("Level");
        return LevelDataSos[currentLevel%2]; 
    }
}
