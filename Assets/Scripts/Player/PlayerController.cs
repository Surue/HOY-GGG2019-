using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WaitForSeconds = UnityEngine.WaitForSeconds;

public class PlayerController : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] GameObject bubble;
    [SerializeField] Image imageChoice;
    [SerializeField] Sprite spriteFun;
    [SerializeField] Sprite spriteRelax;
    [SerializeField] Sprite spriteWeird;
    [SerializeField] Sprite spriteClean;
    [SerializeField] Sprite spriteFood;
    [SerializeField] Sprite spriteWork;

    [Header("Movements")]
    //Movements
    [SerializeField] float movementSpeed;

    SkeletonAnimation skeleton;

    Rigidbody2D body;
    Vector2 movementDirection;
    Collider2D boxCollider;

    [Header("Raycast UI")]
    [SerializeField] GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    [SerializeField] EventSystem m_EventSystem;

    bool movementLock = false;

    float timeLock = -1;

    [Header("Sounds")]
    [FMODUnity.EventRef] public string youHoy;

    bool mouseOver = false;

    void Awake()
    {
        skeleton = GetComponentInChildren<SkeletonAnimation>();
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<CapsuleCollider2D>();
    }

    void Start()
    {
        switch (ChoiceMaker.FinalChoice) {
            case ChoiceMaker.Choice.FUN:
                imageChoice.sprite = spriteFun;
                break;
            case ChoiceMaker.Choice.RELAX:
                imageChoice.sprite = spriteRelax;
                break;
            case ChoiceMaker.Choice.WEIRD:
                imageChoice.sprite = spriteWeird;
                break;
            case ChoiceMaker.Choice.CLEAN:
                imageChoice.sprite = spriteClean;
                break;
            case ChoiceMaker.Choice.FOOD:
                imageChoice.sprite = spriteFood;
                break;
            case ChoiceMaker.Choice.WORK:
                imageChoice.sprite = spriteWork;
                break;
        }

        bubble.gameObject.SetActive(false);
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
            //Raycast to check if hit the UI
            m_PointerEventData = new PointerEventData(m_EventSystem) { position = Input.mousePosition };

            List<RaycastResult> results = new List<RaycastResult>();

            m_Raycaster.Raycast(m_PointerEventData, results);

            if (results.Count <= 0) {
                OverworldManager.Instance.GrabObject();
            }
        }
    }

    void OnMouseOver()
    {
        mouseOver = true;
    }

    void FixedUpdate()
    {
        //Movement
        body.velocity = movementDirection * movementSpeed;
        movementDirection = Vector2.zero;

        if(mouseOver) {
            bubble.gameObject.SetActive(true);
            mouseOver = false;
        } else {
            bubble.gameObject.SetActive(false);
        }
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
        Lock(0.75f);
        skeleton.loop = false;
        skeleton.AnimationName = "take_object";
        SoundManager.Instance.PlaySingle(youHoy, transform.position, true);
    }

    public void Lock(float time = -1)
    {
        timeLock = time;
        skeleton.AnimationName = "idle";
        body.velocity = Vector2.zero;
        movementLock = true;
        boxCollider.enabled = false;
    }

    public void Unlock()
    {
        movementLock = false;
        boxCollider.enabled = true;
        timeLock = -1;
    }
}
