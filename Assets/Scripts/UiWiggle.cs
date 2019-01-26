using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiWiggle : MonoBehaviour
{
    Vector2 originPos;

    [SerializeField] float amplitude;
    [SerializeField] float speed;
    Vector2 nextPos;

    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        nextPos = originPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, nextPos) < 1) {
            nextPos = originPos + Random.insideUnitCircle * amplitude;
        } else {
            transform.position = Vector2.Lerp(transform.position, nextPos, Time.deltaTime * speed);
        }
        
    }
}
