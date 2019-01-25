using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    //Object to follow
    [SerializeField] Transform objectToFollow;
    [SerializeField] float heightOffset = 10;
    [SerializeField] float lerpSpeed = 5;
    
    void Start()
    {
        transform.position = new Vector2(objectToFollow.transform.position.x, objectToFollow.transform.position.y + heightOffset);
    }
    
    void Update()
    {
        Vector2 desiredPosition = new Vector2(objectToFollow.transform.position.x, objectToFollow.transform.position.y + heightOffset);
        transform.position = Vector2.Lerp(transform.position, desiredPosition, Time.deltaTime * lerpSpeed);
    }
}
