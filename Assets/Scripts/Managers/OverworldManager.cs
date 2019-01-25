using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldManager : GameManager
{
    [SerializeField] TextMeshProUGUI dynamicText;
    [SerializeField] PlayerController player;
    [SerializeField] Spaceship spaceship;
    const int MAXIMUM_OBJECT_ON_SPACESHIP = 3;

    public static OverworldManager Instance => (OverworldManager)_instance;

    public static PlayerController Player => Instance.player;

    int grabbedObject = 0;

    PickableObject pickableObject;
    
    void Start()
    {
        dynamicText.text = "0";
    }
    
    void Update()
    {
        
    }

    public void GrabObject()
    {
        if (pickableObject && grabbedObject < MAXIMUM_OBJECT_ON_SPACESHIP) {

            spaceship.GrabObject(pickableObject);
            pickableObject.Grab();

            grabbedObject++;
            dynamicText.text = grabbedObject.ToString();

            if (grabbedObject == MAXIMUM_OBJECT_ON_SPACESHIP) {
                spaceship.MustGrabPlayer();
            }
        }
    }

    public void SetPickableObject(PickableObject o)
    {
        pickableObject = o;
    }

    public void RemovePickableObject(PickableObject o)
    {
        if (pickableObject == o) {
            pickableObject = null;
        }
    }

    public void LockPlayer()
    {
        player.Lock();
    }

    public void PlayerPickedUp()
    {
        SceneManager.LoadScene("Hub");
    }
}
