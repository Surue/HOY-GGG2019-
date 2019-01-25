using TMPro;
using UnityEngine;

public class OverworldManager : GameManager
{
    [SerializeField] TextMeshProUGUI dynamicText;
    [SerializeField] PlayerController player;
    [SerializeField] Spaceship spaceship;
    const int MAXIMUM_OBJECT_ON_SPACESHIP = 3;

    public static OverworldManager Instance => (OverworldManager)_instance;

    int grabbedObject = 0;
    
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
