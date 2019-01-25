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

    bool movementLock = false;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (movementLock) return;

        //Input for movement
        movementDirection.x = Input.GetAxis("Horizontal");
        movementDirection.y = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Fire1")) {
            OverworldManager.Instance.GrabObject();
        }
    }

    void FixedUpdate()
    {
        //Movement
        body.velocity = movementDirection * movementSpeed;
        movementDirection = Vector2.zero;
    }

    public void Lock()
    {
        movementLock = true;
    }
}
