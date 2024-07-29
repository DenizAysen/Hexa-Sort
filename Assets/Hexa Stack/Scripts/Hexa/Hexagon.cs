using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hexagon : MonoBehaviour
{
    [SerializeField] private Renderer renderer;

    public HexStack HexStack {  get; private set; }
    public Color Color 
    {  
        get => renderer.material.color ;
        set => renderer.material.color = value; 
    }
    public void Configure(HexStack hexStack)
    {
        HexStack = hexStack;
    }
}
