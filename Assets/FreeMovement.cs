using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FreeMovement : MonoBehaviour
{
    Rigidbody2D body;

    Vector2 directionMovement;

    [SerializeField] float speedMovement;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        directionMovement.x = Input.GetAxis("Horizontal");
        directionMovement.y = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        body.velocity = directionMovement * speedMovement;
        directionMovement = Vector2.zero;
    }
}
