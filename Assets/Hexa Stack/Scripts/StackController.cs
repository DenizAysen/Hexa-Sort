using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class StackController : MonoBehaviour
{
    #region Variables
    [SerializeField] private LayerMask hexagonLayerMask;
    [SerializeField] private LayerMask gridHexagonLayerMask;
    [SerializeField] private LayerMask groundLayerMask;

    private HexStack currentStack;
    private GridCell targetCell;

    private Vector3 currentStackInitialPos;

    private Camera mainCam;
    #endregion

    public static Action<GridCell> onStackPlaced;

    #region Unity Methods
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ManageControl();
    } 
    #endregion

    private void ManageControl()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ManageMouseDown();
        }
        else if (Input.GetMouseButton(0) && currentStack != null) 
        {
            ManageMouseDrag();
        }
        else if (Input.GetMouseButtonUp(0) && currentStack != null) 
        {
            ManageMouseUp();
        }
    }
    private void ManageMouseDown()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, hexagonLayerMask);

        if (hit.collider == null)
            return;

        Debug.Log(hit.collider.gameObject.name);

        currentStack = hit.collider.GetComponent<Hexagon>().HexStack;
        currentStackInitialPos = currentStack.transform.position;
    }
    private void ManageMouseUp()
    {
        if(targetCell == null)
        {
            currentStack.transform.position = currentStackInitialPos;
            currentStack = null;
            return;
        }

        currentStack.transform.position = targetCell.transform.position.With(y: .2f);
        currentStack.transform.SetParent(targetCell.transform);
        currentStack.Place();

        targetCell.AssignStack(currentStack);
        onStackPlaced?.Invoke(targetCell);

        targetCell = null;
        currentStack = null;
    }

    private void ManageMouseDrag()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, gridHexagonLayerMask);

        if (hit.collider == null)
        {
            DraggingAboveGround();
        }
        else
            DraggingAboveGridCell(hit);
    }

    private void DraggingAboveGridCell(RaycastHit hit)
    {
       GridCell gridCell = hit.collider.GetComponent<GridCell>();

        if (gridCell.IsOccupied)
            DraggingAboveGround();
        else
        {
            DraggingAboveNonOcupiedGridCell(gridCell);
        }
    }

    private void DraggingAboveNonOcupiedGridCell(GridCell gridCell)
    {
        Vector3 currentStackTargetPos = gridCell.transform.position.With(y: 2);

        currentStack.transform.position = Vector3.MoveTowards(currentStack.transform.position,
            currentStackTargetPos,
            Time.deltaTime * 30f);

        targetCell = gridCell;
    }

    private void DraggingAboveGround()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, groundLayerMask);

        if(hit.collider == null)
        {
            Debug.LogError("no ground detected");
            return;
        }

        Vector3 currentStackTargetPos = hit.point.With(y: 2);
        currentStack.transform.position = Vector3.MoveTowards(currentStack.transform.position,
            currentStackTargetPos, 
            Time.deltaTime * 30f);

        targetCell = null;
    }

   
    private Ray GetClickedRay() => mainCam.ScreenPointToRay(Input.mousePosition);
}
