using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class GridCell : MonoBehaviour
{
    [SerializeField] private Hexagon hexagonPrefab;

    [OnValueChanged("GenerateInitialHexagons")]
    [SerializeField] private Color[] hexagonColors;
    public HexStack Stack { get; private set; }
    public bool IsOccupied {  
        get => Stack != null;
        private set { } }

    public void AssignStack(HexStack stack)
    {
        Stack = stack;
    }
    private void Start()
    {
        if(transform.childCount > 1)
        {
            Stack = transform.GetChild(1).GetComponent<HexStack>();
            Stack.Initiliaze();
        }
    }
    private void GenerateInitialHexagons()
    {
        while(transform.childCount > 1)
        {
            Transform t = transform.GetChild(1);
            t.SetParent(null);
            DestroyImmediate(t.gameObject);
        }

        Stack = new GameObject("Initial Stack").AddComponent<HexStack>();
        Stack.transform.SetParent(transform);
        Stack.transform.localPosition = Vector3.up * .2f;

        for (int i = 0; i < hexagonColors.Length; i++)
        {
            Vector3 spawnPoint = Stack.transform.TransformPoint(Vector3.up * i * .2f);
            Hexagon hexagonInstance = Instantiate(hexagonPrefab, spawnPoint, Quaternion.identity);
            hexagonInstance.Color = hexagonColors[i];

            Stack.Add(hexagonInstance);
        }
    }
}
