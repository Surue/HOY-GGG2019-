using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerTransitionScene : MonoBehaviour
{
    [SerializeField] float timer = 5;
    [SerializeField] string nextScene = "";

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            SceneManager.LoadScene(nextScene);
        }

        if (timer < 0) {
            SceneManager.LoadScene(nextScene);
        } else {
            timer -= Time.deltaTime;
        }
    }
}
