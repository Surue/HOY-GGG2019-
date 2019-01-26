using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEditorInternal;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    SkeletonAnimation skeleton;

    // Start is called before the first frame update
    void Start()
    {
        skeleton = player.GetComponent<SkeletonAnimation>();

        List<Vector2Int> listFreeSpace = FindObjectOfType<PlacementGrid>().GetListFreeSpace();

        float minDistance = float.PositiveInfinity;
        Vector2 minPosition = Vector2.zero;

        foreach (Vector2Int vector2Int in listFreeSpace) {
            if (Vector2.Distance(Vector2.zero, vector2Int) < minDistance) {
                minDistance = Vector2.Distance(Vector2.zero, vector2Int);
                minPosition = vector2Int;
            }
        }

        player.transform.position = minPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
