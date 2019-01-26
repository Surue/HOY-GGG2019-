using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceMaker : MonoBehaviour
{
    static ChoiceMaker _instance = null;
    public static ChoiceMaker Instance => (ChoiceMaker)_instance;

    public enum Choice
    {
        FUN,
        RELAX,
        WEIRD,
        CLEAN,
        FOOD,
        WORK,
        LENGHT
    }

    Choice choice;

    public static Choice FinalChoice => Instance.choice;

    protected void Awake() {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (SceneManager.GetActiveScene().name == "IntroCinematic") {
            choice = (Choice)Random.Range(0, (int)Choice.LENGHT);
            FindObjectOfType<CinematicManager>().SetSprite(choice);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "IntroCinematic") {
            choice = (Choice)Random.Range(0, (int)Choice.LENGHT);
            FindObjectOfType<CinematicManager>().SetSprite(choice);
        }
    }
}
