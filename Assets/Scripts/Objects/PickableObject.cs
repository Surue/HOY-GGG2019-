using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PickableObject : MonoBehaviour
{
    [SerializeField] PickableObjectData pickableObjectData;

    [SerializeField] TextMeshProUGUI dynamicText;
    [SerializeField] string textToDisplay = "Pick up";

    bool playerCanGrab = false;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = pickableObjectData.sprite;

        dynamicText.text = "";
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerCanGrab = true;
        dynamicText.text = textToDisplay;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerCanGrab = false;
        dynamicText.text = "";
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
