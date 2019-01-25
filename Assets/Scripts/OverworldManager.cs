using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OverworldManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dynamicText;
    [SerializeField] PlayerController player;
    [SerializeField] Spaceship spaceship;
    const int MAXIMUM_OBJECT_ON_SPACESHIP = 3;

    static OverworldManager _instance = null;

    public static OverworldManager Instance => (OverworldManager)_instance;

    int grabbedObject = 0;

    void Awake() {
        _instance = this;
    }

    void OnDestroy() {
        _instance = null;
    }
    
    void Start()
    {
        dynamicText.text = "0";
    }
    
    void Update()
    {
        
    }

    public void GrabObject()
    {
        grabbedObject++;
        dynamicText.text = grabbedObject.ToString();

        if (grabbedObject == MAXIMUM_OBJECT_ON_SPACESHIP) {
            Debug.Log("end game");
        }
    }
}
