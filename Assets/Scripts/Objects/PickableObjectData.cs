using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PicakbleObject")]
public class PickableObjectData : ScriptableObject
{
    [SerializeField] public Sprite sprite;
    [SerializeField] public Vector2Int size;
    [SerializeField] public bool isPlaced = false;
    [SerializeField] public List<Vector2Int> tileTaken;

    [SerializeField] [Range(-5, 5)] public float FunAmout;
    [SerializeField] [Range(-5, 5)] public float RelaxingAmout;
    [SerializeField] [Range(-5, 5)] public float WeirdAmout;
    [SerializeField] [Range(-5, 5)] public float CleanAmout;
    [SerializeField] [Range(-5, 5)] public float FoodAmout;
    [SerializeField] [Range(-5, 5)] public float WorkAmout;
}
