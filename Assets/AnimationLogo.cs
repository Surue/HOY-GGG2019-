using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationLogo : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = curve.Evaluate(Time.time) * Vector3.one;
    }
}
