using System;
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
        //Place a correct position
        skeleton = player.GetComponent<SkeletonAnimation>();

        List<Vector2Int> listFreeSpace = FindObjectOfType<PlacementGrid>().GetListFreeSpace();

        float minDistance = float.PositiveInfinity;
        Vector2 minPosition = new Vector2(60, 0);

        Vector2 desiredPosition = new Vector2(2, 0);

        foreach (Vector2Int vector2Int in listFreeSpace) {
            if (!(Vector2.Distance(desiredPosition, vector2Int) < minDistance)) continue;

            minDistance = Vector2.Distance(desiredPosition, vector2Int);
            minPosition = vector2Int;
        }

        player.transform.position = minPosition + Vector2.one;

        //Check if has win  
        float valueFun = 0;
        float valueRelax = 0;
        float valueWeird = 0;
        float valueClean = 0;
        float valueFood = 0;
        float valueWork = 0;

        foreach (PickableObjectData pickableObjectData in InventoryManager.Instance.pickedUpObject) {
            if (!pickableObjectData.isPlaced) continue;


            valueFun += pickableObjectData.FunAmout;
            valueRelax += pickableObjectData.RelaxingAmout;
            valueWeird += pickableObjectData.WeirdAmout;
            valueClean += pickableObjectData.CleanAmout;
            valueFood += pickableObjectData.FoodAmout;
            valueWork += pickableObjectData.WorkAmout;
        }

        bool hasWin = false;

        switch (ChoiceMaker.FinalChoice) {
            case ChoiceMaker.Choice.FUN: {
                int funWin = 0;
                if (valueFun > valueClean) {
                    funWin++;
                }

                if (valueFun > valueRelax) {
                    funWin++;
                }

                if (valueFun > valueWeird) {
                    funWin++;
                }

                if (valueFun > valueWork) {
                    funWin++;
                }

                if (valueFun > valueFood) {
                    funWin++;
                }

                if (funWin > 2) {
                    hasWin = true;
                }
            }
                break;
            case ChoiceMaker.Choice.RELAX: {
                int relaxWin = 0;
                if(valueRelax > valueClean) {
                    relaxWin++;
                }

                if(valueRelax > valueFun) {
                    relaxWin++;
                }

                if(valueRelax > valueWeird) {
                    relaxWin++;
                }

                if(valueRelax > valueWork) {
                    relaxWin++;
                }

                if(valueRelax > valueFood) {
                    relaxWin++;
                }

                if(relaxWin > 2) {
                    hasWin = true;
                }
            }
                break;
            case ChoiceMaker.Choice.WEIRD: {
                int weirdWin = 0;
                if(valueWeird > valueClean) {
                    weirdWin++;
                }

                if(valueWeird > valueFun) {
                    weirdWin++;
                }

                if(valueWeird > valueRelax) {
                    weirdWin++;
                }

                if(valueWeird > valueWork) {
                    weirdWin++;
                }

                if(valueWeird > valueFood) {
                    weirdWin++;
                }

                if(weirdWin > 2) {
                    hasWin = true;
                }
            }
                break;
            case ChoiceMaker.Choice.CLEAN: {
                int cleanWin = 0;
                if(valueClean > valueWeird) {
                    cleanWin++;
                }

                if(valueClean > valueFun) {
                    cleanWin++;
                }

                if(valueClean > valueRelax) {
                    cleanWin++;
                }

                if(valueClean > valueWork) {
                    cleanWin++;
                }

                if(valueClean > valueFood) {
                    cleanWin++;
                }

                if(cleanWin > 2) {
                    hasWin = true;
                }
            }
                break;
            case ChoiceMaker.Choice.FOOD: {
                int foodWin = 0;
                if(valueFood > valueWeird) {
                    foodWin++;
                }

                if(valueFood > valueFun) {
                    foodWin++;
                }

                if(valueFood > valueRelax) {
                    foodWin++;
                }

                if(valueFood > valueWork) {
                    foodWin++;
                }

                if(valueFood > valueClean) {
                    foodWin++;
                }

                if(foodWin > 2) {
                    hasWin = true;
                }
            }
                break;
            case ChoiceMaker.Choice.WORK: {
                int workWin = 0;
                if(valueWork > valueWeird) {
                    workWin++;
                }

                if(valueWork > valueFun) {
                    workWin++;
                }

                if(valueWork > valueRelax) {
                    workWin++;
                }

                if(valueWork > valueFood) {
                    workWin++;
                }

                if(valueWork > valueClean) {
                    workWin++;
                }

                if(workWin > 2) {
                    hasWin = true;
                }
            }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (hasWin) {
            Debug.Log("has win");
        } else {
            Debug.Log("has lose");
        }
    }
}
