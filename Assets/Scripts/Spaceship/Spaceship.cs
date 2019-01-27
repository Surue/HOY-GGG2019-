using System;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;

public class Spaceship : MonoBehaviour
{
    //Object to follow
    Transform objectToFollow;
    [SerializeField] float heightOffset = 10;
    [SerializeField] float lerpSpeed = 5;

    [SerializeField] SpriteRenderer spriteHalo;
    [SerializeField] float speedPickUp = 5;
    [SerializeField] Sprite spriteSpaceshipFull;
    [SerializeField] SpriteRenderer spriteSpaceship;
    [SerializeField] float speedWiggle = 2;
    [SerializeField] float amountWiggle = 3;

    [Header("Sprite")]
    [SerializeField] Sprite loaded0;
    [SerializeField] Sprite loaded33;
    [SerializeField] Sprite loaded66;
    [SerializeField] Sprite loaded100;
    [SerializeField] Image imageLoaded;

    [Header("Sounds")]
    [FMODUnity.EventRef] public string laserSound;
    [FMODUnity.EventRef] public string youWeeee;

    List<PickableObject> objectsToGrab;

    bool mustGrabPlayer = false;

    int objectGrabbed = 0;

    [SerializeField] float timeFlyAway = 5;

    enum State
    {
        DROP_PLAYER,
        FOLLOW_PLAYER,
        GOES_TO_OBJECT,
        PICK_UP_OBJECT,
        PICK_UP_PLAYER,
        FLY_AWAY
    }

    State state = State.DROP_PLAYER;

    void Start()
    {
        imageLoaded.sprite = loaded0;
        imageLoaded.color = new Color(1, 1, 1, 0);

        objectToFollow = OverworldManager.Player.transform;

        transform.position = new Vector2(objectToFollow.transform.position.x,
            objectToFollow.transform.position.y + heightOffset);

        objectsToGrab = new List<PickableObject>();

        OverworldManager.Instance.LockPlayer();
        objectToFollow.position = transform.position;
    }

    void Update()
    {
        switch (state) {
            case State.DROP_PLAYER:
                objectToFollow.position = Vector2.Lerp(objectToFollow.position, (Vector2)transform.position + Vector2.down * 10, Time.deltaTime * lerpSpeed * 0.9f);
                
                if (Vector2.Distance(transform.position, objectToFollow.transform.position) > 9) {
                    state = State.FOLLOW_PLAYER;
                    OverworldManager.Instance.UnlockPlayer();
                    spriteHalo.color = new Color(1, 1, 1, 0);
                    imageLoaded.color = new Color(1, 1, 1, 1);
                }

                break;
            case State.FOLLOW_PLAYER: {
                if (mustGrabPlayer && Vector2.Distance(transform.position, objectToFollow.position + Vector3.up * heightOffset) < 1f) {
                    state = State.PICK_UP_PLAYER;
                    spriteHalo.color = new Color(1, 1, 1, 0.9f);
                    SoundManager.Instance.PlaySingle(laserSound, transform.position, true);
                    OverworldManager.Instance.LockPlayer();
                }

                Vector2 desiredPosition = new Vector2(objectToFollow.transform.position.x,
                    objectToFollow.transform.position.y + heightOffset);
                transform.position = Vector2.Lerp(transform.position, desiredPosition, Time.deltaTime * lerpSpeed);

                if (objectsToGrab.Count > 0 && objectsToGrab[0]!= null) {
                    state = State.GOES_TO_OBJECT;
                    objectToFollow = objectsToGrab[0].transform;
                    objectsToGrab.RemoveAt(0);
                }
            }
                break;
            case State.GOES_TO_OBJECT: {
                if (Vector2.Distance(transform.position, objectToFollow.position + Vector3.up * heightOffset) < 1f) {
                    state = State.PICK_UP_OBJECT;
                    spriteHalo.color = new Color(1, 1, 1, 0.9f);
                        
                    SoundManager.Instance.PlaySingle(laserSound, transform.position, true);

                    if (objectToFollow.gameObject.GetComponents<Collider2D>().Length > 0) {
                        foreach (Collider2D component in objectToFollow.GetComponents<Collider2D>()) {
                            component.enabled = false;
                        }
                    }
                } else {
                    Vector2 desiredPosition = new Vector2(objectToFollow.transform.position.x,
                        objectToFollow.transform.position.y + heightOffset);
                    transform.position = Vector2.Lerp(transform.position, desiredPosition, Time.deltaTime * lerpSpeed);
                }
            }
                break;
            case State.PICK_UP_OBJECT:
                if (Vector2.Distance(transform.position, objectToFollow.position) < 1f) {
                    Destroy(objectToFollow.gameObject);

                    objectToFollow = OverworldManager.Player.transform;

                    spriteHalo.color = new Color(1, 1, 1, 0);

                    objectGrabbed++;

                    switch (objectGrabbed) {
                        case 1:
                            imageLoaded.sprite = loaded33;
                            heightOffset -= 1;
                            speedWiggle = 10;
                            break;

                        case 2:
                            imageLoaded.sprite = loaded66;
                            heightOffset -= 1;
                            speedWiggle = 25;
                            break;

                        case 3:
                            imageLoaded.sprite = loaded100;
                            heightOffset -= 1;
                            speedWiggle = 50;
                            break;
                    }

                    state = State.FOLLOW_PLAYER;
                } else {
                    objectToFollow.position = Vector3.Lerp(objectToFollow.position, transform.position, Time.deltaTime * speedPickUp);
                    objectToFollow.localScale = Vector3.Lerp(objectToFollow.localScale, new Vector3(0.1f, 0.1f, 0.1f),
                        Time.deltaTime * speedPickUp);
                }
                break;
            case State.PICK_UP_PLAYER:
                if(Vector2.Distance(transform.position, objectToFollow.position) < 1f) {
                    state = State.FLY_AWAY;
                    OverworldManager.Instance.CameraSetObjectToFollow(transform);
                    SoundManager.Instance.PlaySingle(youWeeee, transform.position, true);
                    spriteSpaceship.sprite = spriteSpaceshipFull;
                    objectToFollow.parent = transform;
                    spriteHalo.color = new Color(1, 1, 1, 0);
                    Destroy(objectToFollow.gameObject);
                    imageLoaded.color = new Color(1, 1, 1, 0);
                } else {
                    objectToFollow.position = Vector3.Lerp(objectToFollow.position, transform.position, Time.deltaTime * speedPickUp);
                }
                break;
            case State.FLY_AWAY:
                transform.position += (Vector3)(Vector2.one * speedPickUp * 5 * Time.deltaTime);
                transform.localScale =
                    Vector3.Lerp(transform.localScale, new Vector3(0.2f, 0.2f, 0.2f), Time.deltaTime);
                timeFlyAway -= Time.deltaTime;
                if (timeFlyAway < 0) {
                    OverworldManager.Instance.PlayerPickedUp();
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        spriteSpaceship.transform.eulerAngles = new Vector3(0, 0, Mathf.Sin(Time.time * speedWiggle) * amountWiggle);
    }

    public void GrabObject(PickableObject o)
    {
        objectsToGrab.Add(o);
    }

    public void MustGrabPlayer()
    {
        mustGrabPlayer = true;
        speedPickUp++;
    }
}