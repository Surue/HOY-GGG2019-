﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryInterface : MonoBehaviour
{
    [SerializeField] RectTransform panelUp;
    Image[] imageUp;
    [SerializeField] RectTransform panelDown;
    Image[] imageDown;

    bool placeUp = true;

    // Start is called before the first frame update
    void Start()
    {
        //Get images
        imageUp = panelUp.GetComponentsInChildren<UIObjectSelection>().Select(componentsInChild => componentsInChild.GetComponent<Image>()).ToArray();

        imageDown = panelDown.GetComponentsInChildren<UIObjectSelection>().Select(componentsInChild => componentsInChild.GetComponent<Image>()).ToArray();

        //Fill images
        int indexUp = 0;
        int indexDown = 0;

        foreach (PickableObjectData pickableObjectData in InventoryManager.Instance.pickedUpObject) {
            if(pickableObjectData.isPlaced) {
                InventoryManager.Instance.PlaceObjectOnGrid(pickableObjectData);
            }

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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ObjectPlaced()
    {
        foreach (Image image in imageUp) {
            if (image.GetComponent<UIObjectSelection>().pickableObjectData.isPlaced) {
                image.GetComponent<UIObjectSelection>().SetPlaced();
                break;
            }
        }

        foreach(Image image in imageDown) {
            if(image.GetComponent<UIObjectSelection>().pickableObjectData.isPlaced) {
                image.GetComponent<UIObjectSelection>().SetPlaced();
                break;
            }
        }
    }
}
