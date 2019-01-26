using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using Spine.Unity.Editor;
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

    void Start()
    {
        skeleton = animation.GetComponent<SkeletonGraphic>();
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
}
