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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
