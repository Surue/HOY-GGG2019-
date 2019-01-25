using UnityEngine;

public class GameManager : MonoBehaviour
{
    protected static GameManager _instance = null;

    protected void Awake() {
        _instance = this;
    }

    protected void OnDestroy() {
        _instance = null;
    }
}
