using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldManager : GameManager
{
    [SerializeField] PlayerController player;
    [SerializeField] Spaceship spaceship;
    [SerializeField] CinemachineVirtualCamera cameraOverworld;

    [FMODUnity.EventRef] public string pickUp;

    const int MAXIMUM_OBJECT_ON_SPACESHIP = 3;

    public static OverworldManager Instance => (OverworldManager)_instance;

    public static PlayerController Player => Instance.player;

    int grabbedObject = 0;

    PickableObject pickableObject;
    
    void Start()
    {

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

            SoundManager.Instance.PlaySingle(pickUp, transform.position, true);

            spaceship.GrabObject(pickableObject);
            InventoryManager.Instance.AddObject(pickableObject.pickableObjectData);
            pickableObject.Grab();
            player.PlayGrabAnimation();

            pickableObject = null;

            grabbedObject++;

            if (grabbedObject == MAXIMUM_OBJECT_ON_SPACESHIP) {
                spaceship.MustGrabPlayer();

                foreach (PickableObject o in FindObjectsOfType<PickableObject>()) {
                    o.Grab(); 
                }
            }
        }
    }

    public void SetPickableObject(PickableObject o)
    {
        if(!InventoryManager.Instance.pickedUpObject.Contains(o.pickableObjectData))
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

    public void ExitLevel()
    {
        spaceship.MustGrabPlayer();
        Debug.Log("Exit level");
    }
}
