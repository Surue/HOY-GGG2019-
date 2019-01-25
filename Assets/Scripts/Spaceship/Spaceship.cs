using System;
using System.Collections.Generic;
using UnityEngine;

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

    List<PickableObject> objectsToGrab;

    bool mustGrabPlayer = false;

    [SerializeField] float timeFlyAway = 5;

    enum State
    {
        FOLLOW_PLAYER,
        GOES_TO_OBJECT,
        PICK_UP_OBJECT,
        PICK_UP_PLAYER,
        FLY_AWAY
    }

    State state = State.FOLLOW_PLAYER;

    void Start()
    {
        objectToFollow = OverworldManager.Player.transform;

        transform.position = new Vector2(objectToFollow.transform.position.x,
            objectToFollow.transform.position.y + heightOffset);
        spriteHalo.color = new Color(1, 1, 1, 0);

        objectsToGrab = new List<PickableObject>();
    }

    void Update()
    {
        switch (state) {
            case State.FOLLOW_PLAYER: {
                if (mustGrabPlayer && Vector2.Distance(transform.position, objectToFollow.position + Vector3.up * heightOffset) < 1f) {
                    state = State.PICK_UP_PLAYER;
                    spriteHalo.color = new Color(1, 1, 1, 0.9f);
                    OverworldManager.Instance.LockPlayer();
                }

                Vector2 desiredPosition = new Vector2(objectToFollow.transform.position.x,
                    objectToFollow.transform.position.y + heightOffset);
                transform.position = Vector2.Lerp(transform.position, desiredPosition, Time.deltaTime * lerpSpeed);

                if (objectsToGrab.Count > 0) {
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
                    spriteSpaceship.sprite = spriteSpaceshipFull;
                    objectToFollow.parent = transform;
                    spriteHalo.color = new Color(1, 1, 1, 0);
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