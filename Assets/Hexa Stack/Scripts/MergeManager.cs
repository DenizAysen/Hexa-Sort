using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    private List<GridCell> updatedGridCells = new List<GridCell>();
    public static Action<int> onStackCompleted;
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
        StartCoroutine(StackPlacedRoutine(gridCell));     
    }
    private IEnumerator StackPlacedRoutine(GridCell gridCell)
    {
        updatedGridCells.Add(gridCell);

        while (updatedGridCells.Count > 0)
            yield return CheckForMerge(updatedGridCells[0]);
    }
    private IEnumerator CheckForMerge(GridCell gridCell)
    {
        updatedGridCells.Remove(gridCell);

        if(!gridCell.IsOccupied)
            yield break;

        List<GridCell> neighborGridCells = GetNeighborGridCells(gridCell);

        if (neighborGridCells.Count <= 0)
        {
            Debug.Log("No neighbors");
            yield break;
        }

        Color gridCellTopHexagonColor = gridCell.Stack.GetTopHexagonColor();

        List<GridCell> similarNeighborGridCells = GetSimilarNeighborGridCells(gridCellTopHexagonColor, neighborGridCells.ToArray());

        if (similarNeighborGridCells.Count <= 0)
        {
            yield break;
        }

        updatedGridCells.AddRange(similarNeighborGridCells);

        List<Hexagon> hexagonsToAdd = GetHexagonsToAdd(gridCellTopHexagonColor, similarNeighborGridCells.ToArray());

        RemoveHexagonsFromStack(similarNeighborGridCells.ToArray(), hexagonsToAdd.ToArray());

        MoveHexagons(gridCell, hexagonsToAdd);

        yield return new WaitForSeconds(.2f +(hexagonsToAdd.Count + 1)* .01f);

        yield return CheckForCompleteStack(gridCell, gridCellTopHexagonColor);
    }
    private List<GridCell> GetNeighborGridCells(GridCell gridCell)
    {
        LayerMask gridCellMask = 1 << gridCell.gameObject.layer;

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

        return neighborGridCells;
    }
    private List<GridCell> GetSimilarNeighborGridCells(Color gridCellTopHexagonColor, GridCell[] neighborGridCells)
    {
        List<GridCell> similarNeighborGridCells =  new List<GridCell>();

        foreach (GridCell neighborGridCell in neighborGridCells)
        {
            Color neighborGridCellTopColor = neighborGridCell.Stack.GetTopHexagonColor();

            if (gridCellTopHexagonColor == neighborGridCellTopColor)
                similarNeighborGridCells.Add(neighborGridCell);

        }

        return similarNeighborGridCells;
    } 
    private List<Hexagon> GetHexagonsToAdd(Color gridCellTopHexagonColor, GridCell[] similarNeighborGridCells)
    {
        List<Hexagon> hexagonsToAdd = new List<Hexagon>();

        foreach (GridCell neighborGridCell in similarNeighborGridCells)
        {
            HexStack neighborCellHexstack = neighborGridCell.Stack;

            for (int i = neighborCellHexstack.Hexagons.Count - 1; i >= 0; i--)
            {
                Hexagon hexagon = neighborCellHexstack.Hexagons[i];

                if (hexagon.Color != gridCellTopHexagonColor)
                    break;

                hexagonsToAdd.Add(hexagon);
                hexagon.SetParent(null);
            }
        }

        return hexagonsToAdd;
    }
    private void RemoveHexagonsFromStack(GridCell[] similarNeighborGridCells, Hexagon[] hexagonsToAdd)
    {
        foreach (GridCell neighborCell in similarNeighborGridCells)
        {
            HexStack stack = neighborCell.Stack;
            foreach (Hexagon hexagon in hexagonsToAdd)
            {
                if (stack.Contains(hexagon))
                    stack.Remove(hexagon);
            }
        }
    }
    private void MoveHexagons(GridCell gridCell, List<Hexagon> hexagonsToAdd)
    {
        float initialY = gridCell.Stack.Hexagons.Count * .2f;

        for (int i = 0; i < hexagonsToAdd.Count; i++)
        {
            Hexagon hexagon = hexagonsToAdd[i];

            float targetY = initialY + i * .2f;

            Vector3 targetLocalPosition = Vector3.up * targetY;

            gridCell.Stack.Add(hexagon);
            hexagon.MoveToLocal(targetLocalPosition);
            //hexagon.transform.localPosition = targetLocalPosition;
        }
    }
    private IEnumerator CheckForCompleteStack(GridCell gridCell, Color topColor)
    {
        if (gridCell.Stack.Hexagons.Count < 10)
            yield break;

        int collectedHexagons = gridCell.Stack.Hexagons.Count;

        Hexagon hex;
        List<Hexagon> similarHexagons = new List<Hexagon>();
        for (int i = gridCell.Stack.Hexagons.Count -1; i >= 0; i--)
        {
            hex = gridCell.Stack.Hexagons[i];
            if (hex.Color != topColor)
                break;

            similarHexagons.Add(hex);
        }

        int similarHexagonCount = similarHexagons.Count;

        if (similarHexagons.Count < 10)
            yield break;

        float delay = 0f;

        while (similarHexagons.Count > 0)
        {
            similarHexagons[0].SetParent(null);
            gridCell.Stack.Remove(similarHexagons[0]);
            similarHexagons[0].Vanish(delay);
            delay += .01f;
            similarHexagons.RemoveAt(0);
        }

        updatedGridCells.Add(gridCell);

        yield return new WaitForSeconds(.2f + (similarHexagonCount + 1f) * .01f);
        onStackCompleted?.Invoke(collectedHexagons);
    }
}
