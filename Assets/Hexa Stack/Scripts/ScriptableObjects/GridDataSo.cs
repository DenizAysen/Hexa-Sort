using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Grid/Data", fileName = "New Grid Data")]
public class GridDataSo : ScriptableObject
{
    public GameObject Hexagon;
    public int GridSize;
}
