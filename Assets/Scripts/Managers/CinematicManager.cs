using System;
using Spine.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CinematicManager : MonoBehaviour
{
    [SerializeField] GameObject animation;
    SkeletonGraphic skeleton;
    [SerializeField] Image imageForChoice;
    [Header("Sprite")]
    [SerializeField] Sprite spriteFun;
    [SerializeField] Sprite spriteRelax;
    [SerializeField] Sprite spriteWeird;
    [SerializeField] Sprite spriteClean;
    [SerializeField] Sprite spriteFood;
    [SerializeField] Sprite spriteWork;
    [SerializeField] string nextSceneName;
    [Header("Sounds")]
    [FMODUnity.EventRef] public string youIWant;
    [FMODUnity.EventRef] public string youMmmmh;
    [FMODUnity.EventRef] public string youFood;
    [FMODUnity.EventRef] public string youClean;
    [FMODUnity.EventRef] public string youFun;
    [FMODUnity.EventRef] public string youRelax;
    [FMODUnity.EventRef] public string youWork;
    [FMODUnity.EventRef] public string youWeird;
    [FMODUnity.EventRef] public string youWeeeee;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        skeleton = animation.GetComponent<SkeletonGraphic>();
    }

    public void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            animator.PlayInFixedTime("1", 0, 10);
        }
    }

    public void SetSprite(ChoiceMaker.Choice choice)
    {
        switch (choice) {
            case ChoiceMaker.Choice.FUN:
                imageForChoice.sprite = spriteFun;
                break;
            case ChoiceMaker.Choice.RELAX:
                imageForChoice.sprite = spriteRelax;
                break;
            case ChoiceMaker.Choice.WEIRD:
                imageForChoice.sprite = spriteWeird;
                break;
            case ChoiceMaker.Choice.CLEAN:
                imageForChoice.sprite = spriteClean;
                break;
            case ChoiceMaker.Choice.FOOD:
                imageForChoice.sprite = spriteFood;
                break;
            case ChoiceMaker.Choice.WORK:
                imageForChoice.sprite = spriteWork;
                break;
            case ChoiceMaker.Choice.LENGHT:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(choice), choice, null);
        }
    }

    public void PlayAnimation(string animationName)
    {
        skeleton.AnimationState.SetAnimation(0, animationName, true);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void PlaySoundIWant()
    {
        SoundManager.Instance.PlaySingle(youIWant, transform.position);
    }

    public void PlaySoundMmmmmh()
    {
        SoundManager.Instance.PlaySingle(youMmmmh, transform.position);
    }

    public void PlaySoundWeeee()
    {
        SoundManager.Instance.PlaySingle(youWeeeee, transform.position, false, GameObject.Find("Image(6)"));
    }

    public void PlaySoundDesire()
    {
        switch(ChoiceMaker.FinalChoice) {
            case ChoiceMaker.Choice.FUN:
                SoundManager.Instance.PlaySingle(youFun, transform.position);
                break;
            case ChoiceMaker.Choice.RELAX:
                SoundManager.Instance.PlaySingle(youRelax, transform.position);
                break;
            case ChoiceMaker.Choice.WEIRD:
                SoundManager.Instance.PlaySingle(youWeird, transform.position);
                break;
            case ChoiceMaker.Choice.CLEAN:
                SoundManager.Instance.PlaySingle(youClean, transform.position);
                break;
            case ChoiceMaker.Choice.FOOD:
                SoundManager.Instance.PlaySingle(youFood, transform.position);
                break;
            case ChoiceMaker.Choice.WORK:
                SoundManager.Instance.PlaySingle(youWork, transform.position);
                break;
            case ChoiceMaker.Choice.LENGHT:
                break;
        }
    }
}
