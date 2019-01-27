using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Experimental.UIElements;

public class UiWiggleWotation : MonoBehaviour
{

    [SerializeField] float speedWiggle = 2;
    [SerializeField] float amountWiggle = 3;
    
    float originRotation;

    // Start is called before the first frame update
    void Start()
    {
        originRotation = transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * speedWiggle) * amountWiggle + originRotation);
    }
}
