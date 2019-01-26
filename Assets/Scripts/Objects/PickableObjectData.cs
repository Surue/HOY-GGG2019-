using UnityEngine;

[CreateAssetMenu(fileName = "PicakbleObject")]
public class PickableObjectData : ScriptableObject
{
    [SerializeField] public Sprite sprite;
    [SerializeField] Vector2Int size;
}
