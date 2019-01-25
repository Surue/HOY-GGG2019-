using System;
using System.Collections;
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

    List<PickableObject> objectsToGrab;

    bool mustGrabPlayer = false;

    enum State
    {
        FOLLOW_PLAYER,
        GOES_TO_OBJECT,
        PICK_UP_OBJECT,
        PICK_UP_PLAYER
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
                if (mustGrabPlayer && (transform.position.x - objectToFollow.transform.position.x) < 0.5f) {
                    state = State.PICK_UP_PLAYER;
                    spriteHalo.color = new Color(1, 1, 1, 0.5f);
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
                if (Mathf.Abs(transform.position.x - objectToFollow.transform.position.x) < 0.5f) {
                    state = State.PICK_UP_OBJECT;
                    spriteHalo.color = new Color(1, 1, 1, 0.5f);
                } else {
                    Vector2 desiredPosition = new Vector2(objectToFollow.transform.position.x,
                        objectToFollow.transform.position.y + heightOffset);
                    transform.position = Vector2.Lerp(transform.position, desiredPosition, Time.deltaTime * lerpSpeed);
                }
            }
                break;
            case State.PICK_UP_OBJECT:
                if ((transform.position.y - objectToFollow.transform.position.y) < 0.5f) {
                    Destroy(objectToFollow.gameObject);

                    objectToFollow = OverworldManager.Player.transform;

                    spriteHalo.color = new Color(1, 1, 1, 0);
                    state = State.FOLLOW_PLAYER;
                } else {
                    objectToFollow.position += Vector3.up * Time.deltaTime * speedPickUp;
                }
                break;
            case State.PICK_UP_PLAYER:
                if((transform.position.y - objectToFollow.transform.position.y) < 0.5f) {
                    OverworldManager.Instance.PlayerPickedUp();
                } else {
                    objectToFollow.position += Vector3.up * Time.deltaTime * speedPickUp;
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
    }
}