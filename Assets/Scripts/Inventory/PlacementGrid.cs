using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementGrid : MonoBehaviour
{
    bool[,] gridBools;

    List<LineRenderer> lines;

    [SerializeField] Material materialLines;
    [SerializeField] Vector2 offsetGrid;

    // Start is called before the first frame update
    void Start()
    {
        lines = new List<LineRenderer>();

        gridBools = new bool[10, 10];
        for (int i = 0; i < gridBools.GetLength(0); i++) {
            for (int j = 0; j < gridBools.GetLength(1); j++) {
                gridBools[i, j] = false;
            }
        }

        for (int i = 0; i <= gridBools.GetLength(0); i++) {
            GameObject newLine = new GameObject("Line");
            LineRenderer l = newLine.AddComponent<LineRenderer>();
            l.positionCount = 2;
            l.SetPosition(0, new Vector2(0, i * 2) + offsetGrid);
            l.SetPosition(1, new Vector2(gridBools.GetLength(0) * 2, i * 2) + offsetGrid);
            l.transform.SetParent(transform);
            l.sortingOrder = 2;
            l.material = materialLines;
            l.widthMultiplier = 0.05f;
            lines.Add(l);
        }

        for(int j = 0;j <= gridBools.GetLength(1);j++) {
            GameObject newLine = new GameObject("Line");
            LineRenderer l = newLine.AddComponent<LineRenderer>();
            l.positionCount = 2;
            l.SetPosition(0, new Vector2(j * 2, 0) + offsetGrid);
            l.SetPosition(1, new Vector2(j * 2, gridBools.GetLength(0) * 2) + offsetGrid);
            l.transform.SetParent(transform);
            l.sortingOrder = 2;
            l.material = materialLines;
            l.widthMultiplier = 0.05f;
            lines.Add(l);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
