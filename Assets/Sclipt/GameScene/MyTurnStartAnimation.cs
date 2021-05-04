using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTurnStartAnimation : MonoBehaviour
{
    [SerializeField] GameObject TurnController1Obj = null;
    [SerializeField] GameObject TurnController2Obj = null;
    [SerializeField] SoundController soundController = null;
    gTurnController1 TurnController1;
    gTurnController2 TurnController2;

    private void Awake()
    {
        TurnController1 = TurnController1Obj.GetComponent<gTurnController1>();
        TurnController2 = TurnController2Obj.GetComponent<gTurnController2>();
    }
    public void MyTurnStartAnimationOff()
    {
        if (TurnController1Obj.activeSelf)
        {
            TurnController1.NextTurnStartJudge1();
        }
        else
        {
            TurnController2.NextTurnStartJudge2();
        }
    }
    public void MyTurnStartSEOn()
    {
        soundController.MyTurnSE();
    }
}
