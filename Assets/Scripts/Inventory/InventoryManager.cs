using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    static InventoryManager _instance;
    public static InventoryManager Instance => _instance;

    private void Awake() {
        DontDestroyOnLoad(this);
        if(_instance == null) {
            _instance = this;
        } else if(_instance != this) {
            Destroy(gameObject);
        }
    }

    public List<PickableObjectData> pickedUpObject = new List<PickableObjectData>();

    public UIObjectSelection selectedObjectForPlacement;
    
    void Update()
    {
        if (Input.GetButtonDown("Fire2") && selectedObjectForPlacement) {
            selectedObjectForPlacement.UnselectObject();
            selectedObjectForPlacement = null;
        }
    }

    public void AddObject(PickableObjectData p)
    {
        pickedUpObject.Add(p);
    }

    public void SetSelectedObject(UIObjectSelection p)
    {
        if(selectedObjectForPlacement)
        selectedObjectForPlacement.UnselectObject();

        selectedObjectForPlacement = p;
    }

    public void ObjectPlaced()
    {
        FindObjectOfType<InventoryInterface>().ObjectPlaced();
        selectedObjectForPlacement.UnselectObject();
        selectedObjectForPlacement = null;
    }

    public void PlaceObjectOnGrid(PickableObjectData d)
    {
        FindObjectOfType<PlacementGrid>().PlaceObjectOnGrid(d);
    }

    public void CleanMemory()
    {
        if (pickedUpObject != null) {

            foreach (PickableObjectData pickableObjectData in pickedUpObject) {
                pickableObjectData.isPlaced = false;
                pickableObjectData.tileTaken = new List<Vector2Int>();
            }
        }

        pickedUpObject.Clear();
    }

    public void RemoveObjectAt(Vector2Int pos)
    {
        foreach (PickableObjectData pickableObjectData in pickedUpObject) {
            if (!pickableObjectData.isPlaced) continue;

            foreach (Vector2Int vector2Int in pickableObjectData.tileTaken) {
                if (vector2Int != pos) continue;

                FindObjectOfType<PlacementGrid>().FreeSpaceFromObject(pickableObjectData);

                pickableObjectData.isPlaced = false;
                pickableObjectData.tileTaken = new List<Vector2Int>();

                FindObjectOfType<InventoryInterface>().RemoveObject();
                return;
            }
        }
    }
}
