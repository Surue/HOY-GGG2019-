using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldManager : GameManager
{
    [SerializeField] TextMeshProUGUI dynamicText;
    [SerializeField] PlayerController player;
    [SerializeField] Spaceship spaceship;
    [SerializeField] CinemachineVirtualCamera cameraOverworld;

    const int MAXIMUM_OBJECT_ON_SPACESHIP = 3;

    public static OverworldManager Instance => (OverworldManager)_instance;

    public static PlayerController Player => Instance.player;

    int grabbedObject = 0;

    PickableObject pickableObject;
    
    void Start()
    {
        dynamicText.text = "0";

        PickableObject[] pickableObjects = FindObjectsOfType<PickableObject>();

        foreach (PickableObject o in pickableObjects) {
            if (InventoryManager.Instance.pickedUpObject.Contains(o.pickableObjectData)) {
                Destroy(o.gameObject);
            }
        }
    }
    
    void Update()
    {
        
    }

    public void GrabObject()
    {
        if (pickableObject && grabbedObject < MAXIMUM_OBJECT_ON_SPACESHIP) {

            spaceship.GrabObject(pickableObject);
            InventoryManager.Instance.AddObject(pickableObject.pickableObjectData);
            pickableObject.Grab();
            player.PlayGrabAnimation();

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

    public void UnlockPlayer()
    {
        player.Unlock();
    }

    public void PlayerPickedUp()
    {
        SceneManager.LoadScene("Home");
    }

    public void CameraSetObjectToFollow(Transform t)
    {
        cameraOverworld.Follow = t;
    }
}
