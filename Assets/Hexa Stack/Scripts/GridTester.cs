using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class GridTester : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [OnValueChanged("UpdateGridPos")]
    [SerializeField] private Vector3Int gridPos;
    private void UpdateGridPos() => transform.position = grid.CellToWorld(gridPos);
}
