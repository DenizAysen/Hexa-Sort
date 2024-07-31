using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        StackController.onStackPlaced += OnStackPlaced;
    }
    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void UnsubscribeEvents()
    {
        StackController.onStackPlaced -= OnStackPlaced;
    }
    private void OnStackPlaced(GridCell gridCell)
    {
        LayerMask gridCellMask = 1<< gridCell.gameObject.layer;

        List<GridCell> neighborGridCells = new List<GridCell>();

        Collider[] neighborGridCellColliders = Physics.OverlapSphere(gridCell.transform.position, 2, gridCellMask);

        foreach (Collider gridCollider in neighborGridCellColliders)
        {
            GridCell neighborGridCell = gridCollider.GetComponent<GridCell>();

            if (!neighborGridCell.IsOccupied)
                continue;

            if (neighborGridCell == gridCell)
                continue;

            neighborGridCells.Add(neighborGridCell);
        }

        if (neighborGridCells.Count <= 0)
        {
            Debug.Log("No neighbors");
            return;
        }

        Color gridCellTopHexagonColor = gridCell.Stack.GetTopHexagonColor();

        List<GridCell> similarNeighborGridCells = new List<GridCell>();

        foreach (GridCell neighborGridCell in neighborGridCells)
        {
            Color neighborGridCellTopColor = neighborGridCell.Stack.GetTopHexagonColor();

            if(gridCellTopHexagonColor  == neighborGridCellTopColor)
                similarNeighborGridCells.Add(neighborGridCell);

        }

        if(similarNeighborGridCells.Count <= 0)
        {
            Debug.Log("No similar neighbors");
        }

        List<Hexagon> hexagonsToAdd = new List<Hexagon>();

        foreach (GridCell neighborGridCell in similarNeighborGridCells)
        {
            HexStack neighborCellHexstack = neighborGridCell.Stack;

            for (int i = neighborCellHexstack.Hexagons.Count -1; i >= 0; i--)
            {
                Hexagon hexagon = neighborCellHexstack.Hexagons[i];

                if (hexagon.Color != gridCellTopHexagonColor)
                    break;

                hexagonsToAdd.Add(hexagon);
                hexagon.SetParent(null);
            }
        }

        foreach (GridCell neighborCell in similarNeighborGridCells)
        {
            HexStack stack = neighborCell.Stack;
            foreach (Hexagon hexagon in hexagonsToAdd)
            {
                if (stack.Contains(hexagon))
                    stack.Remove(hexagon);
            }
        }

        float initialY = gridCell.Stack.Hexagons.Count* .2f;

        for (int i = 0; i < hexagonsToAdd.Count; i++)
        {
            Hexagon hexagon = hexagonsToAdd[i];

            float targetY = initialY + i*.2f;

            Vector3 targetLocalPosition = Vector3.up * targetY;

            gridCell.Stack.Add(hexagon);
            hexagon.transform.localPosition = targetLocalPosition;
        }
    }
    
}
