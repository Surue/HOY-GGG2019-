using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class PlayerController : MonoBehaviour
{

    //Movements
    [SerializeField] float movementSpeed;

    Rigidbody2D body;
    Vector2 movementDirection;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        //Input for movement
        movementDirection.x = Input.GetAxis("Horizontal");
        movementDirection.y = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        //Movement
        body.velocity = movementDirection * movementSpeed;
        movementDirection = Vector2.zero;
    }
}
