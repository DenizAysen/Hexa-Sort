using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexStack : MonoBehaviour
{
   public List<Hexagon> Hexagons {  get; private set; }

    public void Add(Hexagon hexagonInstance)
    {
        if (Hexagons == null) 
            Hexagons = new List<Hexagon>();

        Hexagons.Add(hexagonInstance);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
