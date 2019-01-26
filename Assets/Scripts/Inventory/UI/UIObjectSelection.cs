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

    void Start()
    {
        defaultColorOver = overImage.color;
        overImage.color = new Color(1, 1, 1, 0);

        defaultColorSelected = selectedImage.color;
        selectedImage.color = new Color(1, 1, 1, 0);
    }

    bool canBeClicked = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        canBeClicked = true;
        overImage.color = defaultColorOver;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canBeClicked = false;
        overImage.color = new Color(1,1 ,1,0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selectedImage.color = defaultColorSelected;
        InventoryManager.Instance.SetSelectedObject(this);
    }

    public void UnselectObject()
    {
        selectedImage.color = new Color(1, 1, 1, 0);
    }
}
