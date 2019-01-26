using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class PlacementGrid : MonoBehaviour
{
    bool[,] gridBools;

    List<LineRenderer> linesHorizontal;
    List<LineRenderer> linesVertical;

    [SerializeField] Material materialLines;
    [SerializeField] Vector2 offsetGrid;

    bool gridDisplayed = false;

    [SerializeField]SpriteRenderer ghostObject;

    [SerializeField] GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    [SerializeField] EventSystem m_EventSystem;

    // Start is called before the first frame update
    void Start()
    {
        linesHorizontal = new List<LineRenderer>();
        linesVertical = new List<LineRenderer>();

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
            linesHorizontal.Add(l);
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
            linesVertical.Add(l);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = 20;
        pos = Camera.main.ScreenToWorldPoint(pos);

        int indexX = -1, indexY = -1;
        for (int i = 0; i < linesHorizontal.Count - 1; i++) {
            if (pos.y > linesHorizontal[i].GetPosition(0).y && pos.y <= linesHorizontal[i + 1].GetPosition(0).y) {
                indexY = i;
            }
        }

        for(int i = 0;i < linesVertical.Count - 1;i++) {
            if(pos.x > linesVertical[i].GetPosition(0).x && pos.x <= linesVertical[i + 1].GetPosition(0).x) {
                indexX = i;
            }
        }

        if (indexX < 0 || indexX > linesVertical.Count || indexY < 0 || indexY > linesHorizontal.Count) {
            
            if(gridDisplayed) {
                HideGrid();
                gridDisplayed = false;

                ghostObject.color = new Color(1, 1, 1, 0);
            }
            return;
        }

        if (!gridDisplayed) {
            ShowGrid();
            gridDisplayed = true;
        }

        if (InventoryManager.Instance.selectedObjectForPlacement) {
            if (gridBools[indexX, indexY]) {
                ghostObject.color = new Color(1, 0.1f, 0.1f, 1.0f);
            } else {
                ghostObject.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            ghostObject.sprite = InventoryManager.Instance.selectedObjectForPlacement.pickableObjectData.sprite;
            ghostObject.transform.position = offsetGrid + new Vector2(indexX * 2 + 1, indexY * 2 + 1);
        }

        if (Input.GetButtonDown("Fire1")) {
            bool canPlace = true;
            
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            if (results.Count > 0) {
                canPlace = false;
            }

            if(canPlace)
                gridBools[indexX, indexY] = true;
        }
    }

    void ShowGrid()
    {
        foreach (LineRenderer lineRenderer in linesHorizontal) {
            lineRenderer.enabled = true;
        }

        foreach (LineRenderer lineRenderer in linesVertical) {
            lineRenderer.enabled = true;
        }
    }

    void HideGrid()
    {
        foreach(LineRenderer lineRenderer in linesHorizontal) {
            lineRenderer.enabled = false;
        }

        foreach(LineRenderer lineRenderer in linesVertical) {
            lineRenderer.enabled = false;
        }
    }
}
