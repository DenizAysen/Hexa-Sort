using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour
{
    [SerializeField] private LayerMask hexagonLayerMask;
    [SerializeField] private LayerMask gridHexagonLayerMask;
    [SerializeField] private LayerMask groundLayerMask;
    private HexStack currentHexStack;
    private Vector3 currentStackInitialPos;

    private Camera mainCam;
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ManageControl();
    }

    private void ManageControl()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ManageMouseDown();
        }
        else if (Input.GetMouseButton(0) && currentHexStack != null) 
        {
            ManageMouseDrag();
        }
        else if (Input.GetMouseButtonUp(0) && currentHexStack != null) 
        {
            ManageMouseUp();
        }
    }

    private void ManageMouseUp()
    {
        
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
            DraggingAboveGridCell();
    }

    private void DraggingAboveGridCell()
    {
       
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
        currentHexStack.transform.position = Vector3.MoveTowards(currentHexStack.transform.position,
            currentStackTargetPos, 
            Time.deltaTime * 30f);
    }

    private void ManageMouseDown()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500,hexagonLayerMask);

        if (hit.collider == null)
            return;

        Debug.Log(hit.collider.gameObject.name);

        currentHexStack = hit.collider.GetComponent<Hexagon>().HexStack;
        currentStackInitialPos = currentHexStack.transform.position;
    }
    private Ray GetClickedRay() => mainCam.ScreenPointToRay(Input.mousePosition);
}
