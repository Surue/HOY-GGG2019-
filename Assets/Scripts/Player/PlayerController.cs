using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using WaitForSeconds = UnityEngine.WaitForSeconds;

public class PlayerController : MonoBehaviour
{
    //Movements
    [SerializeField] float movementSpeed;

    SkeletonAnimation skeleton;

    Rigidbody2D body;
    Vector2 movementDirection;
    Collider2D boxCollider;

    bool movementLock = false;

    float timeLock = -1;

    void Start()
    {
        skeleton = GetComponentInChildren<SkeletonAnimation>();
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    
    void Update()
    {
        if (movementLock) {
            if (timeLock != -1) {
                timeLock -= Time.deltaTime;

                if (timeLock < 0) {
                    movementLock = false;
                    boxCollider.enabled = true;
                }
            }
            return;
        }

        //Input for movement
        movementDirection.x = Input.GetAxis("Horizontal");
        movementDirection.y = Input.GetAxis("Vertical");

        UpdateAnimation();

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

    void UpdateAnimation()
    {
        skeleton.loop = true;

        skeleton.transform.localScale = new Vector3(1, 1, 1);

        if (Mathf.Abs(body.velocity.y) < 0.1f && Mathf.Abs(body.velocity.x) < 0.1f) {

            skeleton.AnimationName = "idle";
            return;
        }

        if(Mathf.Abs(body.velocity.x) > 0.1f)
        {
            skeleton.AnimationName = "walkside";
            if(body.velocity.x < 0) {
                skeleton.transform.localScale = new Vector3(-1, 1, 1);
            }
            return;
        } else {
            if(body.velocity.y < 0) {
                skeleton.AnimationName = "walkdown";
            } else {
                skeleton.AnimationName = "walkup";
            }

            return;
        }
    }

    public void PlayGrabAnimation()
    {
        StartCoroutine(waitBeforeAnimation());
    }

    IEnumerator waitBeforeAnimation()
    {
        yield return new WaitForSeconds(0.75f);
        skeleton.transform.localScale = new Vector3(1, 1, 1);
        Lock(1);
        skeleton.loop = false;
        skeleton.AnimationName = "take_object";
    }

    public void Lock(int time = -1)
    {
        timeLock = time;
        skeleton.AnimationName = "idle";
        body.velocity = Vector2.zero;
        movementLock = true;
        boxCollider.enabled = false;
    }
}
