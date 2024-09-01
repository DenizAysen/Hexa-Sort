using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HexStack : MonoBehaviour
{
    public List<Hexagon> Hexagons {  get; private set; }
    public void Initiliaze()
    {
        for (int i = 0; i < transform.childCount; i++)
            Add(transform.GetChild(i).GetComponent<Hexagon>());

        Place();
    }
    public void Add(Hexagon hexagonInstance)
    {
        if (Hexagons == null) 
            Hexagons = new List<Hexagon>();

        Hexagons.Add(hexagonInstance);
        hexagonInstance.SetParent(transform);
    }
    public void Place()
    {
        foreach (Hexagon hexagon in Hexagons)
        {
            hexagon.DisableCollider();
        }
    }
    public Color GetTopHexagonColor() => Hexagons[^1].Color;
    public bool Contains(Hexagon hexagon) => Hexagons.Contains(hexagon);
    public void Remove(Hexagon hexagon)
    {
        Hexagons.Remove(hexagon);

        if (Hexagons.Count <= 0)
            DestroyImmediate(gameObject);
    }
}
