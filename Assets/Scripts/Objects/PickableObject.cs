using TMPro;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class PickableObject : MonoBehaviour
{
    [SerializeField] PickableObjectData pickableObjectData;

    [SerializeField] TextMeshProUGUI dynamicText;
    [SerializeField] string textToDisplay = "Pick up";

    bool playerCanGrab = false;
    bool grabbed = false;

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
        if (!other.CompareTag("Player") && !grabbed) return;
        OverworldManager.Instance.SetPickableObject(this);
        playerCanGrab = true;
        dynamicText.text = textToDisplay;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player") && !grabbed) return;
        OverworldManager.Instance.RemovePickableObject(this);
        playerCanGrab = false;
        dynamicText.text = "";
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1);
    }

    public void Grab()
    {
        dynamicText.text = "";
        grabbed = true;
    }
}
