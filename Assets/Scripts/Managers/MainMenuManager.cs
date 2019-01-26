using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : GameManager
{
    public static MainMenuManager Instance => (MainMenuManager)_instance;

    [SerializeField] RectTransform panelMain;
    [SerializeField] RectTransform panelCredits;

    [SerializeField] float speedSwitchPanel = 5;

    enum CurrentPanel
    {
        MAIN,
        CREDITS
    }

    CurrentPanel currentPanel = CurrentPanel.MAIN;

    enum State
    {
        IDLE,
        MOVE_LEFT,
        MOVE_RIGHT
    }

    State state = State.IDLE;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case State.IDLE:
                break;
            case State.MOVE_LEFT:
                panelCredits.localPosition += Vector3.left * speedSwitchPanel;
                panelMain.localPosition += Vector3.left * speedSwitchPanel;

                if (panelCredits.localPosition.x <= 0) {
                    panelCredits.localPosition = new Vector3(0, panelCredits.localPosition.y);
                    panelMain.localPosition = new Vector3(-1920, panelMain.localPosition.y);
                    state = State.IDLE;
                }
                break;
            case State.MOVE_RIGHT:
                panelCredits.position += Vector3.right * speedSwitchPanel;
                panelMain.position += Vector3.right * speedSwitchPanel;

                if(panelMain.localPosition.x >= 0) {
                    panelCredits.localPosition = new Vector3(1920, panelCredits.localPosition.y);
                    panelMain.localPosition = new Vector3(0, panelMain.localPosition.y);
                    state = State.IDLE;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SwitchPanel()
    {
        switch (currentPanel) {
            case CurrentPanel.MAIN:
                currentPanel = CurrentPanel.CREDITS;
                state = State.MOVE_LEFT;
                break;
            case CurrentPanel.CREDITS:
                currentPanel = CurrentPanel.MAIN;
                state = State.MOVE_RIGHT;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
