using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryInterface : MonoBehaviour
{
    [SerializeField] Button button;

    [SerializeField] RectTransform panelUp;
    Image[] imageUp;
    [SerializeField] RectTransform panelDown;
    Image[] imageDown;

    bool placeUp = true;

    // Start is called before the first frame update
    void Start()
    {
        //Disable button
        if (button != null) {
            if (InventoryManager.Instance.pickedUpObject.Count >= 15) {
                button.interactable = false;
            }
        }

        //Get images
        if (panelUp) {
            imageUp = panelUp.GetComponentsInChildren<UIObjectSelection>()
                .Select(componentsInChild => componentsInChild.GetComponent<Image>()).ToArray();

            imageDown = panelDown.GetComponentsInChildren<UIObjectSelection>()
                .Select(componentsInChild => componentsInChild.GetComponent<Image>()).ToArray();
        }

        //Fill images
        int indexUp = 0;
        int indexDown = 0;

        foreach (PickableObjectData pickableObjectData in InventoryManager.Instance.pickedUpObject) {
            if(pickableObjectData.isPlaced) {
                InventoryManager.Instance.PlaceObjectOnGrid(pickableObjectData);
            }

            if(imageUp != null)

            if (placeUp) {
                imageUp[indexUp].sprite = pickableObjectData.sprite;
                imageUp[indexUp].preserveAspect = true;
                imageUp[indexUp].GetComponent<UIObjectSelection>().pickableObjectData = pickableObjectData;
                indexUp++;
            } else {
                imageDown[indexDown].sprite = pickableObjectData.sprite;
                imageDown[indexDown].preserveAspect = true;
                imageDown[indexDown].GetComponent<UIObjectSelection>().pickableObjectData = pickableObjectData;
                indexDown++;
            }

            placeUp = !placeUp;
        }

        if (imageUp == null) {
            return;
        }

        //Hide remaining sprite
        for (int i = indexUp; i < imageUp.Length; i++) {
            imageUp[i].color = new Color(1, 1, 1, 0);
            Destroy(imageUp[i].gameObject);
        }

        for(int i = indexDown;i < imageDown.Length; i++) {
            imageDown[i].color = new Color(1, 1, 1, 0);
            Destroy(imageDown[i].gameObject);
        }
    }

    public void ObjectPlaced()
    {
        foreach (Image image in imageUp) {
            if(image != null)
            if (image.GetComponent<UIObjectSelection>().pickableObjectData.isPlaced) {
                image.GetComponent<UIObjectSelection>().SetPlaced();
            }
        }

        foreach(Image image in imageDown) {
            if(image != null)
            if(image.GetComponent<UIObjectSelection>().pickableObjectData.isPlaced) {
                image.GetComponent<UIObjectSelection>().SetPlaced();
            }
        }
    }

    public void RemoveObject()
    {
        foreach(Image image in imageUp) {
            if(image != null)
                if(!image.GetComponent<UIObjectSelection>().pickableObjectData.isPlaced) {
                    image.GetComponent<UIObjectSelection>().Remove();
                }
        }

        foreach(Image image in imageDown) {
            if(image != null)
                if(!image.GetComponent<UIObjectSelection>().pickableObjectData.isPlaced) {
                    image.GetComponent<UIObjectSelection>().Remove();
                }
        }
    }
}
