using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Sound Hover")]
    [FMODUnity.EventRef] public string soundHover0;
    [FMODUnity.EventRef] public string soundHover1;
    [FMODUnity.EventRef] public string soundHover2;
    [FMODUnity.EventRef] public string soundHover3;
    [FMODUnity.EventRef] public string soundHover4;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void PlayHover(int i)
    {
        switch (i) {
            case 0:
                SoundManager.Instance.PlaySingle(soundHover0, transform.position, false);
                break;
            case 1:
                SoundManager.Instance.PlaySingle(soundHover1, transform.position, false);
                break;
            case 2:
                SoundManager.Instance.PlaySingle(soundHover2, transform.position, false);
                break;
            case 3:
                SoundManager.Instance.PlaySingle(soundHover3, transform.position, false);
                break;
            case 4:
                SoundManager.Instance.PlaySingle(soundHover4, transform.position, false);
                break;
            default:
                SoundManager.Instance.PlaySingle(soundHover0, transform.position, true);
                break;
        }
    }
}
