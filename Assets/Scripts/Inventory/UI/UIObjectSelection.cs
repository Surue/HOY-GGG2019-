using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIObjectSelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] Image overImage;
    Color defaultColorOver;
    [SerializeField] Image selectedImage;
    Color defaultColorSelected;

    public PickableObjectData pickableObjectData;

    bool canBePlaced = true;

    void Start()
    {
        defaultColorOver = overImage.color;
        overImage.color = new Color(1, 1, 1, 0);

        defaultColorSelected = selectedImage.color;
        selectedImage.color = new Color(1, 1, 1, 0);

        StartCoroutine(waitPickableData());
    }

    IEnumerator waitPickableData()
    {
        while (pickableObjectData == null) {

            yield return new WaitForFixedUpdate();
        }

        canBePlaced = !pickableObjectData.isPlaced;

        if (!canBePlaced) {
            GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
    }

    bool canBeClicked = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!canBePlaced) return;
        canBeClicked = true;
        overImage.color = defaultColorOver;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!canBePlaced) return;
        canBeClicked = false;
        overImage.color = new Color(1,1 ,1,0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!canBePlaced) return;
        selectedImage.color = defaultColorSelected;
        InventoryManager.Instance.SetSelectedObject(this);
    }

    public void UnselectObject()
    {
        selectedImage.color = new Color(1, 1, 1, 0);
    }

    public void SetPlaced()
    {
        canBePlaced = false;
        GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        selectedImage.color = new Color(1, 1, 1, 0);
        overImage.color = new Color(1, 1, 1, 0);
    }
}
