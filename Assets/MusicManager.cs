using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    static MusicManager _instance = null;
    public static MusicManager Instance => (MusicManager)_instance;

    protected void Awake() {
        if(_instance == null) {
            DontDestroyOnLoad(this);
            _instance = this;
        } else if(_instance != this) {
            Destroy(gameObject);
        }
    }

    [Header("Musics")]
    [FMODUnity.EventRef] public string musicMenu;
    [FMODUnity.EventRef] public string musicGame;

    bool isPlayingMusicGame = false;

    EventInstance eventSound;


    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if(SceneManager.GetActiveScene().name == "MainMenu") {
            eventSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            eventSound = SoundManager.Instance.PlaySingle(musicMenu, transform.position);
            isPlayingMusicGame = false;
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu") {
            eventSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            eventSound = SoundManager.Instance.PlaySingle(musicMenu, transform.position);
            isPlayingMusicGame = false;
        }

        if (scene.name == "HUB" && !isPlayingMusicGame) {
            eventSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            eventSound = SoundManager.Instance.PlaySingle(musicGame, transform.position);
            isPlayingMusicGame = true;
        }
    }
}
