using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlacementGrid : MonoBehaviour
{
    bool[,] gridBools;

    List<LineRenderer> linesHorizontal;
    List<LineRenderer> linesVertical;

    [SerializeField] Material materialLines;
    [SerializeField] Vector2 offsetGrid;

    bool gridDisplayed = false;

    [SerializeField] SpriteRenderer ghostObject;

    [SerializeField] GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    [SerializeField] EventSystem m_EventSystem;

    [SerializeField] bool defaultShowGrid = true;

    List<GameObject> placedObjects;

    // Start is called before the first frame update
    void Start()
    {
        if (!defaultShowGrid) {
            materialLines = null;
        }

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

        for (int j = 0; j <= gridBools.GetLength(1); j++) {
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

        placedObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!defaultShowGrid) {
            return;
        }

        #region Get position and offset

        Vector3 pos = Input.mousePosition;
        pos.z = 20;
        pos = Camera.main.ScreenToWorldPoint(pos);

        bool cellOffsetDown = true;
        bool cellOffsetLeft = true;

        int indexX = -1, indexY = -1;
        for (int i = 0; i < linesHorizontal.Count - 1; i++) {
            if (!(pos.y > linesHorizontal[i].GetPosition(0).y) ||
                !(pos.y <= linesHorizontal[i + 1].GetPosition(0).y)) continue;
            indexY = i;

            if (pos.y > linesHorizontal[i].GetPosition(0).y + 1) {
                cellOffsetLeft = false;
            }

            break;
        }

        for (int i = 0; i < linesVertical.Count - 1; i++) {
            if (!(pos.x > linesVertical[i].GetPosition(0).x) ||
                !(pos.x <= linesVertical[i + 1].GetPosition(0).x)) continue;
            indexX = i;

            if (pos.x > linesVertical[i].GetPosition(0).x + 1) {
                cellOffsetDown = false;
            }

            break;
        }

        if (indexX < 0 || indexX > linesVertical.Count || indexY < 0 || indexY > linesHorizontal.Count) {
            if (!gridDisplayed) return;
            HideGrid();
            gridDisplayed = false;

            ghostObject.color = new Color(1, 1, 1, 0);
            return;
        }

        #endregion

        if (!gridDisplayed) {
            ShowGrid();
            gridDisplayed = true;
        }

        #region Show ghost

        Vector2Int size = Vector2Int.one;
        bool canPlace = true;

        //Show ghost
        if (InventoryManager.Instance.selectedObjectForPlacement) {
            size = InventoryManager.Instance.selectedObjectForPlacement.pickableObjectData.size;
            if (TestIfFree(indexX, indexY, cellOffsetDown, cellOffsetLeft)) {
                ghostObject.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            } else {
                canPlace = false;
                ghostObject.color = new Color(1, 0.1f, 0.1f, 1.0f);
            }

            ghostObject.sprite = InventoryManager.Instance.selectedObjectForPlacement.pickableObjectData.sprite;

            float posX = indexX * 2 + 1;
            if (size.x == 2) {
                if (cellOffsetDown) {
                    posX += -1;
                } else {
                    posX += 1;
                }
            }

            float posY = indexY * 2 + 1;
            if (size.y == 2) {
                if (cellOffsetLeft) {
                    posY += -1;
                } else {
                    posY += 1;
                }
            }

            ghostObject.transform.position = offsetGrid + new Vector2(posX, posY);
        } else {
            canPlace = false;
            ghostObject.color = new Color(1, 1, 1, 0);

            if (Input.GetButtonDown("Fire2")) {
                if (gridBools[indexX, indexY]) {
                    InventoryManager.Instance.RemoveObjectAt(new Vector2Int(indexX, indexY));
                }
            }
        }

        if (!Input.GetButtonDown("Fire1") || !canPlace) return;
        //Raycast to check if hit the UI
        m_PointerEventData = new PointerEventData(m_EventSystem) {position = Input.mousePosition};

        List<RaycastResult> results = new List<RaycastResult>();

        m_Raycaster.Raycast(m_PointerEventData, results);

        if (results.Count > 0) {
            canPlace = false;
        }

        #endregion
        
        //Lock place
        if (!canPlace) return;
        PlaceObject(indexX, indexY, cellOffsetDown, cellOffsetLeft);
    }

    void PlaceObject(int indexX, int indexY, bool offDown, bool offLeft)
    {
        Vector2Int size = InventoryManager.Instance.selectedObjectForPlacement.pickableObjectData.size;

        gridBools[indexX, indexY] = true;
        InventoryManager.Instance.selectedObjectForPlacement.pickableObjectData.tileTaken.Add(new Vector2Int(indexX, indexY));

        if(size.x == 2) {
            if(offDown) {
                gridBools[indexX - 1, indexY] = true;
                InventoryManager.Instance.selectedObjectForPlacement.pickableObjectData.tileTaken.Add(new Vector2Int(indexX - 1, indexY));
                if(size.y == 2) {
                    if(offLeft) {
                        gridBools[indexX, indexY - 1] = true;
                        gridBools[indexX - 1, indexY - 1] = true;
                        InventoryManager.Instance.selectedObjectForPlacement.pickableObjectData.tileTaken.Add(new Vector2Int(indexX, indexY - 1));
                        InventoryManager.Instance.selectedObjectForPlacement.pickableObjectData.tileTaken.Add(new Vector2Int(indexX - 1, indexY - 1));
                    } else {
                        gridBools[indexX, indexY + 1] = true;
                        gridBools[indexX - 1, indexY + 1] = true;
                        InventoryManager.Instance.selectedObjectForPlacement.pickableObjectData.tileTaken.Add(new Vector2Int(indexX, indexY + 1));
                        InventoryManager.Instance.selectedObjectForPlacement.pickableObjectData.tileTaken.Add(new Vector2Int(indexX - 1, indexY + 1));
                    }
                }
            } else {
                gridBools[indexX + 1, indexY] = true;
                InventoryManager.Instance.selectedObjectForPlacement.pickableObjectData.tileTaken.Add(new Vector2Int(indexX + 1, indexY));
                if(size.y == 2) {
                    if(offLeft) {
                        gridBools[indexX, indexY - 1] = true;
                        InventoryManager.Instance.selectedObjectForPlacement.pickableObjectData.tileTaken.Add(new Vector2Int(indexX, indexY - 1));
                        InventoryManager.Instance.selectedObjectForPlacement.pickableObjectData.tileTaken.Add(new Vector2Int(indexX + 1, indexY - 1));
                        gridBools[indexX + 1, indexY - 1] = true;
                    } else {
                        gridBools[indexX, indexY + 1] = true;
                        gridBools[indexX + 1, indexY + 1] = true;
                        InventoryManager.Instance.selectedObjectForPlacement.pickableObjectData.tileTaken.Add(new Vector2Int(indexX, indexY + 1));
                        InventoryManager.Instance.selectedObjectForPlacement.pickableObjectData.tileTaken.Add(new Vector2Int(indexX + 1, indexY + 1));
                    }
                }
            }
        } else if(size.y == 2) {
            if(offLeft) {
                gridBools[indexX, indexY - 1] = true;
                InventoryManager.Instance.selectedObjectForPlacement.pickableObjectData.tileTaken.Add(new Vector2Int(indexX, indexY - 1));
            } else {
                gridBools[indexX, indexY + 1] = true;
                InventoryManager.Instance.selectedObjectForPlacement.pickableObjectData.tileTaken.Add(new Vector2Int(indexX, indexY + 1));
            }
        }

        GameObject o = new GameObject {
            name = InventoryManager.Instance.selectedObjectForPlacement.pickableObjectData.name
        };
        o.transform.position = ghostObject.transform.position;
        o.AddComponent<SpriteRenderer>();
        o.GetComponent<SpriteRenderer>().sprite = ghostObject.sprite;

        placedObjects.Add(o);

        InventoryManager.Instance.selectedObjectForPlacement.pickableObjectData.isPlaced = true;
        InventoryManager.Instance.ObjectPlaced();
    }

    public void PlaceObjectOnGrid(PickableObjectData data)
    {
        Vector2 pos = Vector2.zero;
        foreach (Vector2Int vector2Int in data.tileTaken) {
            gridBools[vector2Int.x, vector2Int.y] = true;
            pos += new Vector2(vector2Int.x * 2, vector2Int.y * 2);
        }

        pos /= data.tileTaken.Count;
        pos += offsetGrid;

        GameObject o = new GameObject {
            name = data.name
        };
        o.transform.position = pos + Vector2.one;
        o.AddComponent<SpriteRenderer>();
        o.GetComponent<SpriteRenderer>().sprite = data.sprite;

        placedObjects.Add(o);
    }

    bool TestIfFree(int indexX, int indexY, bool offDown, bool offLeft)
    {
        Vector2Int size = InventoryManager.Instance.selectedObjectForPlacement.pickableObjectData.size;

        if (size == Vector2Int.one) {
            if (gridBools[indexX, indexY]) {
                return false;
            } else {
                return true;
            }
        } else if (size == new Vector2Int(1, 2)) {
            if (offLeft) {
                if (gridBools[indexX, indexY] || gridBools[indexX, indexY - 1]) {
                    return false;
                } else {
                    return true;
                }
            } else {
                if (gridBools[indexX, indexY] || gridBools[indexX, indexY + 1]) {
                    return false;
                } else {
                    return true;
                }
            }
        } else if(size == new Vector2Int(2, 1)){
            if(offDown) {
                if(gridBools[indexX, indexY] || gridBools[indexX - 1, indexY]) {
                    return false;
                } else {
                    return true;
                }
            } else {
                if(gridBools[indexX, indexY] || gridBools[indexX + 1, indexY ]) {
                    return false;
                } else {
                    return true;
                }
            }
        }else if (size == new Vector2Int(2, 2)) {
            if(offLeft) {
                if(gridBools[indexX, indexY] || gridBools[indexX, indexY - 1]) {
                    return false;
                } else {
                    if(offDown) {
                        if(gridBools[indexX - 1, indexY] || gridBools[indexX - 1, indexY - 1]) {
                            return false;
                        } else {
                            return true;
                        }
                    } else {
                        if(gridBools[indexX + 1, indexY] || gridBools[indexX + 1, indexY - 1]) {
                            return false;
                        } else {
                            return true;
                        }
                    }
                }
            } else {
                if(gridBools[indexX, indexY] || gridBools[indexX, indexY + 1]) {
                    return false;
                } else {
                    if(offDown) {
                        if(gridBools[indexX - 1, indexY] || gridBools[indexX - 1, indexY + 1]) {
                            return false;
                        } else {
                            return true;
                        }
                    } else {
                        if(gridBools[indexX + 1, indexY] || gridBools[indexX + 1, indexY + 1]) {
                            return false;
                        } else {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
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
        foreach (LineRenderer lineRenderer in linesHorizontal) {
            lineRenderer.enabled = false;
        }

        foreach (LineRenderer lineRenderer in linesVertical) {
            lineRenderer.enabled = false;
        }
    }

    public void FreeSpaceFromObject(PickableObjectData d)
    {
        foreach (Vector2Int vector2Int in d.tileTaken) {
            gridBools[vector2Int.x, vector2Int.y] = false;
        }

        foreach (GameObject placedObject in placedObjects) {
            if (placedObject.GetComponent<SpriteRenderer>().sprite == d.sprite) {
                Destroy(placedObject);
                placedObjects.Remove(placedObject);
                return;
            }
        }
    }
}