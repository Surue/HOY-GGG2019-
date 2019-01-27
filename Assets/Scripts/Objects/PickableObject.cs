using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class PickableObject : MonoBehaviour
{
    public PickableObjectData pickableObjectData;

    [SerializeField] Image dynamicImage;

    bool playerCanGrab = false;
    bool grabbed = false;

    SpriteRenderer spriteRenderer;

    float originDistance;

    Transform playerTransform;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = pickableObjectData.sprite;

        dynamicImage.color = new Color(0, 0, 0, 0);

        originDistance = GetComponent<CircleCollider2D>().radius;
        playerTransform = OverworldManager.Player.transform;
    }

    void Update()
    {
        if (grabbed || playerTransform == null) return; 

        if (Vector2.Distance(playerTransform.position, transform.position) < originDistance) {
            transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            dynamicImage.color = new Color(1, 1, 1, 1);
            playerCanGrab = true;
        } else {
            transform.localScale = new Vector3(1f, 1f, 1f);
            dynamicImage.color = new Color(0, 0, 0, 0);
            playerCanGrab = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") && !grabbed) return;
        OverworldManager.Instance.SetPickableObject(this);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player") && !grabbed) return;
        OverworldManager.Instance.RemovePickableObject(this);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1);
    }

    public void Grab()
    {
        dynamicImage.color = new Color(0, 0, 0, 0);
        grabbed = true;
    }
}
