using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PicakbleObject")]
public class PickableObjectData : ScriptableObject
{
    [SerializeField] public Sprite sprite;
    [SerializeField] public Vector2Int size;
    [SerializeField] public bool isPlaced = false;
    [SerializeField] public List<Vector2Int> tileTaken;
}
